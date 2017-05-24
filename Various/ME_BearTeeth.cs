using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense
{
    class ME_BearTeeth : RSModule
    {


        // variables for logic

        private double lipDistance = 0;
        private double[] distances = new double[numFramesBeforeAccept];
        private string debug_message = "Bear Theeth: ";

        /**
         * Sets default-values
         */
        public ME_BearTeeth()
        {
            DEF_MIN = -1;
            DEF_MAX = 8;
            reset();
            MIN_TOL = -1;
            MAX_TOL = 2;
            XTREME_MAX = 100;
            XTREME_MIN = -1;
            debug = true;
            model.AU_Values[typeof(ME_BearTeeth).ToString()] = 0;
        }

        /**
         *@Override 
         * Calculates the difference between the two lip corners
         */
        public override void Work(Graphics g)
        {
            /* calculations */


            if (framesGathered < numFramesBeforeAccept)
            {
                lipDistance = 0;
                lipDistance += model.Difference(52, 46);
                lipDistance += model.Difference(51, 47);
                lipDistance += model.Difference(50, 48);
                lipDistance /= 3;

                if (Math.Abs(model.AU_Values[typeof(ME_NoseWrinkled).ToString()]) > 30)
                    distances[framesGathered++] = lipDistance;
                else distances[framesGathered++] = 0;
            }
            else
            {
                filterToleranceValues(distances);

                double distance = filteredAvg(distances);

                dynamicMinMax(new double[] { distance });

                double[] diffs = convertValues(new double[] { distance });

                /* Update value in Model */
                if (model.CurrentPoseDiff < model.PoseMax)
                    model.AU_Values[typeof(ME_BearTeeth).ToString()] = diffs[0];

                /* print debug-values */
                if (debug)
                {
                    output = debug_message + "(" + (int)model.AU_Values[typeof(ME_BearTeeth).ToString()] + ") (" + (int)MIN + ", " + (int)MAX + ")";
                }
                framesGathered = 0;
            }
        }

    }
}
