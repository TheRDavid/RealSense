using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /*
     *Measures if complete brow is raised or lowered (each eye)  - Action Unit Number 4 
     *@author Anton 
     *@date 20.03.2017
     *@HogwartsHouse Slytherin  
     * 
     * Interpretation:      -100 = Brows down (grumpy af)
     *                       100 = Brows up
     */
    class AU_BrowShift : RSModule
    {
        // Variables for logic


        private double[] leftDistances = new double[numFramesBeforeAccept];
        private double[] rightDistances = new double[numFramesBeforeAccept];
        private double leftEyeBrow_r, leftEyeBrow_m, leftEyeBrow_l;
        private double rightEyeBrow_r, rightEyeBrow_m, rightEyeBrow_l;

        // Variables for debugging

        // Default values
        public AU_BrowShift()
        {
            DEF_MIN = -7;
            DEF_MAX = 12;
            reset();
            MIN_TOL = -2;
            MAX_TOL = 1;
            debug = true;
            XTREME_MAX = 33;
            XTREME_MIN = -24;


            model.AU_Values[typeof(AU_BrowShift).ToString() + "_left"] = 0;
            model.AU_Values[typeof(AU_BrowShift).ToString() + "_right"] = 0;
        }

        public override void Work(Graphics g)
        {
                /* Calculations */

                // calculates the difference between the Nullface and the currentface -> to check if the whole eyebrow is raised or lowered

                leftEyeBrow_r = model.Difference(0, Model.NOSE_FIX) - 100;
            leftEyeBrow_m = model.Difference(2, Model.NOSE_FIX) - 100;
            leftEyeBrow_l = model.Difference(4, Model.NOSE_FIX) - 100;

            rightEyeBrow_r = model.Difference(9, Model.NOSE_FIX) - 100;
            rightEyeBrow_m = model.Difference(7, Model.NOSE_FIX) - 100;
            rightEyeBrow_l = model.Difference(5, Model.NOSE_FIX) - 100;

            double ld = ((leftEyeBrow_r + leftEyeBrow_m + leftEyeBrow_l) / 3) * 1;
            double rd = ((rightEyeBrow_r + rightEyeBrow_m + rightEyeBrow_l) / 3) * 1;

            if (framesGathered < numFramesBeforeAccept)
            {
                leftDistances[framesGathered] = ld;
                rightDistances[framesGathered++] = rd;
            }
            else
            {
                filterToleranceValues(leftDistances);
                filterToleranceValues(rightDistances);

                double leftDistance = filteredAvg(leftDistances);
                double rightDistance = filteredAvg(rightDistances);

                dynamicMinMax(new double[] { leftDistance, rightDistance });

                double[] diffs = convertValues(new double[] { leftDistance, rightDistance });

                if (model.CurrentPoseDiff < model.PoseMax)
                {
                    model.AU_Values[typeof(AU_BrowShift).ToString() + "_left"] = diffs[0];
                    model.AU_Values[typeof(AU_BrowShift).ToString() + "_right"] = diffs[1];
                }

                // print debug-values 
                if (debug)
                {
                    output = "BrowShift: " + "(" + (int)model.AU_Values[typeof(AU_BrowShift).ToString() + "_left"]  + ", " + (int)model.AU_Values[typeof(AU_BrowShift).ToString() + "_right"] + ")(" + (int)MIN + ", " + (int)MAX + ")";
                }

                framesGathered = 0;
            }
        }

    }
}
