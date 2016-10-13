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
    class FaceTrackerModule : RSModule
    {

        // Pen which defines the appereance of the rect
        private Pen pen = new Pen(Color.BlueViolet, 2);

        public override void Work(Graphics g)
        {
            if (model.FaceAktuell != null)
            {
                PXCMRectI32 rect;
                model.FaceAktuell.QueryDetection().QueryBoundingRect(out rect); // get Face bounds
                Rectangle rectangle = new Rectangle(rect.x, rect.y, rect.w, rect.h); // Convert to Rectangle
                g.DrawRectangle(pen, rectangle); // Draw
            }
        }
    }
}
