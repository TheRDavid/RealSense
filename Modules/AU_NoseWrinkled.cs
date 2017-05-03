using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;



namespace RealSense
{
    /**
     * Measures wrinkling of nose
     * @author: David Rosenbusch
     * @HogwartsHouse Hufflepuff
     */
    class AU_NoseWrinkled : RSModule
    {

        // variables for logic

        private double left_diff, right_diff, middle_diff;
        private double distance;

        // Default values
        public AU_NoseWrinkled()
        {
            debug = true;
        }

        /** 
         * @Override 
         * Hard to describe, have a look at Images/AU_NoseWrinkledModule.png
         * Result of calculation constantly changing between positive and negative -> relaxed
         * Result of calculation constantly positive -> wrinkled (tiny values)
         * Result of calculation constantly negative -> ... go see a doctor m8
         * See Images/AU_NoseWrinkledModule.png
         */
        public override void Work(Graphics g)
        {
            /* calculations */

            left_diff = model.Difference(30, Model.NOSE_FIX) - 100;
            right_diff = model.Difference(32, Model.NOSE_FIX) - 100;
            // what the fuck are you doing ? 
            middle_diff = model.Difference(31, Model.NOSE_FIX) - 100;

            distance = (left_diff + right_diff + middle_diff) / 3;

            distance = distance < MAX_TOL && distance > MIN_TOL ? 0 : distance;

            distance = filterExtremeValues(distance);

            dynamicMinMax(new double[] { distance });

            double[] diffs = convertValues(new double[] { distance });

            /* Update value in Model */
            model.setAU_Value(typeof(AU_NoseWrinkled).ToString() , diffs[0]);
      

            /* print debug-values */
            if (debug)
            {
                output = "NoseWrinkled: " + "(" + diffs[0] + ")";
            }
        }
    }
}
