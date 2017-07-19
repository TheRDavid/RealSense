using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;



namespace RealSense
{
    /**
     * Measures wrinkling of the nose and stores its' value inside the model
     * @author: David Rosenbusch
     * @HogwartsHouse Hufflepuff
     * 
     * Interpretation:         0 = Relaxed
     *                      -100 = Wrinkled
     */
    class AU_NoseWrinkled : RSModule
    {

        // variables for logic
        private double left_diff, right_diff;
        private double[] distances = new double[numFramesBeforeAccept];
        private double distance;
        private string debug_message = "NoseWrinkled: ";

        /**
         * Initializes the AU by setting up the default value boundaries.
         */
        public AU_NoseWrinkled()
        {
            DEF_MIN = -1;
            DEF_MAX = 8;
            reset();
            MIN_TOL = -1;
            MAX_TOL = 1;
            debug = true;
            XTREME_MAX = 1;
            XTREME_MIN = -50; 
            model.AU_Values[typeof(AU_NoseWrinkled).ToString()] = 0;
        }

        /** 
         * @Override 
         * Have a look at Images/AU_NoseWrinkledModule.png to see the calculations visualized
         * Result of calculation constantly changing between positive and negative -> relaxed
         * Result of calculation constantly positive -> wrinkled (tiny values)
         * Result of calculation constantly negative -> ... go see a doctor m8
         * Calculates the wrinkling-value over a set number of frames and prints its' debug-message ti tge CameraView when debug is enabled.
         * @param Graphics g for the view
         */
        public override void Work(Graphics g)
        {
            //Get Values from AU's
            left_diff = model.Difference(30, Model.NOSE_FIX) - 100;
            right_diff = model.Difference(32, Model.NOSE_FIX) - 100;
            //middle_diff = model.Difference(31, Model.NOSE_FIX) - 100;

            distance = (left_diff + right_diff) / 2;
            
            //Gather Frames
            if (framesGathered < numFramesBeforeAccept)
            {
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
                    model.AU_Values[typeof(AU_NoseWrinkled).ToString()] = diffs[0];

                /* print debug-values */
                if (debug)
                {
                    output = debug_message + " -> (" + (int)model.AU_Values[typeof(AU_NoseWrinkled).ToString()] + ") (" + (int)MIN + ", " + (int)MAX + ")";
                }
                framesGathered = 0;
            }
        }
    }
}
