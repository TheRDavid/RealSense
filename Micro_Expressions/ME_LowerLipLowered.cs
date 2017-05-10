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
     * 
     * Interpretation:         0 = Relaxed
     *                       100 = Lip down
     */
    class ME_LowerLipLowered : RSModule
    {

        // variables for logic

        private double[] lowerLip_Distance = new double[5];
        private double distance;
        private double[] distances = new double[numFramesBeforeAccept];
        private string debug_message = "LowerLipLowered: ";

        /**
         * Sets default-values
         */
        public ME_LowerLipLowered()
        {
            DEF_MIN = -1;
            DEF_MAX = 4;
            reset();
            MIN_TOL = -1;
            MAX_TOL = 1;
            debug = true;
            XTREME_MAX = 8;
            XTREME_MIN = -6;
            model.AU_Values[typeof(ME_LowerLipLowered).ToString()] = 0;
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


            if (framesGathered < numFramesBeforeAccept)
            {
                distances[framesGathered++] = distance;
            }
            else
            {
                filterToleranceValues(distances);

                double distance = filteredAvg(distances);

                dynamicMinMax(new double[] { distance });

                double[] diffs = convertValues(new double[] { distance });

                /* Update value in Model */
                model.AU_Values[typeof(ME_NoseWrinkled).ToString()] = diffs[0];

                /* print debug-values */
                if (debug)
                {
                    output = debug_message + "(" + diffs[0] + ")";
                }
                framesGathered = 0;
            }
        }
    }
}
