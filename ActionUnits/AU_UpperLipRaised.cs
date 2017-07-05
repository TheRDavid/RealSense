using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /**
     * Measures whether the upper lip is raised
     * @author Tobias Schramm
     * @HogwartsHouse Hufflepuff
     */
    class ME_UpperLipRaised : RSModule
    {
        // variables for logic
        private double[] upperLip_Distance = new double[5];
        private double[] distances = new double[numFramesBeforeAccept];
        private double distance;
        private string debug_message = "UpperLipRaised: ";

        // Default values
        public ME_UpperLipRaised()
        {
            DEF_MIN = -1;
            DEF_MAX = 8;
            reset();
            MIN_TOL = -1;
            MAX_TOL = 2;
            XTREME_MAX = 25;
            XTREME_MIN = -1;
            debug = true;
            model.AU_Values[typeof(ME_UpperLipRaised).ToString()] = 0;
        }

        /**
         *@Override 
         * Calculates the difference between the upper lip and the nose.
         * @param Graphics g for the view
         */
        public override void Work(Graphics g)
        {
            //Gather Frames
            if (framesGathered < numFramesBeforeAccept)
            {
                //Get Values from AU's
                upperLip_Distance[0] = Utilities.Difference(34, Model.NOSE_FIX);
                upperLip_Distance[1] = Utilities.Difference(35, Model.NOSE_FIX);
                upperLip_Distance[2] = Utilities.Difference(36, Model.NOSE_FIX);
                upperLip_Distance[3] = Utilities.Difference(37, Model.NOSE_FIX);
                upperLip_Distance[4] = Utilities.Difference(38, Model.NOSE_FIX);

                distance = (upperLip_Distance[0] + upperLip_Distance[1] + upperLip_Distance[2] + upperLip_Distance[3] + upperLip_Distance[4]) / 5;
                distance -= 100;
                distance *= -1;

                distances[framesGathered++] = distance;
            }
            else
            {
                /* Update value in Model */
                if (model.CurrentPoseDiff < model.PoseMax)
                    model.AU_Values[typeof(ME_UpperLipRaised).ToString()] = Utilities.ConvertValue(distances, MAX, MIN, MAX_TOL, MIN_TOL, XTREME_MAX, XTREME_MIN);

                /* print debug-values */
                if (debug)
                {
                    output = debug_message + "(" + (int)model.AU_Values[typeof(ME_UpperLipRaised).ToString()] + ") (" + (int)MIN + ", " + (int)MAX + ")";
                }
                framesGathered = 0;
            }
        }
    }
}
