using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /*
     *@author Marlon
     * 
     * Interpretation:      -100 = Corners down (not reliable, use LipLine) set to 0!
     *                       100 = Big stupid smile
     *                       
     */
    class AU_LipCorner : RSModule
    {
        // Variables for logic

        private double cornerLeft = 0, cornerRight = 0;
        private double[] cornersLeft = new double[numFramesBeforeAccept];
        private double[] cornersRight = new double[numFramesBeforeAccept];
        private string debug_message = "LipCorner: ";

        // Variables for debugging

        // Default values
        public AU_LipCorner()
        {
            DEF_MIN = -1;
            DEF_MAX = 5;
            reset();
            MIN_TOL = -1;
            MAX_TOL = 0.5;
            debug = true;
            XTREME_MAX = 45;
            XTREME_MIN = -36;
            model.AU_Values[typeof(AU_LipCorner).ToString() + "_left"] = 0;
            model.AU_Values[typeof(AU_LipCorner).ToString() + "_right"] = 0;
        }

        public override void Work(Graphics g)
        {
            /* Calculations */

            // calculates difference between nose and LipCorner 
            cornerLeft = -((model.Difference(33, 36)) - 100);  //left LipCorner
            cornerRight = -((model.Difference(39, 36)) - 100);  //right LipCorner


            double hDiff = model.DifferenceByAxis(33, 35, Model.AXIS.Y, false) + model.DifferenceByAxis(39, 37, Model.AXIS.Y, false);

            if (framesGathered < numFramesBeforeAccept)
            {
                cornersLeft[framesGathered] = cornerLeft;
                cornersRight[framesGathered++] = cornerRight;
            }
            else
            {
                filterToleranceValues(cornersRight);
                filterToleranceValues(cornersLeft);

                double leftDistance = filteredAvg(cornersLeft);
                double rightDistance = filteredAvg(cornersRight);

                dynamicMinMax(new double[] { leftDistance, rightDistance });

                double[] diffs = convertValues(new double[] { leftDistance, rightDistance });

                /* Update value in Model */
                if (model.CurrentPoseDiff < model.PoseMax)
                {
                    model.AU_Values[typeof(AU_LipCorner).ToString() + "_left"] = diffs[0] * -1; //war falschherum?
                    model.AU_Values[typeof(AU_LipCorner).ToString() + "_right"] = diffs[1] * -1; //war falschherum?
                }
                /* print debug-values */
                if (debug)
                {
                    output = debug_message + "(" + (int)model.AU_Values[typeof(AU_LipCorner).ToString() + "_left"] + ", " + (int)model.AU_Values[typeof(AU_LipCorner).ToString() + "_right"] + ") (" + (int)MIN + ", " + (int)MAX + ") -> " + hDiff;
                }

                framesGathered = 0;
            }
        }
    }
}