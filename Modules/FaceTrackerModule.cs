using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
/**
 * @author: David
 * @author:
 */
namespace RealSense
{
    /**
     * Instance of an abstract RealSenseModule (see RSModule.cs)
     * Draws a Rectangle around every Face it finds
     */ 
    class FaceTrackerModule : RSModule
    {
        
        // Stuff for Face detection
        private PXCMFaceModule module;
        private PXCMFaceData data;

        // Pen which defines the appereance of the rect
        private Pen pen = new Pen(Color.BlueViolet, 2);

        // Initialise all the things
        public override void Init(CameraView cv)
        {
            senseManager = cv.SenseManager;
            module = (PXCMFaceModule)cv.CreatePXCMModule(PXCMFaceData.CUID);
            Console.WriteLine("FaceTracker_David: " + module.GetHashCode());
            PXCMFaceConfiguration config = module.CreateActiveConfiguration();
            config.SetTrackingMode(PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR);
            config.detection.isEnabled = true;
            config.ApplyChanges();
            config.Update();
        }

        public override void Work(Graphics g)
        {
            if (module != null)
            {
                data = module.CreateOutput();
                data.Update(); // props to Tanja
                PXCMFaceData.Face[] faces = data.QueryFaces(); // get all the faces
              
              //  Console.WriteLine(faces.Length + " ugly face(s)");

                foreach (PXCMFaceData.Face face in faces) // Loop through all the faces
                {
                    PXCMRectI32 rect;
                    face.QueryDetection().QueryBoundingRect(out rect); // get Face bounds
                    Rectangle rectangle = new Rectangle(rect.x, rect.y, rect.w, rect.h); // Convert to Rectangle
                    g.DrawRectangle(pen, rectangle); // Draw
                }

                data.Dispose(); // DONE!
            }
        }
    }
}
