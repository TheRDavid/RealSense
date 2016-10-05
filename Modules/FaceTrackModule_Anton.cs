using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RealSense
{
    /**
     * Instance of an abstract RealSenseModule (see RSModule.cs)
     * Draws a Rectangle around every Face it finds
     */
    class FaceTrackModule_Anton : RSModule
    {

        // Stuff for Face detection
        private PXCMFaceModule module;
        private PXCMFaceData data;
        private PXCMFaceConfiguration fg;



        // Initialise all the things
        public override void Init(PXCMSenseManager sManager)
        {
            senseManager = sManager;
            module = senseManager.QueryFace();
            fg = module.CreateActiveConfiguration();
            fg.SetTrackingMode(PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR);
            fg.detection.isEnabled = true;
            // Set to enable all alerts
            fg.EnableAllAlerts();
            // Apply changes
            fg.ApplyChanges();
            fg.Update();



        }

        public override void Work(Graphics g)
        {
            data = module.CreateOutput();
            data.Update();
            // Get the number of tracked faces
            Int32 nfaces = data.QueryNumberOfDetectedFaces();
            Console.WriteLine("Number of faces : " + nfaces);

            data.Dispose();

        }
    }
}
