using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /**
     * Measures whether the upper lip is raised
     * @author Tobias Schramm
     * @HogwartsHouse Hufflepuff
     */
    class AU_UpperLipRaised : RSModule
    {

        // variables for logic

        private double[] upperLip_Distance = new double[5];
        private double distance;

        // variables for debugging

        private string debug_message = "AU_UpperLipRaised: ";

        /**
         * Sets default-values
         */
        public AU_UpperLipRaised()
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

            // calculates the difference between the Nullface and the currentface -> to check if the whole LowerLip is raised
            upperLip_Distance[0] = model.DifferenceNullCurrent(34, Model.AXIS.Y);
            upperLip_Distance[1] = model.DifferenceNullCurrent(35, Model.AXIS.Y);
            upperLip_Distance[2] = model.DifferenceNullCurrent(36, Model.AXIS.Y);
            upperLip_Distance[3] = model.DifferenceNullCurrent(37, Model.AXIS.Y);
            upperLip_Distance[4] = model.DifferenceNullCurrent(38, Model.AXIS.Y);
            distance = (upperLip_Distance[0] + upperLip_Distance[1] + upperLip_Distance[2] + upperLip_Distance[3] + upperLip_Distance[4]) / 5;
            distance *= 100;
            /* Update value in Model */
            model.setAU_Value(typeof(AU_UpperLipRaised).ToString(), distance);

            /* print debug-values */
            if (debug)
            {
                model.View.Debug_Y += 20; // new row
                g.DrawString(debug_message + distance, model.DefaultFont, model.DefaultStringBrush, new Point(0, model.View.Debug_Y));
            }
        }
    }
}
