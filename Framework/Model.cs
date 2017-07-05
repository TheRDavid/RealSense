using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace RealSense
{
    /**
     * Stores all of our data. It is based at the MVC Pattern
     * 
     * @author Tanja Witke
     */
    public class Model
    {
        public enum AXIS { X, Y, Z };
        public enum Emotion { ANGER, CONTEMPT, DISGUST, FEAR, JOY, SADNESS, SURPRISE }
        public enum MODE { ANALYZE, RUN, TEST }
        public static int NOSE_FIX = 26;
        public static bool calibrated = false;

        private PXCMSenseManager senseManager;
        private PXCMFaceModule face;
        private PXCMFaceData faceData;
        private PXCMFaceConfiguration faceConfig;
        private PXCMFaceData.Face faceCurrent;
        private PXCMFaceData.LandmarksData lp;
        private PXCMFaceData.LandmarkPoint[] nullFace = null; //thx David :*
        private PXCMFaceData.LandmarkPoint[] currentFace;
        private PXCMFaceData.PoseEulerAngles nullPose = new PXCMFaceData.PoseEulerAngles();
        public PXCMFaceData.PoseEulerAngles currentPose = new PXCMFaceData.PoseEulerAngles();
        private Dictionary<String, double> au_Values = new Dictionary<String, double>();
        private Dictionary<Emotion, double> emotions = new Dictionary<Emotion, double>();
        private Dictionary<Emotion, double> emotionMax = new Dictionary<Emotion, double>();
        private List<RSModule> modules;
        private int width;
        private int height;
        private int framerate;
        private double currentPoseDiff = 0, yawDiff = 0, rollDiff = 0, pitchDiff = 0;

        public double calibrationProgress = 0;

        private CameraView view;

        private Font defaultFont = new Font("Arial", 18);
        private SolidBrush defaultStringBrush = new SolidBrush(Color.White);
        private SolidBrush bgStringBrush = new SolidBrush(Color.FromArgb(200, 0, 0, 0));
        private SolidBrush opaqueStringBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));

        private int maxPose = 13;
        private bool stream;


        /**
         * Constructor of the model 
         * It does all the important stuff to use our camera.  Its so FANCY ! 
         * 
         */
        public Model(bool s)
        {
            stream = s;
            emotions[Emotion.ANGER] = 0;
            emotions[Emotion.CONTEMPT] = 0;
            emotions[Emotion.DISGUST] = 0;
            emotions[Emotion.FEAR] = 0;
            emotions[Emotion.JOY] = 0;
            emotions[Emotion.SADNESS] = 0;
            emotions[Emotion.SURPRISE] = 0;

            if (stream)
            {
                width = 1920;
                height = 1080;
                framerate = 30;
                senseManager = PXCMSenseManager.CreateInstance();
                senseManager.EnableStream(PXCMCapture.StreamType.STREAM_TYPE_COLOR, width, height, framerate);
                // Enable Face detection
                senseManager.EnableFace();
                senseManager.Init();

                face = senseManager.QueryFace();
                faceConfig = face.CreateActiveConfiguration();
                faceConfig.SetTrackingMode(PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR_PLUS_DEPTH);
                faceConfig.detection.isEnabled = true;
                faceConfig.pose.isEnabled = true;
                faceConfig.ApplyChanges();
                faceConfig.Update();


                modules = new List<RSModule>();
            }
        }

        /**
         * Returns the value of the given emotion.
         * 
         * @param emotion given emotion
         * @returns the emotion value or -1 if the key doesn't exist
         * 
         * */
        public double EmotionValue(Emotion emotion)
        {
            if (emotions.ContainsKey(emotion))
                return emotions[emotion];
            else return -1;
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
         *  getter of the modules
         */
        public List<RSModule> Modules
        {
            get { return modules; }
        }

        /**
         *  getter and setter of the senseManager
         */
        public PXCMSenseManager SenseManager
        {
            get { return senseManager; }
            set { senseManager = value; }
        }

        /**
         *  getter of the width
         */
        public int Width
        {
            get { return width; }
        }

        /**
         *  getter of the height
         */
        public int Height
        {
            get { return height; }
        }


        /**
         *  getter of the face
         */
        public PXCMFaceModule Face
        {
            get { return face; }
        }

        /**
         *  getter and setter of the landmarkpoints
         *  
         *  WARNING do not touch outside the camera thread ->  so use currentFace
         */
        public PXCMFaceData.LandmarksData Lp
        {
            get { return lp; }
            set
            {
                if (value != null)
                {
                    lp = value;
                    lp.QueryPoints(out currentFace); // update the current face for save landmarkpoint usage
                }
                else
                {
                    // Console.WriteLine("No Face while calculating");
                }
            }
        }

        /**
         *  getter of the stream
         *  
         */
        public bool Stream
        {
            get { return stream; }
        }

        /**
         *  getter and setter of the currentFace
         */
        public PXCMFaceData.LandmarkPoint[] CurrentFace
        {
            get { return currentFace; }
            set { currentFace = value; }
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
        public PXCMFaceData.Face FaceCurrent
        {
            get { return faceCurrent; }
            set { faceCurrent = value; }
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
         *  getter and setter of the currentPoseDiff
         */
        public double CurrentPoseDiff
        {
            get { return currentPoseDiff; }
            set { currentPoseDiff = value; }
        }

        /**
         *  getter and setter of the poseMax
         */
        public int PoseMax
        {
            get { return maxPose; }
            set { maxPose = value; }
        }

        /**
         *  getter and setter of the currentRollDiff
         */
        public double CurrentRollDiff
        {
            get { return rollDiff; }
            set { rollDiff = value; }
        }

        /**
         *  getter and setter of the currentPitchDiff
         */
        public double CurrentPitchDiff
        {
            get { return pitchDiff; }
            set { pitchDiff = value; }
        }

        /**
         *  getter and setter of the currentYawDiff
         */
        public double CurrentYawDiff
        {
            get { return yawDiff; }
            set { yawDiff = value; }
        }

        /**
         *  getter and setter of the array from the emotions 
         */
        public Dictionary<Emotion, double> Emotions
        {
            get { return emotions; }
            set { emotions = value; }
        }

        /**
         *  getter and setter of the defaultFont
         */
        public Font DefaultFont
        {
            get { return defaultFont; }
            set { defaultFont = value; }
        }

        /**
         *  getter and setter of the defaultStringBrush
         */
        public SolidBrush DefaultStringBrush
        {
            get { return defaultStringBrush; }
            set { defaultStringBrush = value; }
        }

        /**
         *  getter and setter of the nullPose
         */
        public PXCMFaceData.PoseEulerAngles NullPose
        {
            get { return nullPose; }
            set { nullPose = value; }
        }

        /**
         *  getter and setter of the currentPose
         */
        public PXCMFaceData.PoseEulerAngles CurrentPose
        {
            get { return currentPose; }
            set { currentPose = value; }
        }

        /**
         *  getter of the defaultBGBrush
         */
        public SolidBrush DefaultBGBrush
        {
            get { return bgStringBrush; }
        }

        /**
         *  getter of the opaqueStringBrush
         */
        public SolidBrush OpaqueBGBrush
        {
            get { return opaqueStringBrush; }
        }

        /**
         *  getter and setter of the au_Values
         */
        public Dictionary<String, double> AU_Values
        {
            get { return au_Values; }
            set { au_Values = value; }
        }

        /**
         *  getter of the ColorBitmap
         */
        public Bitmap ColorBitmap
        {
            get { return view.ColorBitmap; }
        }

        /**
         *  getter and setter of the emotionMax
         */
        public Dictionary<Emotion, double> EmotionMax
        {
            get { return emotionMax; }
            set { emotionMax = value; }
        }
    }
}