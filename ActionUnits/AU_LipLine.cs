using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /*
    * Measures how accurate the lip-landmark-points are on one line and stores its' value inside the model.
    * @author David
    * @HogwartsHouse Hufflepuff
    *
    * Interpretation:      -100 = Saaaaaad
    *                         0 = Relaxed
    *                       100 = Grinning
    */
    class AU_LipLine : RSModule
    {
        // Variables for logic
        private string debug_message = "LipLine: ";
        private double[] lines = new double[numFramesBeforeAccept];

        /**
         * Initializes the AU by setting up the default value boundaries.
         */
        public AU_LipLine()
        {
            debug = true;
            DEF_MIN = -5;
            DEF_MAX = 40;
            reset();
            MIN_TOL = -1;
            MAX_TOL = 1;
            XTREME_MAX = 90;
            XTREME_MIN = -33;
            model.AU_Values[typeof(AU_LipLine).ToString()] = 0;
        }

        /** 
         * @Override 
         * Calculates whether or not all LipPoints are placed on one line over a set number of frames and prints its' debug-message to the CameraView when debug is enabled.
         * @param Graphics g for the view
         */
        public override void Work(Graphics g)
        {
            //Get Values from AU's
            double line = model.DifferenceByAxis(33, 44, Model.AXIS.Y, false);
            line += model.DifferenceByAxis(33, 43, Model.AXIS.Y, false);
            line += model.DifferenceByAxis(39, 41, Model.AXIS.Y, false);
            line += model.DifferenceByAxis(39, 40, Model.AXIS.Y, false);
            line += model.DifferenceByAxis(39, 42, Model.AXIS.Y, false);
            line *= 1000;

            line = line < MAX_TOL && line > MIN_TOL ? 0 : line;

            //Gather Frames
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
                if (model.CurrentPoseDiff < model.PoseMax)
                    model.AU_Values[typeof(AU_LipLine).ToString()] = diffs[0];

                /* print debug-values */
                if (debug)
                {
                    output = debug_message + "(" + (int)model.AU_Values[typeof(AU_LipLine).ToString()] + ") (" + (int)MAX + ", " + (int)MIN + ")";
                }

                framesGathered = 0;
            }
        }
    }
}