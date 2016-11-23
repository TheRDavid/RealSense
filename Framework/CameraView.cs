using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
/**
 * @author: David
 * @author:
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
                Console.WriteLine("Fuck!");
                Application.Exit();
                return;
            }

            iv = session.QueryVersion();
            String versionString = "v" + iv.major + "." + iv.minor;
            Console.WriteLine(versionString);
            Text = versionString;


            pb = new PictureBox();
            // Set size
            pb.Bounds = new Rectangle(0, 0, model.Width, model.Height);
            // init UI
            this.Bounds = new Rectangle(0, 0, model.Width, model.Height+150);
            this.Controls.Add(pb);
            FormClosed += new FormClosedEventHandler(Quit);
            this.Show();
            // Start Updater Thread
            updaterThread = new Thread(this.update);
            updaterThread.Start();
        }

        /**
          * Exit Application
        */
        private void Quit(object sender, FormClosedEventArgs e)
        {
            updaterThread.Abort();
            Console.WriteLine("Cleaning");
            session.Dispose();
            model.SenseManager.Dispose();
            Console.WriteLine("Closing");
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
            while (model.SenseManager.AcquireFrame(true) >= pxcmStatus.PXCM_STATUS_NO_ERROR) // Got an image?
            {
                // <magic>
                PXCMCapture.Sample sample = model.SenseManager.QueryFaceSample();

                sample.color.AcquireAccess(PXCMImage.Access.ACCESS_READ, PXCMImage.PixelFormat.PIXEL_FORMAT_RGB24, out colorData);

                model.FaceData = model.Face.CreateOutput();
                model.FaceData.Update(); 
                model.FaceAktuell = model.FaceData.QueryFaceByIndex(0);
                if (model.FaceAktuell != null)
                {
                    model.Edata = model.FaceAktuell.QueryExpressions();
                    model.Lp = model.FaceAktuell.QueryLandmarks();
                }

                colorBitmap = colorData.ToBitmap(0, sample.color.info.width, sample.color.info.height);
                Graphics bitmapGraphics = Graphics.FromImage(colorBitmap);

                model.Modules.ForEach(delegate (RSModule mod)
                {
                    mod.Work(bitmapGraphics);
                });
                // update PictureBox
                pb.Image = colorBitmap;
                model.SenseManager.ReleaseFrame();
                model.FaceData.Dispose(); // DONE!
                model.Edata = null;
                sample.color.ReleaseAccess(colorData);
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

        private void CameraView_Load(object sender, EventArgs e)
        {

        }
    }
}