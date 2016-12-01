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
        public PXCMFaceData.Face faceAktuell;
        private PXCMFaceData.ExpressionsData edata;
        private PXCMFaceData.LandmarksData lp;
        private PXCMFaceData.LandmarkPoint[] normalFace;

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

        /**
        public void calibrateFace()
        {
            //normalFace Werte belegen
            if (lp != null)
            {
                lp.QueryPoints(out normalFace);
            }
            Console.WriteLine("Error");
            throw new NullReferenceException();
        }*/

        public double normalFaceBetween(int i01, int i02)
        {
            if (normalFace[0] != null)
            {
                double a = Math.Abs(normalFace[i01].world.y - normalFace[i01].world.y);
                double b = Math.Abs(normalFace[i02].world.x - normalFace[i02].world.x);
                return Math.Sqrt(a * a + b * b);
            }
            throw new NullReferenceException();
        }

        public double between(int i01, int i02)
        {
            PXCMFaceData.LandmarkPoint point01 = null;
            PXCMFaceData.LandmarkPoint point02 = null;
            if (lp != null)
            {
                lp.QueryPoint(i01, out point01);
                lp.QueryPoint(i02, out point02);

                double a = Math.Abs(point01.world.y - point02.world.y);
                double b = Math.Abs(point01.world.x - point02.world.x);
                return Math.Sqrt(a * a + b * b);
            }
            throw new NullReferenceException();
        }

        /**
       public void QueryPoint(int i, out double x, out double y)  
       {
           PXCMFaceData.LandmarkPoint point = null;
           if (lp != null)
           {
               //Console.WriteLine(i);
               lp.QueryPoint(i, out point);
               x = point.world.x;
               y = point.world.y;
               return;
           }
           throw new NullReferenceException();           
       }*/

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

        public PXCMFaceData.LandmarkPoint[] NormalFace
        {
            get { return normalFace; }
            set { normalFace = value; }
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
     
        public CameraView View
        {
            get { return view; }
            set { view = value; }
        }
    }
}