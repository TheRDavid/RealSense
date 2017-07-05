using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense
{

    /*
     * Measures if jaw is dropped and stores its' value inside the model.
     * @author René 
     * @HogwartsHouse Slytherin
     * 
     * Interpretation:      -100 = Doesn't usually happen
     *                         0 = Normal
     *                       100 = Drop it like it's hot
    */
    class AU_JawDrop : RSModule
    {
        // Variables for logic
        private double[] leftDistances = new double[numFramesBeforeAccept];
        private double[] rightDistances = new double[numFramesBeforeAccept];

        //chin vars
        double chin_dist;
        private double[] chinDistances = new double[numFramesBeforeAccept];

        // variables for debugging
        private string debug_message = "JawDrop: ";

        /**
         * Initializes the AU by setting up the default value boundaries.
         */
        public AU_JawDrop()
        {
            DEF_MIN = 0;
            DEF_MAX = 20;
            reset();
            MIN_TOL = -1.5;
            MAX_TOL = 1.5;
            debug = true;
            XTREME_MAX = 62;
            XTREME_MIN = 0;
            model.AU_Values[typeof(AU_JawDrop).ToString()] = 0;
        }

        /** 
         * @Override 
         * Calculates difference of lip-distance over a set number of frames and prints its' debug-message to the CameraView when debug is enabled.
         * @param Graphics g for the view
         */
        public override void Work(Graphics g)
        {
            //Get Values from AU's
            chin_dist = (Utilities.Difference(61, 26)) - 100;

            //Gather Frames
            if (framesGathered < numFramesBeforeAccept)
            {
                chinDistances[framesGathered++] = chin_dist;
            }
            else
            {
                /* Update value in Model */
                if (model.CurrentPoseDiff < model.PoseMax)
                {
                    model.AU_Values[typeof(AU_JawDrop).ToString()] = Utilities.ConvertValue(chinDistances, MAX, MIN, MAX_TOL, MIN_TOL, XTREME_MAX, XTREME_MIN);
                }

                /* print debug-values */
                if (debug)
                {
                    output = debug_message + "(" + (int)model.AU_Values[typeof(AU_JawDrop).ToString()] + ") (" + (int)MIN + ", " + ")";
                }
                framesGathered = 0;
            }
        }
    }
}

