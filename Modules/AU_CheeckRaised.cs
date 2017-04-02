using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;



namespace RealSense
{
    /**
     * Measures raising of cheeck (each cheeck)
     * @author: David Rosenbusch
     * @HogwartsHouse Hufflepuff
     */
    class AU_CheeckRaised : RSModule
    {

        // variables for logic

        private double left_diff, right_diff;
        private int beenPositive = 0;

        // variables for debugging

        private string debug_message = "CheeckRaised: ";

        // Default values
        public AU_CheeckRaised()
        {
            debug = true;
        }

        /** 
         * @Override 
         * Calculates difference of cheeck to nose-distance for each side.
         * It only takes the y-AXIS into consideration.
         * Result of calculation constantly changing between positive and negative -> relaxed
         * Result of calculation constantly positive -> raised (tiny values)
         * Result of calculation constantly negative -> how... did you do that?
         * See Images/AU_CheeckRaisedModule.png
         */
        public override void Work(Graphics g)
        {
            /* calculations */
            //left_diff = (model.Difference(55, Model.NOSE_FIX) + model.Difference(54, Model.NOSE_FIX) + model.Difference(56, Model.NOSE_FIX)) / 3 - 100;
            //right_diff = (model.Difference(66, Model.NOSE_FIX) + model.Difference(67, Model.NOSE_FIX) + model.Difference(68, Model.NOSE_FIX)) / 3 - 100;

            left_diff = model.Difference(55, Model.NOSE_FIX) - 100;
            right_diff = model.Difference(67, Model.NOSE_FIX) - 100;

            int val = Convert.ToInt16(left_diff + right_diff) / 2;

            if (left_diff > 0 && right_diff > 0) beenPositive++;
            else beenPositive = 0;


            int d_l = Convert.ToInt32(left_diff * 100) / 100;
            int d_r = Convert.ToInt32(right_diff * 100) / 100;

            /* Update value in Model */
            model.setAU_Value(typeof(AU_CheeckRaised).ToString() + "_left", d_l);
            model.setAU_Value(typeof(AU_CheeckRaised).ToString() + "_right", d_r);

            /* print debug-values */
            if (debug)
            {
                output = debug_message + "(" + d_l + ", " + d_r + ") -> " + (beenPositive > 8 ? "raised -> " : "lowered -> ") + beenPositive+" , "+val;
            }
        }
    }
}
