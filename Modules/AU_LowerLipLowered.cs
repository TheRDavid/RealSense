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

        private double[] lowerLip_Distance=new double[4];
        private double distance;

        // variables for debugging

        private string debug_message = "LowerLipLowered: ";

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
            lowerLip_Distance[0] = model.DifferenceNullCurrent(44, Model.AXIS.Y);
            lowerLip_Distance[0] = model.DifferenceNullCurrent(43, Model.AXIS.Y);
            lowerLip_Distance[0] = model.DifferenceNullCurrent(42, Model.AXIS.Y);
            lowerLip_Distance[0] = model.DifferenceNullCurrent(41, Model.AXIS.Y);
            lowerLip_Distance[0] = model.DifferenceNullCurrent(40, Model.AXIS.Y);
            distance = (lowerLip_Distance[0] + lowerLip_Distance[1] + lowerLip_Distance[2] + lowerLip_Distance[3] + lowerLip_Distance[4]) / 5;

            /* Update value in Model */
            model.setAU_Value(typeof(AU_LowerLipLowered).ToString(), distance);

            /* print debug-values */
            if (debug)
            {
                model.View.Debug_Y += 20; // new row
                g.DrawString(debug_message + distance, model.DefaultFont, model.DefaultStringBrush, new Point(0, model.View.Debug_Y));
            }
        }
    }
}
