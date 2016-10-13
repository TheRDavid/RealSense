using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense
{
    class HandTrackerModule : RSModule
    {


        private Pen pen = new Pen(Color.DarkKhaki, 2);

        public override void Work(Graphics g)
        {
            if (model.HandData != null)
            {
                PXCMHandData.IHand hands = null;
                model.HandData.QueryHandData(PXCMHandData.AccessOrderType.ACCESS_ORDER_BY_ID, 0, out hands);
                if (hands == null) return;
                PXCMRectI32 rect = hands.QueryBoundingBoxImage();
                Rectangle rectangle = new Rectangle(rect.x, rect.y, rect.w, rect.h); // Convert to Rectangle
                g.DrawRectangle(pen, rectangle); // Draw
            }
        }
    }
}
