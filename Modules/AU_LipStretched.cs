using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /**
     * Measures whether the lips are stretched 
     * @author Tobias Schramm
     * @HogwartsHouse Hufflepuff
     */
    class AU_LipStretched : RSModule
    {

        // variables for logic

        private double lips_corner_distance;

        // variables for debugging

        private string debug_message = "LipStreched: ";

        /**
         * Sets default-values
         */
        public AU_LipStretched()
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

            lips_corner_distance = model.Difference(33, 39);

            /* Update value in Model */
            model.setAU_Value(typeof(AU_LipStretched).ToString(), lips_corner_distance);

            /* print debug-values */
            if (debug)
            {
                model.View.Debug_Y += 20; // new row
                g.DrawString(debug_message + lips_corner_distance, model.DefaultFont, model.DefaultStringBrush, new Point(0, model.View.Debug_Y));
            }
        }
    }
}
