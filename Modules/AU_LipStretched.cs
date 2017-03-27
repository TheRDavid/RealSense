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

            lips_corner_distance =(model.Difference(33, 39) - 100);
            int d = Convert.ToInt32(lips_corner_distance);
            //lips_corner_distance /= 100;
            /* Update value in Model */
            model.setAU_Value(typeof(AU_LipStretched).ToString(), d);

            /* print debug-values */
            if (debug)
            {
                output = debug_message + d;

            }
        }
    }
}
