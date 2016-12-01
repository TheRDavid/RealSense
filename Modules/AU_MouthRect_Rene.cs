using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense
{
    /**
     * This class detects whether the mouth forms an rectangle or not 
     * 
     * @author: René
     */
    class AU_MouthRect_Rene : RSModule
    {
        private float[,] lmArray = new float[4, 2];

        private Font font = new Font("Arial", 18);
        private SolidBrush stringBrush = new SolidBrush(Color.Red);

        //differences
        float urDr, ulDl, ulUr, dlDr;


        public override void Work(Graphics g)
        {
            initLandmarks();
            findRect();

            if (findRect())//mathematisches Wirrwar
            {
                g.DrawString("Rectangle", font, stringBrush, new PointF(20, 80));
            }
        }

        /**
  * Initiates mouthLandmarks by 
  */
        public void initLandmarks()
        {
            if (model.FaceAktuell == null) return;
            PXCMFaceData.LandmarksData lp = model.FaceAktuell.QueryLandmarks();
            PXCMFaceData.LandmarkPoint point;

            if (lp != null)
            {
                //ul
                lp.QueryPoint(33, out point);
                lmArray[0, 0] = point.world.x;
                lp.QueryPoint(33, out point);
                lmArray[0, 1] = point.world.y;

                //ur
                lp.QueryPoint(39, out point);
                lmArray[1, 0] = point.world.y;
                lp.QueryPoint(39, out point);
                lmArray[1, 1] = point.world.x;

                //dl
                lp.QueryPoint(44, out point);
                lmArray[2, 0] = point.world.y;
                lp.QueryPoint(44, out point);
                lmArray[2, 1] = point.world.x;

                //dr
                lp.QueryPoint(40, out point);
                lmArray[3, 0] = point.world.y;
                lp.QueryPoint(40, out point);
                lmArray[3, 1] = point.world.x;


            }

            //x-differences
            urDr = lmArray[1, 0] - lmArray[3, 0];
            ulDl = lmArray[0, 0] - lmArray[2, 0];

            //y-differences
            ulUr = lmArray[0, 1] - lmArray[1, 1];
            dlDr = lmArray[2, 1] - lmArray[3, 1];


        }

        /*
         * Checks whether the mouth is an rectangle
         */
        private bool findRect()
        {

            //rect request without values yet
            if (urDr <= 0 || urDr >= -0)
                if (ulDl <= 0 || ulDl >= -0)
                    if (ulUr <= 0 || ulUr >= -0)
                        if (dlDr <= 0 || dlDr >= -0)
                            return true;


            return false;

        }








    }
}
