using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace RealSense
{
    /*
    * Upon being triggered by a key, this module records 300 frames of landmark-data and classifies it as a set emotion.
    * @author David 
    * @HogwartsHouse Hufflepuff  
    */
    class FaceRecorder : RSModule
    {

        private FaceRecording currentRecording = null;
        private static int numLandmarkPoints = 78, framesStored = 300;
        private PXCMFaceData.LandmarkPoint[][] data = new PXCMFaceData.LandmarkPoint[framesStored][]; // 300 Frames (10 seconds), each 78 LPs
        private bool recording = false;
        private int frameIndex = 0;
        private string[] typeNames = new string[] { "anger", "joy", "fear", "contempt", "sadness", "disgust", "surprise" };

        /**
        * Initializes the keyTriggers and an empty buffer for the landmark-data.
        */
        public FaceRecorder()
        {
            // anger, joy, fear, contempt, sadness, disgust, surprise, stop
            triggers = new int[] { (int)Keys.D1, (int)Keys.D2, (int)Keys.D3, (int)Keys.D4, (int)Keys.D5, (int)Keys.D6, (int)Keys.D7, (int)Keys.D8 }; // record 7 emotions + stop recording
            debug = true;
            for (int i = 0; i < framesStored; i++) data[i] = new PXCMFaceData.LandmarkPoint[numLandmarkPoints];
        }

        /**
          * The current recording's type is determined by the key that is pressed to trigger the recording.
          * @param key - key that triggers the module
          */
        public override void KeyTrigger(int key)
        {
            for (int i = 0; i < typeNames.Length; i++)
            {
                if (triggers[i] == key)
                {
                    frameIndex = 0;
                    currentRecording = new FaceRecording(typeNames[i]);
                    recording = true;
                    return;
                }
            }
            if (recording && key == triggers[7])
            {
                recording = false;
                currentRecording.setData(data, model.NullFace);
                currentRecording.save();
            }
        }
        /**
     * @Override
     * Shows whether or not a recording is taking place and records the current landmark-data
     * @param Graphics g for the view
     */
        public override void Work(Graphics g)
        {
            if (recording)
            {
                g.FillEllipse(new SolidBrush(Color.Red), 35, 35, 35, 35);
            }

            if (recording && frameIndex < framesStored)
            {
                for (int i = 0; i < numLandmarkPoints; i++)
                {
                    data[frameIndex][i] = model.CurrentFace[i];
                }
                frameIndex++;
            }
            else if (recording && frameIndex == framesStored)
            {
                KeyTrigger(triggers[7]);
            }
        }

        /**
         * Returns the current recording
         */
        public bool Recording
        {
            get { return recording; }
        }


        /**
         * Sets or gets the current recording-index
         */
        public int RecordingIndex
        {
            get { return frameIndex; }
            set { frameIndex = value; }
        }


    }
}
