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
    class AU_EyelidTight : RSModule
    {

        // variables for logic

        private double leftEye_leftDistance_diff, leftEye_middleDistance_diff, leftEye_rightDistance_diff, 
            rightEye_leftDistance_diff, rightEye_middleDistance_diff, rightEye_rightDistance_diff;
        private double left_diff, right_diff;

        // variables for debugging

        private string debug_message = "EyelidTight: ";

        // Default values
        public AU_EyelidTight()
        {
            debug = true;
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

            left_diff = left_diff < MAX_TOL && left_diff > MIN_TOL ? 0 : left_diff;
            right_diff = right_diff < MAX_TOL && right_diff > MIN_TOL ? 0 : right_diff;

            left_diff = filterExtremeValues(left_diff);
            right_diff = filterExtremeValues(right_diff);

            dynamicMinMax(new double[] { left_diff, right_diff });

            double[] diffs = convertValues(new double[] { left_diff, right_diff});

            model.setAU_Value(typeof(AU_BrowShift).ToString() + "_right", );

            /* Update value in Model */
            model.setAU_Value(typeof(AU_EyelidTight).ToString() + "_left", diffs[0]);
            model.setAU_Value(typeof(AU_EyelidTight).ToString() + "_right", diffs[1]); ;

            /* print debug-values */
            if (debug)
            {
                output = debug_message + "("+ diffs[0] + ", "+ diffs[1] + ")";
            }
        }
    }
}
