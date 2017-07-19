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
     * 
     * Interpretation:      -100 = Kissing
     *                         0 = Relaxed
     *                       100 = Frogface
     */
    class AU_LipStretched : RSModule
    {

        // variables for logic

        private double lips_corner_distance;
        double[] lips_corner_distances = new double[numFramesBeforeAccept];
        private string debug_message = "LipStretched: ";

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
            model.AU_Values[typeof(AU_LipStretched).ToString()] = 0;
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
                if (model.CurrentPoseDiff < model.PoseMax)
                    model.AU_Values[typeof(AU_LipStretched).ToString()] = diffs[0];

                /* print debug-values */
                if (debug)
                {
                    output = debug_message + "(" + (int)model.AU_Values[typeof(AU_LipStretched).ToString()] + ") ("+ (int)MIN + ", " + (int)MAX + ")";
                }
                framesGathered = 0;
            }
        }
    }
}
