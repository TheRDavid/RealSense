using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense
{
    /**
     * Measures how much the lower lip is raised.
     * @author: not known
     * Interpretation:         0 = Relaxed
     *                      -100 = Wrinkled
     */
    class ME_LowerLipRaised : RSModule
    {
        // variables for logic
        private double[] upperLip_Distance = new double[2];
        private double[] distances = new double[numFramesBeforeAccept];
        private double distance;
        private string debug_message = "LowerLipRaised: ";

        // Default values
        public ME_LowerLipRaised()
        {
            DEF_MIN = -1;
            DEF_MAX = 8;
            reset();
            MIN_TOL = -1;
            MAX_TOL = 2;
            XTREME_MAX = 25;
            XTREME_MIN = -1;
            debug = true;
            model.AU_Values[typeof(ME_LowerLipRaised).ToString()] = 0;
        }

        /**
         *@Override 
         * Calculates the difference between the lower lip and the nose.
         * @param Graphics g for the view
         */
        public override void Work(Graphics g)
        {
            //Gather Frames
            if (framesGathered < numFramesBeforeAccept)
            {
                //Get Values from AU's
                upperLip_Distance[0] = Utilities.Difference(42, Model.NOSE_FIX);
                upperLip_Distance[1] = Utilities.Difference(51, Model.NOSE_FIX);

                distance = (upperLip_Distance[0] + upperLip_Distance[1]) / 2;
                distance -= 100;
                distance *= -1;

                distances[framesGathered++] = distance;
            }
            else
            {
                /* Update value in Model */
                if (model.CurrentPoseDiff < model.PoseMax)
                    model.AU_Values[typeof(ME_LowerLipRaised).ToString()] = Utilities.ConvertValue(distances, MAX, MIN, MAX_TOL, MIN_TOL, XTREME_MAX, XTREME_MIN);

                /* print debug-values */
                if (debug)
                {
                    output = debug_message + "(" + (int)model.AU_Values[typeof(ME_LowerLipRaised).ToString()] + ") (" + (int)MIN + ", " + (int)MAX + ")";
                }
                framesGathered = 0;
            }
        }

    }
}
