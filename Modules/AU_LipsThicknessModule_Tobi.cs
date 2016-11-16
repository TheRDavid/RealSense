using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    class AU_LipsThicknessModul_Tobi : RSModule
    {
        


        private Font font = new Font("Arial", 18);
        private SolidBrush stringBrush = new SolidBrush(Color.ForestGreen);
        private float[] lipUp = new float[6];
        private float[] lipLow = new float[6];

     
        public override void Work(Graphics g)
        {
            if (model.FaceAktuell == null) return;
            PXCMFaceData.LandmarksData lp = model.FaceAktuell.QueryLandmarks();
            PXCMFaceData.LandmarkPoint point;

            if (lp != null)
            {
                //upper Lip 
                lp.QueryPoint(35, out point);
                lipUp[0] = point.world.y;
                lp.QueryPoint(36, out point);
                lipUp[1] = point.world.y;
                lp.QueryPoint(37, out point);
                lipUp[2] = point.world.y;

                lp.QueryPoint(46, out point);
                lipUp[3] = point.world.y;
                lp.QueryPoint(47, out point);
                lipUp[4] = point.world.y;
                lp.QueryPoint(48, out point);
                lipUp[5] = point.world.y;

                //bottom Lip 
                lp.QueryPoint(52, out point);
                lipLow[0] = point.world.y;
                lp.QueryPoint(51, out point);
                lipLow[1] = point.world.y;
                lp.QueryPoint(50, out point);
                lipLow[2] = point.world.y;

                lp.QueryPoint(43, out point);
                lipLow[3] = point.world.y;
                lp.QueryPoint(42, out point);
                lipLow[4] = point.world.y;
                lp.QueryPoint(41, out point);
                lipLow[5] = point.world.y;

                

                float distanceUp = lipUp[0] - lipUp[3]
                    + lipUp[1] - lipUp[4]
                    + lipUp[2] - lipUp[5];


                float distanceDown = lipLow[0] - lipLow[3]
                    + lipLow[1] - lipLow[4]
                    + lipLow[2] - lipLow[5];

                Console.WriteLine(distanceUp);
                Console.WriteLine(distanceDown);

                if (distanceUp < .0225f && distanceDown < .0225f)
                {
                    g.DrawString("Lips are thin", font, stringBrush, new PointF(20, 20));
                }
            }
        }
    }
}
