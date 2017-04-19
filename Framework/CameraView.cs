using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
/**
 * You´re supposed to make some comments! Otherwise I´m going to kill you! :) 
 * 
 * @author: David Rosenbusch
 */
namespace RealSense
{
    public class CameraView : Form
    {
        // all the things 
        private PXCMSession session;
        private PXCMSession.ImplVersion iv;
        // Used to store Image Data and convert to bitmap
        private PXCMImage.ImageData colorData;
        // the bitmap that we put into the pictureBox
        private Bitmap colorBitmap;
        // the PictureBox that we put into the window (this class)
        private PictureBox pb;
        // running number to save all the images to the hard drive (careful with that ;) )
        private Thread updaterThread;
        private Model model;
        public int save = 0, debug_y = 0;
        private Button enableOutput = new Button();
        private Button enableImage = new Button();
        private bool outputEnabled = true, imageEnabled = true;

        /**
         * Initialise View and start updater Thread
         */
        public CameraView(Model model)
        {
            this.model = model;
            model.View = this;
            session = PXCMSession.CreateInstance();

            if (session == null) // Something went wrong, session could not be initialised
            {
                //Console.WriteLine("Fuck!");
                Application.Exit();
                return;
            }

            iv = session.QueryVersion();
            String versionString = "v" + iv.major + "." + iv.minor;
            //Console.WriteLine(versionString);
            Text = versionString;


            pb = new PictureBox();
            // Set size
            pb.Bounds = new Rectangle(0, 0, model.Width, model.Height);
            // init UI
            this.Bounds = new Rectangle(0, 0, model.Width, model.Height + 180);
            this.Controls.Add(pb);
            FormClosed += new FormClosedEventHandler(Quit);

            enableOutput.Bounds = new Rectangle(20, 1080, 500, 30);
            enableOutput.Text = "Output";
            enableOutput.Click +=
                new System.EventHandler(delegate
                {
                    System.Windows.Forms.Clipboard.SetText("" + 
                        model.getAU_Value(typeof(AU_BrowShift).ToString() + "_left") + ", "+
                        model.getAU_Value(typeof(AU_BrowShift).ToString() + "_right") + ", " +

                        model.getAU_Value(typeof(AU_InnerBrowShift).ToString() + "_left") + ", " +
                        model.getAU_Value(typeof(AU_InnerBrowShift).ToString() + "_right") + ", " +

                        model.getAU_Value(typeof(AU_LipCornerV2).ToString() + "_left") + ", " +
                        model.getAU_Value(typeof(AU_LipCornerV2).ToString() + "_right") + ", " +
                        model.getAU_Value(typeof(AU_LipCornerV2).ToString() + "_line") + ", " +

                        model.getAU_Value(typeof(AU_LipCorner).ToString() + "_left") + ", " +
                        model.getAU_Value(typeof(AU_LipCorner).ToString() + "_right") + ", " +

                        model.getAU_Value(typeof(AU_LipsTightened).ToString() + "_upperBottomLip")
                         );

                    
                    outputEnabled = !outputEnabled;
                });
            AddComponent(enableOutput);

            enableImage.Bounds = new Rectangle(20, 1110, 500, 30);
            enableImage.Text = "NoImg";
            enableImage.Click +=
                new System.EventHandler(delegate
                {
                    imageEnabled = !imageEnabled;
                });
            AddComponent(enableImage);

            this.Show();
            // Start Updater Thread
           // Console.WriteLine("Starting Thread");
            updaterThread = new Thread(this.update);
            updaterThread.Start();
        }

        /**
          * Exit Application
        */
        private void Quit(object sender, FormClosedEventArgs e)
        {
            updaterThread.Abort();
           // Console.WriteLine("Cleaning");
            session.Dispose();
            model.SenseManager.Dispose();
          //  Console.WriteLine("Closing");
            Application.Exit();
        }

        public void AddComponent(Control c)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
               {
                   AddComponent(c);
               });
            }
            else
            {
                this.Controls.Add(c);
            }
        }
        /**
         * Update the View
         */
        private void update()
        {
          //  Console.WriteLine("Update");
            //Console.Write(model.SenseManager.AcquireFrame(true));
            Stopwatch stopwatch = new Stopwatch();
            while (true)
            {
                if (model.SenseManager.AcquireFrame(true) >= pxcmStatus.PXCM_STATUS_NO_ERROR) // Dauert manchmal voll lange ...
                {
                    debug_y = 0;
                    // welcher trottel .... Console.WriteLine("While schleife");
                    // <magic>
                    PXCMCapture.Sample sample = model.SenseManager.QueryFaceSample();
                    sample.color.AcquireAccess(PXCMImage.Access.ACCESS_READ, PXCMImage.PixelFormat.PIXEL_FORMAT_RGB24, out colorData);

                    model.FaceData = model.Face.CreateOutput();
                    model.FaceData.Update();
                    model.FaceAktuell = model.FaceData.QueryFaceByIndex(0);
                    if (model.FaceAktuell != null)
                    {
                        model.Lp = model.FaceAktuell.QueryLandmarks();
                        if (model.NullFace == null)
                        {
                            if (model.Lp != null)
                            {
                                PXCMFaceData.LandmarkPoint[] aPoints;
                                model.Lp.QueryPoints(out aPoints);
                                model.NullFace = aPoints;
                            }
                        }
                    }

                    colorBitmap = colorData.ToBitmap(0, sample.color.info.width, sample.color.info.height);
                    Graphics bitmapGraphics = Graphics.FromImage(colorBitmap);
                    if(outputEnabled)
                        bitmapGraphics.FillRectangle(model.DefaultBGBrush, new Rectangle(0, 0, model.Width, model.Height));
                    if(!imageEnabled)
                        bitmapGraphics.FillRectangle(model.OpaqueBGBrush, new Rectangle(0, 0, model.Width, model.Height));
                    if (model.CurrentFace != null)
                        model.Modules.ForEach(delegate (RSModule mod)
                        {
                            mod.Work(bitmapGraphics);
                            if (outputEnabled && mod.output != "")
                            {
                                bitmapGraphics.DrawString(mod.output, model.DefaultFont, model.DefaultStringBrush, 10, Debug_Y);
                                Debug_Y += 25; // new row
                            }
                        });
                    // update PictureBox
                    pb.Image = colorBitmap;
                    model.SenseManager.ReleaseFrame();
                    model.FaceData.Dispose(); // DONE!
                    sample.color.ReleaseAccess(colorData);

                }
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CameraView
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "CameraView";
            this.Load += new System.EventHandler(this.CameraView_Load);
            this.ResumeLayout(false);

        }

        public int Debug_Y
        {
            get { return debug_y; }
            set { debug_y = value; }
        }

        private void CameraView_Load(object sender, EventArgs e)
        {

        }

 
    }
}