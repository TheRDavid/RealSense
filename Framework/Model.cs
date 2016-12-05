using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RealSense
{

    public class Model
    {
        // Reference to globally used SenseManager
        private PXCMSenseManager senseManager;
        private PXCMFaceModule face;
        private PXCMFaceData faceData;
        private PXCMFaceConfiguration faceConfig;
        public  PXCMFaceData.Face faceAktuell;
        private PXCMFaceData.ExpressionsData edata;
        private PXCMFaceData.LandmarksData lp;
        private PXCMFaceData.LandmarkPoint[] nullFace=null; //thx David
        private int anger = 0;

        private List<RSModule> modules;
        private int width;
        private int height;
        private int framerate;

        private CameraView view;

        public Model()
        {
            width = 640;
            height = 480;
            framerate = 30;
            senseManager = PXCMSenseManager.CreateInstance();
            senseManager.EnableStream(PXCMCapture.StreamType.STREAM_TYPE_COLOR, width, height, framerate);
            // Enable Face detection
            senseManager.EnableFace();
            senseManager.EnableHand();
            senseManager.Init();

            face = senseManager.QueryFace();
            faceConfig = face.CreateActiveConfiguration();
            faceConfig.SetTrackingMode(PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR);
            faceConfig.detection.isEnabled = true;
            faceConfig.QueryExpressions();
            PXCMFaceConfiguration.ExpressionsConfiguration expc = faceConfig.QueryExpressions();
            expc.Enable();
            expc.EnableAllExpressions();
            faceConfig.ApplyChanges();
            faceConfig.Update();

            modules = new List<RSModule>();
        }

        public void AddModule(RSModule m)
        {
            modules.Add(m);
        }

        public double Difference(int i01, int i02)
        {
            return 100/NullFaceBetween(i01, i02) * Between(i01, i02);
        }

        public double NullFaceBetween(int i01, int i02)
        {
            if (nullFace[i01].world.x != 0)
            {
                double a = Math.Abs(nullFace[i02].world.y - nullFace[i01].world.y);
                double b = Math.Abs(nullFace[i02].world.x - nullFace[i01].world.x);
                double c = Math.Abs(nullFace[i02].world.z - nullFace[i01].world.z);
                return Math.Sqrt(a * a + b * b + c * c);
            }
            throw new NullReferenceException();
        }

        public double Between(int i01, int i02)
        {
            PXCMFaceData.LandmarkPoint point01 = null;
            PXCMFaceData.LandmarkPoint point02 = null;

            if (lp != null)
            {
                lp.QueryPoint(i01, out point01); //AccessViolationException ...
                lp.QueryPoint(i02, out point02);

                double a = Math.Abs(point02.world.y - point01.world.y);
                double b = Math.Abs(point02.world.x - point01.world.x);
                double c = Math.Abs(point02.world.z - point01.world.z);
                return Math.Sqrt(a * a + b * b + c * c);
            }
            throw new NullReferenceException();

            /**
             * Exception error = new Exception();
            try
            {
                if (lp != null)
                {
                    lp.QueryPoint(i01, out point01);
                    lp.QueryPoint(i02, out point02);

                    double a = Math.Abs(point02.world.y - point01.world.y);
                    double b = Math.Abs(point02.world.x - point01.world.x);
                    double c = Math.Abs(point02.world.z - point01.world.z);
                    return Math.Sqrt(a * a + b * b + c * c);
                }
            }catch (Exception e)
            {
                error = e;
            }
            throw error;
    */
        }

        public List<RSModule> Modules
        {
            get { return modules; }
        }

        public PXCMSenseManager SenseManager
        {
            get { return senseManager; }
            set { senseManager = value; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public PXCMFaceModule Face
        {
            get { return face; }
        }

        public PXCMFaceData.LandmarksData Lp
        {
            get { return lp; }
            set { lp = value; }
        }

        public PXCMFaceData.LandmarkPoint[] NullFace
        {
            get { return nullFace; }
            set { nullFace = value; }
        }

        public PXCMFaceData FaceData
        {
            get { return faceData; }
            set { faceData = value; }
        }

        public PXCMFaceData.Face FaceAktuell
        {
            get { return faceAktuell; }
            set { faceAktuell = value; }
        }

        public PXCMFaceData.ExpressionsData Edata
        {
            get { return edata; }
            set { edata = value; }
        }

        public int Anger
        {
            get { return anger; }
            set { anger = value; }
        }

        public CameraView View
        {
            get { return view; }
            set { view = value; }
        }
    }
}
 