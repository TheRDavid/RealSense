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

        /**
         * Sets default-valuesC:\Users\Tanja\Source\Repos\RealSense\Modules\AU_LipStretched.cs
         */
        public AU_LipStretched()
        {
          
            //correct values
            DEF_MIN = -13;
            DEF_MAX = 13;
            reset();
            MIN_TOL = -2;
            MAX_TOL = 0.5;
            debug = true;
            XTREME_MAX = 60;
            XTREME_MIN = -45;
        }

        /**
         *@Override 
         * Calculates the difference between the two lip corners
         */
        public override void Work(Graphics g)
        {
            /* calculations */

            lips_corner_distance =(model.Difference(33, 39) - 100);

            lips_corner_distance = lips_corner_distance < MAX_TOL && lips_corner_distance > MIN_TOL ? 0 : lips_corner_distance;

            lips_corner_distance = filterExtremeValues(lips_corner_distance);

            dynamicMinMax(new double[] { lips_corner_distance });

            double[] diffs = convertValues(new double[] { lips_corner_distance });

            /* Update value in Model */
            model.setAU_Value(typeof(AU_LipStretched).ToString(), diffs[0]);

            /* print debug-values */
            if (debug)
            {
                output = "LipStreched: " + diffs[0];

            }
        }
    }
}
