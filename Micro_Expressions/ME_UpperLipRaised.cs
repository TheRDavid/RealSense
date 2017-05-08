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
    class ME_UpperLipRaised : RSModule
    {

        // variables for logic

        private double[] upperLip_Distance = new double[5];
        private double[] distances = new double[numFramesBeforeAccept];
        private double distance;
        private string debug_message = "UpperLipRaised: ";

        /**
         * Sets default-values
         */
        public ME_UpperLipRaised()
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

            if (framesGathered < numFramesBeforeAccept)
            {
                upperLip_Distance[0] = model.Difference(34, Model.NOSE_FIX);
                upperLip_Distance[1] = model.Difference(35, Model.NOSE_FIX);
                upperLip_Distance[2] = model.Difference(36, Model.NOSE_FIX);
                upperLip_Distance[3] = model.Difference(37, Model.NOSE_FIX);
                upperLip_Distance[4] = model.Difference(38, Model.NOSE_FIX);

                distance = (upperLip_Distance[0] + upperLip_Distance[1] + upperLip_Distance[2] + upperLip_Distance[3] + upperLip_Distance[4]) / 5;
                distance -= 100;
                distance *= -1;

                distances[framesGathered++] = distance;
            }
            else
            {
                filterToleranceValues(distances);

                double distance = filteredAvg(distances);

                dynamicMinMax(new double[] { distance });

                double[] diffs = convertValues(new double[] { distance });

                /* Update value in Model */
                model.setAU_Value(typeof(ME_UpperLipRaised).ToString(), diffs[0]);

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
