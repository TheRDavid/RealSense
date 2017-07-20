using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense
{

    /**
     * Measures if lips are tightened and stores its' value inside the model.
     * @author René 
     * @HogwartsHouse Slytherin 
     * 
     * Interpretation:      -100 = Tight af
     *                         0 = normal
     *                       100 = doesn't usually happen
     */
    class AU_LipsTightened : RSModule
    {
        // Variables for logic
        double[] topDownDistances = new double[numFramesBeforeAccept];
        double upperLip;
        double bottomLip;

        /**
        * Initializes the AU by setting up the default value boundaries.
        */
        public AU_LipsTightened()
        {
            //values correct
            DEF_MIN = -3;
            DEF_MAX = 1;
            Reset();
            MIN_TOL = -1;
            MAX_TOL = 1;
            debug = true;
            XTREME_MAX = 0;
            XTREME_MIN = -16.5;
            model.AU_Values[typeof(AU_LipsTightened).ToString()] = 0;
        }

        /** 
         * @Override 
         * Calculates the average difference of the lip and the nose over a set number of frames and prints its' debug-message to the CameraView when debug is enabled.
         * @param Graphics g for the view
         */
        public override void Work(Graphics g)
        {
            //Get Values from AU's
            upperLip = (model.Difference(36, Model.NOSE_FIX) - 100);
            bottomLip = (model.Difference(50, Model.NOSE_FIX) - 100);

            double tdDist = (upperLip + bottomLip) / 2;

            //Gather Frames
            if (framesGathered < numFramesBeforeAccept)
            {
                topDownDistances[framesGathered++] = tdDist;
            }
            else
            {
                FilterToleranceValues(topDownDistances);

                double topDownDistance = FilteredAvg(topDownDistances);
                double[] diffs = new double[] { topDownDistance };
                DynamicMinMax(diffs);
                diffs = ConvertValues(diffs);

                // Update value in Model 
                if (model.CurrentPoseDiff < model.PoseMax)
                    model.AU_Values[typeof(AU_LipsTightened).ToString()] = diffs[0];
                if (debug)
                {
                    output = "LipsTightened: " + "(" + (int)model.AU_Values[typeof(AU_LipsTightened).ToString()] + ")(" + (int)MIN + ", " + (int)MAX + ")";
                }
                framesGathered = 0;
            }


        }



    }
}
