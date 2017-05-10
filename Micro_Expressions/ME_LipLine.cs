using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /*
     *@author David
     * 
     * Problem besteht noch bei weit offenen Mund, keine Unterscheidung mehr zwischen Runter und Hoch why is that german ? 

     * Lip Corner down does not work so far, landmarkpoints at the lip corner are not tracked when they go down,
     * no further sdk settings found, maybe recognize patterns via opencv?
     */
    class ME_LipLine : RSModule
    {

        private string debug_message = "LipLine: ";
        private double[] lines = new double[numFramesBeforeAccept];
        // Default values
        public ME_LipLine()
        {
            debug = true;
            DEF_MIN = -5;
            DEF_MAX = 40;
            reset();
            MIN_TOL = -1;
            MAX_TOL = 1;
            XTREME_MAX = 90;
            XTREME_MIN = -33;
            model.AU_Values[typeof(ME_LipLine).ToString()] = 0;
        }

        public override void Work(Graphics g)
        {
            /* Calculations */

            double line = model.DifferenceByAxis(33, 44, Model.AXIS.Y, false);
            line += model.DifferenceByAxis(33, 43, Model.AXIS.Y, false);
            line += model.DifferenceByAxis(39, 41, Model.AXIS.Y, false);
            line += model.DifferenceByAxis(39, 40, Model.AXIS.Y, false);
            line *= 1000;

            line = line < MAX_TOL && line > MIN_TOL ? 0 : line;

            if (framesGathered < numFramesBeforeAccept)
            {
                lines[framesGathered++] = line;
            }
            else
            {
                filterToleranceValues(lines);

                double distance = filteredAvg(lines);

                dynamicMinMax(new double[] { distance });

                double[] diffs = convertValues(new double[] { distance });

                /* Update value in Model */
                model.AU_Values[typeof(ME_LipLine).ToString()] = diffs[0];

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