using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /**
     * Measures whether the lower lip is lower
     * @author Tobias Schramm
     * @HogwartsHouse Hufflepuff
     */
    class AU_LowerLipLowered : RSModule
    {

        // variables for logic

        private double[] lowerLip_Distance=new double[5];
        private double distance;

        /**
         * Sets default-values
         */
        public AU_LowerLipLowered()
        {
            debug = true;
        }

        /**
         *@Override 
         * Calculates the difference between the two lip corners
         */
        public override void Work(Graphics g)
        {
            /* calculations */

            // calculates the difference between the Nullface and the currentface -> to check if the whole LowerLip is lowered
            lowerLip_Distance[0] = model.Difference(44, Model.NOSE_FIX);
            lowerLip_Distance[1] = model.Difference(43, Model.NOSE_FIX);
            lowerLip_Distance[2] = model.Difference(42, Model.NOSE_FIX);
            lowerLip_Distance[3] = model.Difference(41, Model.NOSE_FIX);
            lowerLip_Distance[4] = model.Difference(40, Model.NOSE_FIX);
            distance = ((lowerLip_Distance[0] + lowerLip_Distance[1] + lowerLip_Distance[2] + lowerLip_Distance[3] + lowerLip_Distance[4]) / 5);
            distance -= 100;

            distance = distance < MAX_TOL && distance > MIN_TOL ? 0 : distance;

            distance = filterExtremeValues(distance);

            dynamicMinMax(new double[] { distance });

            double[] diffs = convertValues(new double[] { distance });

            /* Update value in Model */
            model.setAU_Value(typeof(AU_LowerLipLowered).ToString(), diffs[0]);

            /* print debug-values */
            if (debug)
            {
                output = "LowerLipLowered: " + diffs[0];
            }
        }
    }
}
