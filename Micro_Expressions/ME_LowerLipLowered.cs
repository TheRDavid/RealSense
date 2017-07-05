using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /**
     * Measures whether the lower lip is lower
     * @author Tobias Schramm
     * @HogwartsHouse Hufflepuff
     * 
     * Interpretation:         0 = Relaxed
     *                       100 = Lip down
     */
    class ME_LowerLipLowered : RSModule
    {

        // variables for logic
        private double[] lowerLip_Distance = new double[5];
        private double distance;
        private double[] distances = new double[numFramesBeforeAccept];
        private string debug_message = "LowerLipLowered: ";

        // Default values
        public ME_LowerLipLowered()
        {
            DEF_MIN = -1;
            DEF_MAX = 4;
            reset();
            MIN_TOL = -1;
            MAX_TOL = 1;
            debug = true;
            XTREME_MAX = 8;
            XTREME_MIN = -6;
            model.AU_Values[typeof(ME_LowerLipLowered).ToString()] = 0;
        }

        /**
         *@Override 
         * Calculates the difference between the lower lip and the nose.
         * @param Graphics g for the view
         */
        public override void Work(Graphics g)
        {
            //Get Values from AU's
            lowerLip_Distance[0] = Utilities.Difference(44, Model.NOSE_FIX);
            lowerLip_Distance[1] = Utilities.Difference(43, Model.NOSE_FIX);
            lowerLip_Distance[2] = Utilities.Difference(42, Model.NOSE_FIX);
            lowerLip_Distance[3] = Utilities.Difference(41, Model.NOSE_FIX);
            lowerLip_Distance[4] = Utilities.Difference(40, Model.NOSE_FIX);
            distance = ((lowerLip_Distance[0] + lowerLip_Distance[1] + lowerLip_Distance[2] + lowerLip_Distance[3] + lowerLip_Distance[4]) / 5);
            distance -= 100;

            //Gather Frames
            if (framesGathered < numFramesBeforeAccept)
            {
                distances[framesGathered++] = distance;
            }
            else
            {
                /* Update value in Model */
                if (model.CurrentPoseDiff < model.PoseMax)
                    model.AU_Values[typeof(ME_LowerLipLowered).ToString()] = Utilities.ConvertValue(distances, MAX, MIN, MAX_TOL, MIN_TOL, XTREME_MAX, XTREME_MIN);

                /* print debug-values */
                if (debug)
                {
                    output = debug_message + "(" + (int)model.AU_Values[typeof(ME_LowerLipLowered).ToString()] + ") (" + (int)MIN + ", " + (int)MAX + ")";
                }
                framesGathered = 0;
            }
        }
    }
}
