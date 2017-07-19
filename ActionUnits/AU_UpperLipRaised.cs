using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /**
     * Measures whether or not the upper lip is raised and stores its' value inside the model.
     * @author Tobias Schramm
     * @HogwartsHouse Hufflepuff    
     * 
     * Interpretation:         0 = Relaxed
     *                       100 = Lip up
     */
    class AU_UpperLipRaised : RSModule
    {

        // variables for logic
        private double[] upperLip_Distance = new double[5];
        private double[] distances = new double[numFramesBeforeAccept];
        private double distance;
        private string debug_message = "UpperLipRaised: ";

        /**
         * Initializes the AU by setting up the default value boundaries.
         */
        public AU_UpperLipRaised()
        {
            DEF_MIN = -1;
            DEF_MAX = 8;
            reset();
            MIN_TOL = -1;
            MAX_TOL = 2;
            XTREME_MAX = 25;
            XTREME_MIN = -1;
            debug = true;
            model.AU_Values[typeof(AU_UpperLipRaised).ToString()] = 0;
        }

        /**
         * @Override 
         * Calculates the average difference between the upper lip and the nose to measure in which direction (and how far) it was moved over a set number of frames and prints its' debug-message to the CameraView when debug is enabled.
         * @param Graphics g for the view
         */
        public override void Work(Graphics g)
        {
            //Gather Frames
            if (framesGathered < numFramesBeforeAccept)
            {
                upperLip_Distance[0] = model.Difference(34, Model.NOSE_FIX);
                upperLip_Distance[1] = model.Difference(35, Model.NOSE_FIX);
                upperLip_Distance[2] = model.Difference(36, Model.NOSE_FIX);
                upperLip_Distance[3] = model.Difference(37, Model.NOSE_FIX);
                upperLip_Distance[4] = model.Difference(38, Model.NOSE_FIX);

                distance = (upperLip_Distance[0] + upperLip_Distance[1] + upperLip_Distance[2] + upperLip_Distance[3] + upperLip_Distance[4]) / 5;
                distance -= 100;
                distance *= -1;

                distances[framesGathered++] = distance;
            }
            else
            {
                filterToleranceValues(distances);

                double distance = filteredAvg(distances);

                dynamicMinMax(new double[] { distance });

                double[] diffs = convertValues(new double[] { distance });

                /* Update value in Model */
                if (model.CurrentPoseDiff < model.PoseMax)
                    model.AU_Values[typeof(AU_UpperLipRaised).ToString()] = diffs[0];

                /* print debug-values */
                if (debug)
                {
                    output = debug_message + "(" + (int)model.AU_Values[typeof(AU_UpperLipRaised).ToString()] + ") ("+ (int)MIN +", " + (int)MAX + ")";
                }
                framesGathered = 0;
            }
        }
    }
}
