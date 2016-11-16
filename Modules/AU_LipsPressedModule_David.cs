using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense
{
    /* Shows how much you are smiling */
    class AU_LipsPressedModule_David : RSModule
    {
        //hey david, i actually love comments, but there are not here ! sucker :D 
        private Font font = new Font("Arial", 18);
        private SolidBrush stringBrush = new SolidBrush(Color.Red);
        private float[] lipUpY = new float[3], lipLowY = new float[3];
        public override void Work(Graphics g)
        {
            if (model.FaceAktuell == null) return;
            PXCMFaceData.LandmarksData lp = model.FaceAktuell.QueryLandmarks();
            PXCMFaceData.LandmarkPoint point;

            lp.QueryPoint(47, out point);
            lipUpY[0] = point.world.y;
            lp.QueryPoint(48, out point);
            lipUpY[1] = point.world.y;
            lp.QueryPoint(49, out point);
            lipUpY[2] = point.world.y;

            lp.QueryPoint(52, out point);
            lipLowY[0] = point.world.y;
            lp.QueryPoint(51, out point);
            lipLowY[1] = point.world.y;
            lp.QueryPoint(50, out point);
            lipLowY[2] = point.world.y;

            Console.WriteLine(lipUpY[0] + "  " + lipUpY[1] + "  " + lipUpY[2]);
            Console.WriteLine(lipLowY[0] + "  " + lipLowY[1] + "  " + lipLowY[2]);
            float distance = lipUpY[0] - lipLowY[0]
                + lipUpY[1] - lipLowY[1]
                + lipUpY[2] - lipLowY[2];

            Console.WriteLine(distance);

            if (distance < .003f)
            {
                g.DrawString("Lips pressed", font, stringBrush, new PointF(20, 20));
            }
        }
    }
}