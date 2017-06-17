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
     * Interpretation:         0 = Relaxed
     *                      -100 = Wrinkled
     */
    class ME_NoseWrinkled : RSModule
    {

        // variables for logic

        private double left_diff, right_diff;
        private double[] distances = new double[numFramesBeforeAccept];
        private double distance;
        private string debug_message = "NoseWrinkled: ";

        // Default values
        public ME_NoseWrinkled()
        {
            DEF_MIN = -1;
            DEF_MAX = 8;
            reset();
            MIN_TOL = -1;
            MAX_TOL = 1;
            debug = false;
            XTREME_MAX = 1;
            XTREME_MIN = -50; // god damnit rené
            model.AU_Values[typeof(ME_NoseWrinkled).ToString()] = 0;
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

            left_diff = model.Difference(30, Model.NOSE_FIX) - 100;
            right_diff = model.Difference(32, Model.NOSE_FIX) - 100;
            //middle_diff = model.Difference(31, Model.NOSE_FIX) - 100;

            distance = (left_diff + right_diff) / 2;

            if (framesGathered < numFramesBeforeAccept)
            {
                distances[framesGathered++] = distance;
            }
            else
            {
                filterToleranceValues(distances);

                double distance = filteredAvg(distances);

                dynamicMinMax(new double[] { distance });

                double[] diffs = convertValues(new double[] { distance });

                /* Update value in Model */
                if (model.CurrentPoseDiff < model.PoseMax)
                    model.AU_Values[typeof(ME_NoseWrinkled).ToString()] = diffs[0];

                /* print debug-values */
                if (debug)
                {
                    output = debug_message + " -> (" + (int)model.AU_Values[typeof(ME_NoseWrinkled).ToString()] + ") (" + (int)MIN + ", " + (int)MAX + ")";
                }
                framesGathered = 0;
            }
        }
    }
}
