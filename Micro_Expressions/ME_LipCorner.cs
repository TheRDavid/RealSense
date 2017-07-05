using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /*
     * Measures the height of the Lip-Corners.
     * @author Marlon
     * 
     * Interpretation:      -100 = Corners down (not reliable, use LipLine) set to 0!
     *                       100 = Big stupid smile
     *                       
     */
    class ME_LipCorner : RSModule
    {
        // Variables for logic
        private double cornerLeft = 0, cornerRight = 0;
        private double[] cornersLeft = new double[numFramesBeforeAccept];
        private double[] cornersRight = new double[numFramesBeforeAccept];
        private string debug_message = "LipCorner: ";

        // Default values
        public ME_LipCorner()
        {
            DEF_MIN = -1;
            DEF_MAX = 5;
            reset();
            MIN_TOL = -1;
            MAX_TOL = 0.5;
            debug = false;
            XTREME_MAX = 45;
            XTREME_MIN = -36;
            model.AU_Values[typeof(ME_LipCorner).ToString() + "_left"] = 0;
            model.AU_Values[typeof(ME_LipCorner).ToString() + "_right"] = 0;
        }

        /** 
         * @Override 
         * Calculates the height of the Lipcorners.
         * @param Graphics g for the view
         */
        public override void Work(Graphics g)
        {
            //Get Values from AU's
            cornerLeft = -((Utilities.Difference(33, 36)) - 100);  //left LipCorner
            cornerRight = -((Utilities.Difference(39, 36)) - 100);  //right LipCorner


            double hDiff = Utilities.DifferenceByAxis(33, 35, Model.AXIS.Y, false) + Utilities.DifferenceByAxis(39, 37, Model.AXIS.Y, false);

            //Gather Frames
            if (framesGathered < numFramesBeforeAccept)
            {
                cornersLeft[framesGathered] = cornerLeft;
                cornersRight[framesGathered++] = cornerRight;
            }
            else
            {
                /* Update value in Model */
                if (model.CurrentPoseDiff < model.PoseMax)
                {
                    model.AU_Values[typeof(ME_LipCorner).ToString() + "_left"] = Utilities.ConvertValue(cornersLeft, MAX, MIN, MAX_TOL, MIN_TOL, XTREME_MAX, XTREME_MIN) * -1;
                    model.AU_Values[typeof(ME_LipCorner).ToString() + "_right"] = Utilities.ConvertValue(cornersRight, MAX, MIN, MAX_TOL, MIN_TOL, XTREME_MAX, XTREME_MIN) * -1;
                }
                /* print debug-values */
                if (debug)
                {
                    output = debug_message + "(" + (int)model.AU_Values[typeof(ME_LipCorner).ToString() + "_left"] + ", " + (int)model.AU_Values[typeof(ME_LipCorner).ToString() + "_right"] + ") (" + (int)MIN + ", " + (int)MAX + ") -> " + hDiff;
                }

                framesGathered = 0;
            }
        }
    }
}