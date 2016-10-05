using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
/**
 * @author: David
 * @author:
 */
namespace RealSense
{
    class CameraView : Form
    {
        // all the things
        private PXCMSession session;
        private PXCMSenseManager senseManager;
        private PXCMSession.ImplVersion iv;
        // Used to store Image Data and convert to bitmap
        private PXCMImage.ImageData colorData;
        // the bitmap that we put into the pictureBox
        private Bitmap colorBitmap;
        // the PictureBox that we put into the window (this class)
        private PictureBox pb;
        // running number to save all the images to the hard drive (careful with that ;) )
        private int capNum = 0;
        private bool storeImages = false;
        private Thread updaterThread;
        private List<RSModule> modules = new List<RSModule>();

        public PXCMSession Session
        {
            get
            {
                return session;
            }

            // No setter for now
        }

        public PXCMSenseManager SenseManager
        {
            get
            {
                return senseManager;
            }

            // No setter for now
        }

        /**
         * Initialise View and start updater Thread
         */
        public CameraView(int width, int height, int framterate, List<RSModule> mods)
        {
            // Initialise Stuff, turn Camera on
            senseManager = PXCMSenseManager.CreateInstance();
            senseManager.EnableStream(PXCMCapture.StreamType.STREAM_TYPE_COLOR, width, height, framterate);
            // Enable Face detection
            senseManager.EnableFace();
            modules = mods;
            modules.ForEach(delegate (RSModule rsm)
            {
                rsm.Init(senseManager);
            });
            senseManager.Init();

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
            pb.Bounds = new Rectangle(0, 0, width, height);
            // init UI
            this.Bounds = new Rectangle(0, 0, width, height);
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
            senseManager.Dispose();
            Console.WriteLine("Closing");
            Application.Exit();
        }

        /**
         * Update the View
         */ 
        private void update()
        {
            while (senseManager.AcquireFrame(true) >= pxcmStatus.PXCM_STATUS_NO_ERROR) // Got an image?
            {
                // Console.WriteLine("Update");
                // <magic>
                PXCMCapture.Sample sample = senseManager.QueryFaceSample();

                sample.color.AcquireAccess(PXCMImage.Access.ACCESS_READ, PXCMImage.PixelFormat.PIXEL_FORMAT_RGB24, out colorData);

                colorBitmap = colorData.ToBitmap(0, sample.color.info.width, sample.color.info.height);
                Graphics bitmapGraphics = Graphics.FromImage(colorBitmap);
                modules.ForEach(delegate (RSModule mod)
                {
                    mod.Work(bitmapGraphics);
                });
                // </magic>
                // save to hard drive (careful!) - will be stored in project folder/bin/debug
                if (storeImages) colorBitmap.Save("cap"+capNum++ +".png");
                // update PictureBox
                pb.Image = colorBitmap;
                senseManager.ReleaseFrame();
            }
        }

    }
}
