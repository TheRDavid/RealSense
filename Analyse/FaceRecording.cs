using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RealSense
{
    [Serializable]
    public class FaceRecording
    {
        private String name;
        private PXCMFaceData.LandmarkPoint[][] data;
        private PXCMFaceData.LandmarkPoint[] nullData;

        public FaceRecording(String emotionName)
        {
            name = DateTime.Now.ToString("dd_MM_HH_mm_ss") + "." + emotionName;
        }

        public void setData(PXCMFaceData.LandmarkPoint[][] d, PXCMFaceData.LandmarkPoint[] nd)
        {
            data = d;
            nullData = nd;
        }

        public void save()
        {
            string serializationFile = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName+"\\Recordings", name);
            Stream stream = File.Open(serializationFile, FileMode.Create);
            new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Serialize(stream, this);
        }   

        public static FaceRecording load(String n)
        {
            Stream stream = File.Open(n, FileMode.Open);
            return (FaceRecording)new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Deserialize(stream);
        }

        public PXCMFaceData.LandmarkPoint[] getFace(int frame)
        {
            return data[frame];
        }

        public PXCMFaceData.LandmarkPoint[] getNullFace()
        {
            return nullData;
        }

    }
}
