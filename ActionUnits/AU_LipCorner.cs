using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /**
     * Measures the height of the Lip-Corners and stores its' value inside the model.
     * @author Marlon
     * @HogwartsHouse Hufflepuff
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

        /**
         * Initializes the AU by setting up the default value boundaries.
         */
        public AU_LipCorner()
        {
            DEF_MIN = -1;
            DEF_MAX = 5;
            Reset();
            MIN_TOL = -1;
            MAX_TOL = 0.5;
            debug = true;
            XTREME_MAX = 45;
            XTREME_MIN = -36;
            model.AU_Values[typeof(AU_LipCorner).ToString() + "_left"] = 0;
            model.AU_Values[typeof(AU_LipCorner).ToString() + "_right"] = 0;
        }

        /** 
         * @Override 
         * Calculates the height of the Lipcorners over a set number of frames and prints its' debug-message to the CameraView when debug is enabled.
         * @param Graphics g for the view
         */
        public override void Work(Graphics g)
        {
            //Get Values from AU's
            cornerLeft = -((model.Difference(33, 36)) - 100);  //left LipCorner
            cornerRight = -((model.Difference(39, 36)) - 100);  //right LipCorner

            double hDiff = model.DifferenceByAxis(33, 35, Model.AXIS.Y, false) + model.DifferenceByAxis(39, 37, Model.AXIS.Y, false);

            //Gather Frames
            if (framesGathered < numFramesBeforeAccept)
            {
                cornersLeft[framesGathered] = cornerLeft;
                cornersRight[framesGathered++] = cornerRight;
            }
            else
            {
                FilterToleranceValues(cornersRight);
                FilterToleranceValues(cornersLeft);

                double leftDistance = FilteredAvg(cornersLeft);
                double rightDistance = FilteredAvg(cornersRight);

                DynamicMinMax(new double[] { leftDistance, rightDistance });

                double[] diffs = ConvertValues(new double[] { leftDistance, rightDistance });

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