using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /*
     * Measures how accurate the lip-landmark-points are on one line.
     * @author David
     *
     * Interpretation:      -100 = Saaaaaad
     *                         0 = Relaxed
     *                       100 = Grinning
     */
    class ME_LipLine : RSModule
    {
        // Variables for logic
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

        /** 
         * @Override 
         * Calculates if all Lippoints are placed in one line.
         * @param Graphics g for the view
         */
        public override void Work(Graphics g)
        {
            //Get Values from AU's
            double line = Utilities.DifferenceByAxis(33, 44, Model.AXIS.Y, false);
            line += Utilities.DifferenceByAxis(33, 43, Model.AXIS.Y, false);
            line += Utilities.DifferenceByAxis(39, 41, Model.AXIS.Y, false);
            line += Utilities.DifferenceByAxis(39, 40, Model.AXIS.Y, false);
            line += Utilities.DifferenceByAxis(39, 42, Model.AXIS.Y, false);
            line *= 1000;

            line = line < MAX_TOL && line > MIN_TOL ? 0 : line;

            //Gather Frames
            if (framesGathered < numFramesBeforeAccept)
            {
                lines[framesGathered++] = line;
            }
            else
            {
                /* Update value in Model */
                if (model.CurrentPoseDiff < model.PoseMax)
                    model.AU_Values[typeof(ME_LipLine).ToString()] = Utilities.ConvertValue(lines, MAX, MIN, MAX_TOL, MIN_TOL, XTREME_MAX, XTREME_MIN);

                /* print debug-values */
                if (debug)
                {
                    output = debug_message + "(" + (int)model.AU_Values[typeof(ME_LipLine).ToString()] + ") (" + (int)MAX + ", " + (int)MIN + ")";
                }

                framesGathered = 0;
            }
        }
    }
}