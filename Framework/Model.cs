using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RealSense
{
    /**
     * Stores all of our data. It is based at the MVC Pattern
     * 
     * @author Tanja Witke
     */ 
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
        public const int ANGER = 0, FEAR = 1, SADNESS = 2;
        private int[] emotions = new int[7];
        public String[] eNames = {"Anger", "Fear", "Sadness"};  // there are actually seven

        private List<RSModule> modules;
        private int width;
        private int height;
        private int framerate;

        private CameraView view;


        /**
         * Constructor of the model 
         * It does all the important stuff to use our camera.  Its so FANCY ! 
         * Like enabling all important tracker(Hand, Face), the stream and builds up the configuration.
         */
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

        /**
         * adds a new modul to the List
         * @param RSModul m which is the new module
         */
        public void AddModule(RSModule m)
        {
            modules.Add(m);
        }


        /**
         * calculates the percentage of the difference between two points
         * @param i01,i02  which are the current points to calculate the difference
         */
        public double Difference(int i01, int i02)
        {
            return 100 / NullFaceBetween(i01, i02) * Between(i01, i02); // calculates the percent (rule of three)
        }

        /**
         * calculates the the differenc of the points from the ABSOLUTENullFace
         * @param i01,i02  which are the current points to calculate the difference 
         */ 
        public double NullFaceBetween(int i01, int i02)
        {
            if (nullFace[i01].world.x != 0) // -------->wofür die abfrage ?  wäre es nicht sinnvoller auf null zu prüfen ?  Tanja said the nullface is never null, so that was the reason why she used 0 instead of null
            {
                double a = Math.Abs(nullFace[i02].world.y - nullFace[i01].world.y);
                double b = Math.Abs(nullFace[i02].world.x - nullFace[i01].world.x);
                double c = Math.Abs(nullFace[i02].world.z - nullFace[i01].world.z);
                return Math.Sqrt(a * a + b * b + c * c);  //vector analysis of the length (Schütt ahu!) 
            }
            throw new NullReferenceException();
        }
        /**
         * calculates the difference between the two points of the current frame
         * @param i01,i02  which are the current points to calculate the difference 
         */
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

            /*
              Exception error = new Exception();
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

        /**
         *  getter of the modules
         */ 
        public List<RSModule> Modules
        {
            get { return modules; }
        }
        
        public PXCMSenseManager SenseManager
        {
            get { return senseManager; }
            set { senseManager = value; }
        }
        /**
         *  getter and setter of the width
         */
        public int Width
        {
            get { return width; }
        }

        /**
         *  getter and setter of the height
         */
        public int Height
        {
            get { return height; }
        }


        /**
         *  getter and setter of the face
         */
        public PXCMFaceModule Face
        {
            get { return face; }
        }

        /**
         *  getter and setter of the landmarkpoints
         */
        public PXCMFaceData.LandmarksData Lp
        {
            get { return lp; }
            set { lp = value; }
        }


        /**
         *  getter and setter of the ABSOLUTE NullFace
         */
        public PXCMFaceData.LandmarkPoint[] NullFace
        {
            get { return nullFace; }
            set { nullFace = value; }
        }


        /**
         *  getter and setter of the faceData
         */
        public PXCMFaceData FaceData
        {
            get { return faceData; }
            set { faceData = value; }
        }

        /**
         *  getter and setter of the FaceCurrent
         *  
         *  FaceAktuell should be changed to FaceCurrent, where is it initialised 
         */
        public PXCMFaceData.Face FaceAktuell
        {
            get { return faceAktuell; }
            set { faceAktuell = value; }
        }

        /**
         *  getter and setter of the expressionData Edata
         */
        public PXCMFaceData.ExpressionsData Edata
        {
            get { return edata; }
            set { edata = value; }
        }


        /**
         *  getter and setter of the View
         */
        public CameraView View
        {
            get { return view; }
            set { view = value; }
        }

        /**
         *  getter and setter of the array from the emotions 
         */
        public int[] Emotions
        {
            get { return emotions; }
            set { emotions = value; }
        }
    }
}
 