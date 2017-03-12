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
            left_diff = model.DifferenceByAxis(30, 29, Model.AXIS.Y, false);
            right_diff = model.DifferenceByAxis(32, 29, Model.AXIS.Y, false);
            middle_diff = model.DifferenceByAxis(32, 31, Model.AXIS.Y, false);

            /* Update value in Model */
            model.setAU_Value(typeof(AU_NoseWrinkled).ToString() + "_left", left_diff);
            model.setAU_Value(typeof(AU_NoseWrinkled).ToString() + "_right", right_diff);

            /* print debug-values */
            if (debug)
            {
                model.View.Debug_Y += 20; // new row
                g.DrawString(debug_message + "(" + left_diff + ", " + right_diff + ")", model.DefaultFont, model.DefaultStringBrush, new Point(0, model.View.Debug_Y));
            }
        }
    }
}
