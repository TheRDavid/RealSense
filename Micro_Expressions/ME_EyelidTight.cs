using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;



namespace RealSense
{
    /**
     * Measures tightening of eyelids (each eye)
     * @author: David Rosenbusch
     * @HogwartsHouse Hufflepuff
     */
    class ME_EyelidTight : RSModule
    {

        // variables for logic

        private double leftEye_leftDistance_diff, leftEye_middleDistance_diff, leftEye_rightDistance_diff,
            rightEye_leftDistance_diff, rightEye_middleDistance_diff, rightEye_rightDistance_diff;
        private double left_diff, right_diff;

        private double[] leftDistances = new double[numFramesBeforeAccept];
        private double[] rightDistances = new double[numFramesBeforeAccept];

        // variables for debugging

        private string debug_message = "EyelidTight: ";

        // Default values
        public ME_EyelidTight()
        {
            //values correct
            DEF_MIN = -34;
            DEF_MAX = 9;
            reset();
            MIN_TOL = -2;
            MAX_TOL = 2.5;
            debug = true;
            XTREME_MAX = 75;
            XTREME_MIN = -78;
        }

        /** 
         * @Override 
         * Calculates difference of lid-distance for each eye.
         * Result of calculation equal 100 -> no change, smaller 100 -> tightened, bigger 100 -> widened
         * See Images/AU_EyelidTightModule.png
         */
        public override void Work(Graphics g)
        {

            /* calculations */
            leftEye_leftDistance_diff = model.Difference(13, 15);
            leftEye_middleDistance_diff = model.Difference(12, 16);
            leftEye_rightDistance_diff = model.Difference(11, 17);

            rightEye_leftDistance_diff = model.Difference(19, 25);
            rightEye_middleDistance_diff = model.Difference(20, 24);
            rightEye_rightDistance_diff = model.Difference(21, 23);

            left_diff = ((leftEye_leftDistance_diff + leftEye_middleDistance_diff + leftEye_rightDistance_diff) / 3) - 100;
            right_diff = ((rightEye_leftDistance_diff + rightEye_middleDistance_diff + rightEye_rightDistance_diff) / 3) - 100;


            if (framesGathered < numFramesBeforeAccept)
            {
                leftDistances[framesGathered] = left_diff;
                rightDistances[framesGathered++] = right_diff;
            }
            else
            {
                for (int i = 0; i < numFramesBeforeAccept; i++)
                {
                    leftDistances[i] = leftDistances[i] < MAX_TOL && leftDistances[i] > MIN_TOL ? 0 : leftDistances[i];
                    rightDistances[i] = rightDistances[i] < MAX_TOL && rightDistances[i] > MIN_TOL ? 0 : rightDistances[i];
                }

                double leftDistance = filteredAvg(leftDistances);
                double rightDistance = filteredAvg(rightDistances);

                dynamicMinMax(new double[] { leftDistance, rightDistance });

                double[] diffs = convertValues(new double[] { leftDistance, rightDistance });

                /* Update value in Model */
                model.setAU_Value(typeof(ME_EyelidTight).ToString() + "_left", diffs[0]);
                model.setAU_Value(typeof(ME_EyelidTight).ToString() + "_right", diffs[1]); ;

                /* print debug-values */
                if (debug)
                {
                    output = debug_message + "(" + diffs[0] + ", " + diffs[1] + ")";
                }

                framesGathered = 0;
            }
        }
    }
}