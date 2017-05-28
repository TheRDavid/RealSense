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
    class FaceRecorder : RSModule
    {

        private FaceRecording currentRecording = null;
        private static int numLandmarkPoints = 78, framesStored = 300;
        private PXCMFaceData.LandmarkPoint[][] data = new PXCMFaceData.LandmarkPoint[framesStored][]; // 300 Frames (10 seconds), each 78 LPs
        private bool recording = false;
        private int frameIndex = 0;
        private string[] typeNames = new string[] { "anger", "joy", "fear", "contempt", "sadness", "disgust", "surprise" };

        public FaceRecorder()
        {
            // anger, joy, fear, contempt, sadness, disgust, surprise, stop
            triggers = new int[] { (int)Keys.D1, (int)Keys.D2, (int)Keys.D3, (int)Keys.D4, (int)Keys.D5, (int)Keys.D6, (int)Keys.D7, (int)Keys.D8 }; // record 7 emotions + stop recording
            debug = true;
            for (int i = 0; i < framesStored; i++) data[i] = new PXCMFaceData.LandmarkPoint[numLandmarkPoints];
        }

        public override void keyTrigger(int key)
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
                keyTrigger(triggers[7]);
            }
        }

        public bool Recording
        {
            get { return recording; }
        }

        public int RecordingIndex
        {
            get { return frameIndex; }
            set { frameIndex = value; }
        }


    }
}
