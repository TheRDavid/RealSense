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
    class AU_LipLine : RSModule
    {
        // Default values
        public AU_LipLine()
        {
            debug = true;
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

            line = filterExtremeValues(line);

            dynamicMinMax(new double[] { line });

            double[] diffs = convertValues(new double[] { line });

            // Update value in Model 
            model.setAU_Value(typeof(AU_LipLine).ToString() + "_line", diffs[0]);

            // print debug-values 
            if (debug)
            {
                output = "LipLine: " + (int)diffs[0];
            }
        }
    }
}