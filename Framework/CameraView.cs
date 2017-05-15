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
        public Bitmap colorBitmap;
        // the PictureBox that we put into the window (this class)
        private PictureBox pb;
        // running number to save all the images to the hard drive (careful with that ;) )
        private Thread updaterThread;
        private Model model;
        public int save = 0, debug_y = 0;
        private Button enableOutput = new Button();
        private Button enableImage = new Button();
        private bool outputEnabled, imageEnabled = true, resetModules = false;
        private bool testMode;

        /**
         * Initialise View and start updater Thread
         */
        public CameraView(Model model, bool test)
        {
            outputEnabled = test;
            testMode = test;
            this.model = model;
            model.View = this;
            session = PXCMSession.CreateInstance();

            if (session == null) // Something went wrong, session could not be initialised
            {
                Application.Exit();
                return;
            }

            iv = session.QueryVersion();
            String versionString = "v" + iv.major + "." + iv.minor;
            Text = versionString;


            pb = new PictureBox();
            FormClosed += new FormClosedEventHandler(Quit);

            if (testMode)
            {
                // Set size
                pb.Bounds = new Rectangle(0, 0, model.Width, model.Height);
                // init UI
                this.Bounds = new Rectangle(0, 0, model.Width, model.Height + 180);
                this.Controls.Add(pb);

                enableOutput.Bounds = new Rectangle(20, 1080, 500, 30);
                enableOutput.Text = "Output";
                enableOutput.Click +=
                    new System.EventHandler(delegate
                    {
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
            }
            else
            {
                this.Bounds = Screen.PrimaryScreen.Bounds;
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
                pb.Bounds = new Rectangle(this.Bounds.Width / 2 - model.Width / 2, this.Bounds.Height / 2 - model.Height / 2, 1920, 1080);
                this.Controls.Add(pb);
                this.BackColor = Color.Black;
                KeyDown += OnKeyDown;
            }
            this.Show();
            // Start Updater Thread
            updaterThread = new Thread(this.update);
            updaterThread.Start();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyValue == (int)Keys.C)
                {
                    model.Modules.ForEach(delegate (RSModule mod)
                    {
                        if (mod.GetType() == typeof(Gauge_Module))
                        {
                            Console.WriteLine("Start calibration");
                            ((Gauge_Module)mod).calibrate = true;
                        }
                    });
                    ResetModules = true;

                }
            }
        }

        /**
          * Exit Application
        */
        private void Quit(object sender, FormClosedEventArgs e)
        {
            updaterThread.Abort();
            session.Dispose();
            model.SenseManager.Dispose();
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
            Stopwatch stopwatch = new Stopwatch();
            while (true)
            {
                if (model.SenseManager.AcquireFrame(true) >= pxcmStatus.PXCM_STATUS_NO_ERROR) // Dauert manchmal voll lange ...
                {
                    debug_y = 0;
                    // <magic>
                    PXCMCapture.Sample sample = model.SenseManager.QueryFaceSample();
                    sample.color.AcquireAccess(PXCMImage.Access.ACCESS_READ, PXCMImage.PixelFormat.PIXEL_FORMAT_RGB24, out colorData);

                    model.FaceData = model.Face.CreateOutput();
                    model.FaceData.Update();
                    model.FaceAktuell = model.FaceData.QueryFaceByIndex(0);
                    if (model.FaceAktuell != null)
                    {
                        PXCMFaceData.PoseData pose = model.FaceAktuell.QueryPose();
                        if (pose != null)
                            pose.QueryPoseAngles(out model.currentPose);
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

                    if (resetModules)
                    {
                        model.Modules.ForEach(delegate (RSModule mod)
                        {
                            mod.reset();
                        });
                        resetModules = false;
                    }

                    if (testMode)
                    {
                        if (outputEnabled)
                            bitmapGraphics.FillRectangle(model.DefaultBGBrush, new Rectangle(0, 0, model.Width, model.Height));
                        if (!imageEnabled)
                            bitmapGraphics.FillRectangle(model.OpaqueBGBrush, new Rectangle(0, 0, model.Width, model.Height));
                    }


                    if (model.FaceData.QueryNumberOfDetectedFaces() > 0 && model.CurrentFace != null)
                    {
                        model.Modules.ForEach(delegate (RSModule mod)
                        {
                            mod.Work(bitmapGraphics);
                            if (outputEnabled && mod.output != "")
                            {
                                bitmapGraphics.DrawString(mod.output, model.DefaultFont, model.DefaultStringBrush, 10, Debug_Y);
                                Debug_Y += 25; // new row
                            }
                        });
                    }

                    double pitchDiff = Math.Abs(model.currentPose.pitch - model.NullPose.pitch);
                    double rollDiff = Math.Abs(model.currentPose.roll - model.NullPose.roll);
                    double yawDiff = Math.Abs(model.currentPose.yaw - model.NullPose.yaw);

                    model.CurrentPoseDiff = pitchDiff + rollDiff + yawDiff;
                    model.CurrentRollDiff = rollDiff;
                    model.CurrentPitchDiff = pitchDiff;
                    model.CurrentYawDiff = yawDiff;

                    /* bitmapGraphics.DrawString("poll: " + pitchDiff + ", roll: " + rollDiff + ", yaw: " + yawDiff, model.DefaultFont, model.DefaultStringBrush, 10, Debug_Y);
                     Debug_Y += 25;
                     bitmapGraphics.DrawString("all: " + (int)(pitchDiff + rollDiff + yawDiff), model.DefaultFont, model.DefaultStringBrush, 10, Debug_Y);
                     Debug_Y += 25;
                     bitmapGraphics.DrawString("pitch: " + pitchDiff, model.DefaultFont, model.DefaultStringBrush, 10, Debug_Y);
                     Debug_Y += 25;
                     bitmapGraphics.DrawString("yaw: " + yawDiff, model.DefaultFont, model.DefaultStringBrush, 10, Debug_Y);
                     Debug_Y += 25;
                     bitmapGraphics.DrawString("roll: " + rollDiff, model.DefaultFont, model.DefaultStringBrush, 10, Debug_Y);*/

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

        public Boolean ResetModules
        {
            set { resetModules = value; }
        }

        private void CameraView_Load(object sender, EventArgs e)
        {

        }


    }
}