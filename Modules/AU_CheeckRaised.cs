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
            left_diff = model.DifferenceByAxis(55, 29, Model.AXIS.Y, false);
            right_diff = model.DifferenceByAxis(67, 29, Model.AXIS.Y, false);

            /* Update value in Model */
            model.setAU_Value(typeof(AU_CheeckRaised).ToString() + "_left", left_diff);
            model.setAU_Value(typeof(AU_CheeckRaised).ToString() + "_right", right_diff);

            /* print debug-values */
            if (debug)
            {
                model.View.Debug_Y += 20; // new row
                g.DrawString(debug_message + "(" + left_diff + ", " + right_diff + ")", model.DefaultFont, model.DefaultStringBrush, new Point(0, model.View.Debug_Y));
            }
        }
    }
}
