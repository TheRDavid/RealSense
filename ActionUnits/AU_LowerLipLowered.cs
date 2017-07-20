using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /**
     * Measures whether the lower lip is lower and stores its' value inside the model
     * @author Tobias Schramm
     * @HogwartsHouse Hufflepuff
     * 
     * Interpretation:         0 = Relaxed
     *                       100 = Lip down
     */
    class AU_LowerLipLowered : RSModule
    {

        // variables for logic
        private double[] lowerLip_Distance = new double[5];
        private double distance;
        private double[] distances = new double[numFramesBeforeAccept];
        private string debug_message = "LowerLipLowered: ";

        /**
         * Initializes the AU by setting up the default value boundaries.
         */
        public AU_LowerLipLowered()
        {
            DEF_MIN = -1;
            DEF_MAX = 4;
            Reset();
            MIN_TOL = -1;
            MAX_TOL = 1;
            debug = true;
            XTREME_MAX = 8;
            XTREME_MIN = -6;
            model.AU_Values[typeof(AU_LowerLipLowered).ToString()] = 0;
        }

        /**
         * @Override 
         * Calculates the average difference between the lower lip and the nose to see if the lower lip was moved over a set number of frames and prints its' debug-message to the CameraView when debug is enabled.
         * @param Graphics g for the view
         */
        public override void Work(Graphics g)
        {
            //Get Values from AU's
            lowerLip_Distance[0] = model.Difference(44, Model.NOSE_FIX);
            lowerLip_Distance[1] = model.Difference(43, Model.NOSE_FIX);
            lowerLip_Distance[2] = model.Difference(42, Model.NOSE_FIX);
            lowerLip_Distance[3] = model.Difference(41, Model.NOSE_FIX);
            lowerLip_Distance[4] = model.Difference(40, Model.NOSE_FIX);
            distance = ((lowerLip_Distance[0] + lowerLip_Distance[1] + lowerLip_Distance[2] + lowerLip_Distance[3] + lowerLip_Distance[4]) / 5);
            distance -= 100;

            //Gather Frames
            if (framesGathered < numFramesBeforeAccept)
            {
                distances[framesGathered++] = distance;
            }
            else
            {
                FilterToleranceValues(distances);

                double distance = FilteredAvg(distances);

                DynamicMinMax(new double[] { distance });

                double[] diffs = ConvertValues(new double[] { distance });

                /* Update value in Model */
                if (model.CurrentPoseDiff < model.PoseMax)
                    model.AU_Values[typeof(AU_LowerLipLowered).ToString()] = diffs[0];

                /* print debug-values */
                if (debug)
                {
                    output = debug_message + "(" + (int)model.AU_Values[typeof(AU_LowerLipLowered).ToString()] + ") ("+ (int)MIN +", " + (int)MAX + ")";
                }
                framesGathered = 0;
            }
        }
    }
}
