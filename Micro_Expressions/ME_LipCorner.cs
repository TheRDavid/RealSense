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
     * Problem besteht noch bei weit offenen Mund, keine Unterscheidung mehr zwischen Runter und Hoch why is that german ? 

     * Lip Corner down does not work so far, landmarkpoints at the lip corner are not tracked when they go down,
     * no further sdk settings found, maybe recognize patterns via opencv?
     */
    class ME_LipCorner : RSModule
    {
        // Variables for logic
        
        private double cornerLeft = 0, cornerRight = 0;
        private double[] cornersLeft = new double[numFramesBeforeAccept];
        private double[] cornersRight = new double[numFramesBeforeAccept];
        private string debug_message = "LipCorner: ";

        // Variables for debugging

        // Default values
        public ME_LipCorner()
        {
            DEF_MIN = 0;
            DEF_MAX = 5;
            reset();
            MIN_TOL = -1;
            MAX_TOL = 0.5;
            debug = true;
            XTREME_MAX = 45;
            XTREME_MIN = -36;
        }

        public override void Work(Graphics g)
        {
            /* Calculations */

            // calculates difference between nose and LipCorner 
            cornerLeft = -((model.Difference(33, 26)) - 100);  //left LipCorner
            cornerRight = -((model.Difference(39, 26)) - 100);  //right LipCorner


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
                model.setAU_Value(typeof(ME_LipCorner).ToString() + "_left", diffs[0]);
                model.setAU_Value(typeof(ME_LipCorner).ToString() + "_right", diffs[1]); ;

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