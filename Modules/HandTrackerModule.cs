using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
/**
 * @author: David
 * @author:
 */
namespace RealSense
{
    /**
     * Instance of an abstract RealSenseModule (see RSModule.cs)
     * Draws a Rectangle around the hand (only 1 for now)
     */
    class HandTrackerModule : RSModule
    {

        // Stuff for Hand detection
        private PXCMHandModule module;
        private PXCMHandData data;

        // Pen which defines the appereance of the rect
        private Pen pen = new Pen(Color.DarkKhaki, 2);

        // Initialise all the things
        public override void Init(CameraView cv)
        {
            senseManager = cv.SenseManager;
            senseManager.EnableHand();
            module = (PXCMHandModule)cv.CreatePXCMModule(PXCMHandData.CUID);
            Console.WriteLine("HandeTracker_David: " + module.GetHashCode());
            PXCMHandConfiguration config = module.CreateActiveConfiguration();
            config.SetTrackingMode(PXCMHandData.TrackingModeType.TRACKING_MODE_FULL_HAND);
            config.ApplyChanges();
            config.Update();
        }

        public override void Work(Graphics g)
        {
            data = module.CreateOutput();
            data.Update();
            PXCMHandData.IHand hands = null;
            data.QueryHandData(PXCMHandData.AccessOrderType.ACCESS_ORDER_BY_ID,0, out hands);
            if (hands == null) return;
            PXCMRectI32 rect = hands.QueryBoundingBoxImage();
            Rectangle rectangle = new Rectangle(rect.x, rect.y, rect.w, rect.h); // Convert to Rectangle
            g.DrawRectangle(pen, rectangle); // Draw

        }
    }
}
