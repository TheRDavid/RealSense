using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RealSense
{
    [Serializable]
    /*
     * All the data requiered to analyze a landmark-recording
     * - Landmark-Coordinates frame by frame
     * - NullFace Landmark-Coordinates as a means to calibrate
     * @author David 
     * @HogwartsHouse Hufflepuff  
     * 
     */
    public class FaceRecording
    {
        private String name;
        private PXCMFaceData.LandmarkPoint[][] data;
        private PXCMFaceData.LandmarkPoint[] nullData;

        /**
         * Initializes the Recording, giving it a name (timestamp) plus the emotion-type
         * @param String emotionName
         */ 
        public FaceRecording(String emotionName)
        {
            name = DateTime.Now.ToString("dd_MM_HH_mm_ss") + "." + emotionName;
        }

        /**
         * Sets the landmark-data
         * @param PXCMFaceData.LandmarkPoint[][] frameByFrameData array of frame-by-frame data
         * @param PXCMFaceData.LandmarkPoint[] nd data of the calibrated NullFace
         */
        public void setData(PXCMFaceData.LandmarkPoint[][] frameByFrameData, PXCMFaceData.LandmarkPoint[] nd)
        {
            data = frameByFrameData;
            nullData = nd;
        }

        /**
         * Saves this recording to a file (timestamp + emotion-type).
         */
        public void save()
        {
            string serializationFile = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Recordings", name);
            Stream stream = File.Open(serializationFile, FileMode.Create);
            new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Serialize(stream, this);
        }

        /**
         *
         * Loads a recording from a file
         * @param String n filename 
         * @return An instance of FaceRecording
         */
        public static FaceRecording load(String n)
        {
            Stream stream = File.Open(n, FileMode.Open);
            return (FaceRecording)new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Deserialize(stream);
        }

        /**
         * Returns the landmark-data of a certain frame.
         * @param int frame
         * @return Landmark-Array
         */ 
        public PXCMFaceData.LandmarkPoint[] getFace(int frame)
        {
            return data[frame];
        }

        /**
         * @return the NullFace of this recording
         */ 
        public PXCMFaceData.LandmarkPoint[] getNullFace()
        {
            return nullData;
        }

    }
}
