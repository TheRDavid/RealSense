using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /**
     * Measures if complete brow is raised or lowered (each eye) and stores its' value inside the model.
     * @author Anton 
     * @HogwartsHouse Slytherin  
     * 
     * Interpretation:      -100 = Brows down (grumpy af)
     *                       100 = Brows up
     */
    class AU_BrowShift : RSModule
    {
        // Variables for logic
        private double[] leftDistances = new double[numFramesBeforeAccept];
        private double[] rightDistances = new double[numFramesBeforeAccept];
        private double leftEyeBrow_r, leftEyeBrow_m, leftEyeBrow_l;
        private double rightEyeBrow_r, rightEyeBrow_m, rightEyeBrow_l;

        /**
         * Initializes the AU by setting up the default value boundaries.
         */
        public AU_BrowShift()
        {
            DEF_MIN = -7;
            DEF_MAX = 12;
            Reset();
            MIN_TOL = -2;
            MAX_TOL = 1;
            debug = true;
            XTREME_MAX = 33;
            XTREME_MIN = -24;


            model.AU_Values[typeof(AU_BrowShift).ToString() + "_left"] = 0;
            model.AU_Values[typeof(AU_BrowShift).ToString() + "_right"] = 0;
        }

        /**
         * @Override 
         * Computes the percentage Value of BrowShift in the current Frame over a set number of frames and prints its' debug-message to the CameraView when debug is enabled.
         * @param Graphics g for the view
         * */
        public override void Work(Graphics g)
        {
            //Get Values from AU's
            leftEyeBrow_r = model.Difference(0, Model.NOSE_FIX) - 100;
            leftEyeBrow_m = model.Difference(2, Model.NOSE_FIX) - 100;
            leftEyeBrow_l = model.Difference(4, Model.NOSE_FIX) - 100;

            rightEyeBrow_r = model.Difference(9, Model.NOSE_FIX) - 100;
            rightEyeBrow_m = model.Difference(7, Model.NOSE_FIX) - 100;
            rightEyeBrow_l = model.Difference(5, Model.NOSE_FIX) - 100;

            //Average
            double ld = ((leftEyeBrow_r + leftEyeBrow_m + leftEyeBrow_l) / 3) * 1;
            double rd = ((rightEyeBrow_r + rightEyeBrow_m + rightEyeBrow_l) / 3) * 1;

            //Gather Frames
            if (framesGathered < numFramesBeforeAccept)
            {
                leftDistances[framesGathered] = ld;
                rightDistances[framesGathered++] = rd;
            }
            else
            {
                FilterToleranceValues(leftDistances);
                FilterToleranceValues(rightDistances);

                double leftDistance = FilteredAvg(leftDistances);
                double rightDistance = FilteredAvg(rightDistances);

                DynamicMinMax(new double[] { leftDistance, rightDistance });

                double[] diffs = ConvertValues(new double[] { leftDistance, rightDistance });

                if (model.CurrentPoseDiff < model.PoseMax)
                {
                    //set Values
                    model.AU_Values[typeof(AU_BrowShift).ToString() + "_left"] = diffs[0];
                    model.AU_Values[typeof(AU_BrowShift).ToString() + "_right"] = diffs[1];
                }

                // print debug-values 
                if (debug)
                {
                    output = "BrowShift: " + "(" + (int)model.AU_Values[typeof(AU_BrowShift).ToString() + "_left"]  + ", " + (int)model.AU_Values[typeof(AU_BrowShift).ToString() + "_right"] + ")(" + (int)MIN + ", " + (int)MAX + ")";
                }

                framesGathered = 0;
            }
        }

    }
}
