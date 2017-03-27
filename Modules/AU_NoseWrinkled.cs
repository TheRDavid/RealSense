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

        // variables for debugging

        private string debug_message = "NoseWrinkled: ";

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



            int d_l = Convert.ToInt32(left_diff);
            int d_r = Convert.ToInt32(right_diff);

            /* Update value in Model */
            model.setAU_Value(typeof(AU_NoseWrinkled).ToString() + "_left", d_l);
            model.setAU_Value(typeof(AU_NoseWrinkled).ToString() + "_right", d_r);

            /* print debug-values */
            if (debug)
            {
                output = debug_message + "(" + d_l + ", " + d_r + ")";
            }
        }
    }
}
