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
    class ME_LipStretched : RSModule
    {

        // variables for logic

        private double lips_corner_distance;
        double[] lips_corner_distances = new double[numFramesBeforeAccept];
        private string debug_message = "LipStretched: ";

        /**
         * Sets default-valuesC:\Users\Tanja\Source\Repos\RealSense\Modules\AU_LipStretched.cs
         */
        public ME_LipStretched()
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
         * @Override 
         * Calculates the difference between the two lip corners
         */
        public override void Work(Graphics g)
        {
            /* calculations */

            lips_corner_distance = (model.Difference(33, 39) - 100);

            if (framesGathered < numFramesBeforeAccept)
            {
                lips_corner_distances[framesGathered++] = lips_corner_distance;
            }
            else
            {
                filterToleranceValues(lips_corner_distances);

                double distance = filteredAvg(lips_corner_distances);

                dynamicMinMax(new double[] { distance });

                double[] diffs = convertValues(new double[] { distance });

                /* Update value in Model */
                model.setAU_Value(typeof(ME_LipLine).ToString(), diffs[0]);

                /* print debug-values */
                if (debug)
                {
                    output = debug_message + "(" + diffs[0] + ")";
                }
                framesGathered = 0;
            }
        }
    }
}
