using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

/**
 * This class is the Autounfall eyelid class.
 */

namespace RealSense
{
    class AU_EyelidTightModul_Anton : RSModule
    {
        //hey david, i actually write comments. because i'm a genius! 


        private Font font = new Font("Arial", 18);
        private SolidBrush stringBrush = new SolidBrush(Color.ForestGreen);
        private float[] lidUpRightY = new float[3];
        private float[] lidLowRightY = new float[3];

        private float[] lidUpLeftY = new float[3];
        private float[] lidLowLeftY = new float[3];

        private float[] distance = new float[2];
        
        private float result;
       

        public override void Work(Graphics g)
        {
            if (model.FaceAktuell == null) return;
            PXCMFaceData.LandmarksData lp = model.FaceAktuell.QueryLandmarks();
            PXCMFaceData.LandmarkPoint point;


            if(lp != null)
            { 
            lp.QueryPoint(19, out point);
            lidUpRightY[0] = point.world.y;
            lp.QueryPoint(20, out point);
            lidUpRightY[1] = point.world.y;
            lp.QueryPoint(21, out point);
            lidUpRightY[2] = point.world.y;

            lp.QueryPoint(25, out point);
            lidLowRightY[0] = point.world.y;
            lp.QueryPoint(24, out point);
            lidLowRightY[1] = point.world.y;
            lp.QueryPoint(23, out point);
            lidLowRightY[2] = point.world.y;


            lp.QueryPoint(13, out point);
            lidUpLeftY[0] = point.world.y;
            lp.QueryPoint(12, out point);
            lidUpLeftY[1] = point.world.y;
            lp.QueryPoint(11, out point);
            lidUpLeftY[2] = point.world.y;

            lp.QueryPoint(15, out point);
            lidLowLeftY[0] = point.world.y;
            lp.QueryPoint(16, out point);
            lidLowLeftY[1] = point.world.y;
            lp.QueryPoint(17, out point);
            lidLowLeftY[2] = point.world.y;

            //   Console.WriteLine(lidUpY[0] + "  " + lidUpY[1] + "  " + lidUpY[2]);
            //  Console.WriteLine(lidLowY[0] + "  " + lidLowY[1] + "  " + lidLowY[2]);


            float distanceRight = lidUpRightY[0] - lidLowRightY[0]
                + lidUpRightY[1] - lidLowRightY[1]
                + lidUpRightY[2] - lidLowRightY[2];


            float distanceLeft = lidUpLeftY[0] - lidLowLeftY[0]
                + lidUpLeftY[1] - lidLowLeftY[1]
                + lidUpLeftY[2] - lidLowLeftY[2];

            Console.WriteLine(distanceRight);
            Console.WriteLine(distanceLeft);

            if (distanceRight < .01f && distanceLeft < .01f)
            {
                //g.DrawString("Lids are tight togehter", font, stringBrush, new PointF(20, 20));
            }


                lp.QueryPoint(0, out point);
                distance[0] = point.world.x;
                lp.QueryPoint(5, out point);
                distance[1] = point.world.x;


                result = distance[1] - distance[0];
                result *= 1000;
                int test = (Int32)result;
               


                g.DrawString("distance "+ test, font, stringBrush, new PointF(20, 20));



            }
        }
    }
}
