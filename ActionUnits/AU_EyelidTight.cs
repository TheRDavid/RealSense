using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;



namespace RealSense
{
    /**
     * Measures tightening of eyelids (each eye) and stores its' value inside the model.
     * @author: David Rosenbusch
     * @HogwartsHouse Hufflepuff
     * 
     * Interpretation:      -100 = Eyes squinted
     *                       100 = Eyes wide open
     */
    class AU_EyelidTight : RSModule
    {

        // variables for logic
        private double leftEye_leftDistance_diff, leftEye_middleDistance_diff, leftEye_rightDistance_diff,
            rightEye_leftDistance_diff, rightEye_middleDistance_diff, rightEye_rightDistance_diff;
        private double left_diff, right_diff;
        private double[] leftDistances = new double[numFramesBeforeAccept];
        private double[] rightDistances = new double[numFramesBeforeAccept];

        /**
         * Initializes the AU by setting up the default value boundaries.
         */
        public AU_EyelidTight()
        {
            //values correct
            DEF_MIN = -34;
            DEF_MAX = 9;
            Reset();
            MIN_TOL = -12;
            MAX_TOL = 12;
            debug = true;
            XTREME_MAX = 75;
            XTREME_MIN = -78;

            if (model.CurrentPoseDiff < 10)
            {
                model.AU_Values[typeof(AU_EyelidTight).ToString() + "_left"] = 0;
                model.AU_Values[typeof(AU_EyelidTight).ToString() + "_right"] = 0;
            }
        }

        /** 
         * @Override 
         * Calculates difference of lid-distance for each eye over a set number of frames and prints its' debug-message to the CameraView when debug is enabled.
         * @param Graphics g for the view
         */
        public override void Work(Graphics g)
        {
            //Get Values from AU's
            leftEye_leftDistance_diff = model.Difference(13, 15);
            leftEye_middleDistance_diff = model.Difference(12, 16);
            leftEye_rightDistance_diff = model.Difference(11, 17);

            rightEye_leftDistance_diff = model.Difference(19, 25);
            rightEye_middleDistance_diff = model.Difference(20, 24);
            rightEye_rightDistance_diff = model.Difference(21, 23);

            left_diff = ((leftEye_leftDistance_diff + leftEye_middleDistance_diff + leftEye_rightDistance_diff) / 3) - 100;
            right_diff = ((rightEye_leftDistance_diff + rightEye_middleDistance_diff + rightEye_rightDistance_diff) / 3) - 100;

            //Gather Frames
            if (framesGathered < numFramesBeforeAccept)
            {
                leftDistances[framesGathered] = left_diff;
                rightDistances[framesGathered++] = right_diff;
            }
            else
            {
                FilterToleranceValues(rightDistances);
                FilterToleranceValues(leftDistances);

                double leftDistance = FilteredAvg(leftDistances);
                double rightDistance = FilteredAvg(rightDistances);

                DynamicMinMax(new double[] { leftDistance, rightDistance });

                double[] diffs = ConvertValues(new double[] { leftDistance, rightDistance });

                if (model.CurrentPoseDiff < model.PoseMax)
                {
                    //set Values
                    model.AU_Values[typeof(AU_EyelidTight).ToString() + "_left"] = diffs[0];
                    model.AU_Values[typeof(AU_EyelidTight).ToString() + "_right"] = diffs[1];
                }

                /* print debug-values */
                if (debug)
                {
                    output = "Eyelid_Tight: " + "(" + (int)model.AU_Values[typeof(AU_EyelidTight).ToString() + "_left"] + ", " + (int)model.AU_Values[typeof(AU_EyelidTight).ToString() + "_right"] + ")(" + (int)MIN + ", " + (int)MAX + ")";
                }

                framesGathered = 0;
            }
        }
    }
}