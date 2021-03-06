﻿using System;
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
        public enum Emotion { ANGER, CONTEMPT, DISGUST, FEAR, JOY, SADNESS, SURPRISE };
        public enum MODE { ANALYZE, RUN, TEST };
        public static int NOSE_FIX = 26;
        public static bool calibrated = false;

        // Reference to globally used SenseManager
        private PXCMSenseManager senseManager;
        private PXCMFaceModule face;
        private PXCMFaceData faceData;
        private PXCMFaceConfiguration faceConfig;
        public PXCMFaceData.Face faceAktuell;
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
        private bool test = false;

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
         * Like enabling all important tracker(Hand, Face), the stream and builds up the configuration.
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
               bool couldInit = senseManager.Init().IsSuccessful();
                if (!couldInit)
                {
                    MessageBox.Show("Could not connect to the hardware! Make sure you have the camera-drivers installed.", "Aww, dang");
                    Environment.Exit(1);
                }
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
         * Adds a new modul to the List
         * @param RSModul m which is the new module
         */
        public void AddModule(RSModule m)
        {
            modules.Add(m);
        }

        /**
         * Returns the total difference of axis-specific distance between two points
         * @param i01,i02 which are the current points to calculate the difference
         * @param axis which is the specific axis to work with
         * @param absolute defines wether or not the absolute difference should be returned or not
         */
        public double DifferenceByAxis(int i01, int i02, AXIS axis, bool absolute)
        {
            return NullFaceBetweenByAxis(i01, i02, axis, absolute) - BetweenByAxis(i01, i02, axis, absolute);
        }

        /**
         * Calculates the axis-specific difference of the points from the ABSOLUTENullFace
         * @param i01,i02  which are the current points to calculate the difference
         * @param axis which is the specific axis to work with
         * @param absolute defines wether or not the absolute difference should be returned or not
         */
        public double NullFaceBetweenByAxis(int i01, int i02, AXIS axis, bool absolute)
        {
            double result = 0;
            switch (axis)
            {
                case AXIS.X: result = nullFace[i02].world.x - nullFace[i01].world.x; break;
                case AXIS.Y: result = nullFace[i02].world.y - nullFace[i01].world.y; break;
                case AXIS.Z: result = nullFace[i02].world.z - nullFace[i01].world.z; break;
            }
            return absolute ? Math.Abs(result) : result;
        }

        /**
         * Calculates the axis-specific difference of the points from the ABSOLUTENullFace
         * @param i01,i02  which are the current points to calculate the difference
         * @param axis which is the specific axis to work with
         * @param absolute defines wether or not the absolute difference should be returned or not
         */
        public double BetweenByAxis(int i01, int i02, AXIS axis, bool absolute)
        {
            double result = 0;
            switch (axis)
            {
                case AXIS.X: result = currentFace[i02].world.x - currentFace[i01].world.x; break;
                case AXIS.Y: result = currentFace[i02].world.y - currentFace[i01].world.y; break;
                case AXIS.Z: result = currentFace[i02].world.z - currentFace[i01].world.z; break;
            }
            return absolute ? Math.Abs(result) : result;
        }

        /**
         * calculates the percentage of the difference of distance between two points
         * @param i01,i02  which are the current points to calculate the difference
         * @returns double between 0 and 100
         */
        public double Difference(int i01, int i02)
        {
            return 100 / NullFaceBetween(i01, i02) * Between(i01, i02); // calculates the percent (rule of three)
        }

        /**
         * Returns the change in distance between the nose-fixpoint along a specified axis
         * @param int i01 - landmark-number
         * @param AXIS axis - axis to consider
         */
        public double DifferenceNullCurrent(int i01, AXIS axis)
        {
            double result = 0;
            switch (axis)
            {
                case AXIS.X: result = (nullFace[NOSE_FIX].world.x - nullFace[i01].world.x) - (currentFace[NOSE_FIX].world.x - currentFace[i01].world.x); break;
                case AXIS.Y: result = (nullFace[NOSE_FIX].world.y - nullFace[i01].world.y) - (currentFace[NOSE_FIX].world.y - currentFace[i01].world.y); break;
                case AXIS.Z: result = (nullFace[NOSE_FIX].world.z - nullFace[i01].world.z) - (currentFace[NOSE_FIX].world.z - currentFace[i01].world.z); break;
            }
            return result;
        }

        /**
         * calculates the differenc of the points from the ABSOLUTENullFace
         * @param i01,i02  which are the current points to calculate the difference 
         */
        public double NullFaceBetween(int i01, int i02)
        {
            if (nullFace[i01].world.x != 0) // -------->wofür die abfrage ?  wäre es nicht sinnvoller auf null zu prüfen ?  Tanja said the nullface is never null, so that was the reason why she used 0 instead of null
            {
                double a = Math.Abs(nullFace[i02].world.y - nullFace[i01].world.y);
                double b = Math.Abs(nullFace[i02].world.x - nullFace[i01].world.x);
                double c = Math.Abs(nullFace[i02].world.z - nullFace[i01].world.z);
                return Math.Sqrt(a * a + b * b + c * c);  //vector analysis of the length (Schuett ahu!) 
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

            if (lp != null || !stream)
            {
                point01 = currentFace[i01];
                point02 = currentFace[i02];

                //lp.QueryPoint(i01, out point01); //AccessViolationException ...
                //lp.QueryPoint(i02, out point02);

                double a = Math.Abs(point02.world.y - point01.world.y);
                double b = Math.Abs(point02.world.x - point01.world.x);
                double c = Math.Abs(point02.world.z - point01.world.z);
                return Math.Sqrt(a * a + b * b + c * c);
            }
            throw new NullReferenceException();

        }

        /**
         *  getter of the modules
         */
        public List<RSModule> Modules
        {
            get { return modules; }
        }

        /**
         *  getter and setter of the SenseManager
         */
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
                    lp.QueryPoints(out currentFace); // Update the current face for Save landmarkpoint usage
                }
                else
                {
                    // Console.WriteLine("No Face while calculating");
                }
            }
        }

        /**
         *  getter and setter of the CurrentFace
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
        public PXCMFaceData.Face FaceAktuell
        {
            get { return faceAktuell; }
            set { faceAktuell = value; }
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
         *  getter and setter of the CurrentPoseDiff
         */
        public double CurrentPoseDiff
        {
            get { return currentPoseDiff; }
            set { currentPoseDiff = value; }
        }

        /**
         *  getter and setter of the PoseMax
         */
        public int PoseMax
        {
            get { return maxPose; }
            set { maxPose = value; }
        }

        /**
         *  getter and setter of the CurrentRollDiff
         */
        public double CurrentRollDiff
        {
            get { return rollDiff; }
            set { rollDiff = value; }
        }

        /**
         *  getter and setter of the CurrentPitchDiff
         */
        public double CurrentPitchDiff
        {
            get { return pitchDiff; }
            set { pitchDiff = value; }
        }

        /**
         *  getter and setter of the CurrentYawDiff
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
         *  getter and setter of the DefaultFont
         */
        public Font DefaultFont
        {
            get { return defaultFont; }
            set { defaultFont = value; }
        }

        /**
         *  getter and setter of the DefaultStringBrush
         */
        public SolidBrush DefaultStringBrush
        {
            get { return defaultStringBrush; }
            set { defaultStringBrush = value; }
        }

        /**
         *  getter and setter of the NullPose
         */
        public PXCMFaceData.PoseEulerAngles NullPose
        {
            get { return nullPose; }
            set { nullPose = value; }
        }

        /**
         *  getter and setter of the CurrentPose
         */
        public PXCMFaceData.PoseEulerAngles CurrentPose
        {
            get { return currentPose; }
            set { currentPose = value; }
        }

        /**
         *  getter and setter of the DefaultBGBrush
         */
        public SolidBrush DefaultBGBrush
        {
            get { return bgStringBrush; }
        }

        /**
         *  getter and setter of the OpaqueBGBrush
         */
        public SolidBrush OpaqueBGBrush
        {
            get { return opaqueStringBrush; }
        }

        /**
         *  getter and setter of the AU_Values
         */
        public Dictionary<String, double> AU_Values
        {
            get { return au_Values; }
            set { au_Values = value; }
        }

        /**
         *  getter and setter of whether to test or not
         */
        public bool Test
        {
            get { return test; }
            set { test = value; }
        }

        /**
         *  getter and setter of the ColorBitmap
         */
        public Bitmap ColorBitmap
        {
            get { return view.ColorBitmap; }
        }

        /**
         *  getter and setter of the EmotionMax
         */
        public Dictionary<Emotion, double> EmotionMax
        {
            get { return emotionMax; }
            set { emotionMax = value; }
        }
    }
}