using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense.Modules
{
    /**
     * This class detects whether the mouth forms an rectangle or not 
     * 
     * @author: René
     */
    class AU_MouthRect_Rene : RSModule
    {
        private float[] lmArray = new float[12];

        private Font font = new Font("Arial", 18);
        private SolidBrush stringBrush = new SolidBrush(Color.Red);



        public override void Work(Graphics g)
        {
            initLandmarks();
            findRect(g);
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
                for (int i = 35; i < 11; i++)
                {
                    int idx = 0;
                    lp.QueryPoint(i, out point);
                    lmArray[idx] = point.world.y;
                    idx++;

                }

        }

        /*
         * Checks whether the mouth is an rectangle
         */
        private void findRect(Graphics g)
        {

            //Das macht geometrisch alles keinen sinn :|

            if (true)//mathematisches Wirrwar
            {
                g.DrawString("Rectangle", font, stringBrush, new PointF(20, 80));
            }

        }








    }
}
