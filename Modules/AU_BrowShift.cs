using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /*
     *Measures if complete brow is raised or lowered (each eye)  - Action Unit Number 4 
     *@author Anton 
     *@date 20.03.2017
     *@HogwartsHouse Slytherin  
     */
    class AU_BrowShift : RSModule
    {
        // Variables for logic


        private double leftDistance;
        private double rightDistance;
        private double leftEyeBrow_r, leftEyeBrow_m, leftEyeBrow_l;
        private double rightEyeBrow_r, rightEyeBrow_m, rightEyeBrow_l;

        // Variables for debugging

        // Default values
        public AU_BrowShift()
        {
            DEF_MIN = -7;
            DEF_MAX = 12;
            reset();
            MIN_TOL = -2;
            MAX_TOL = 1;
            debug = true;
        }

        public override void Work(Graphics g)
        {
            /* Calculations */

            // calculates the difference between the Nullface and the currentface -> to check if the whole eyebrow is raised or lowered

            leftEyeBrow_r = model.Difference(0, Model.NOSE_FIX) - 100;
            leftEyeBrow_m = model.Difference(2, Model.NOSE_FIX) - 100;
            leftEyeBrow_l = model.Difference(4, Model.NOSE_FIX) - 100;

            rightEyeBrow_r = model.Difference(9, Model.NOSE_FIX) - 100;
            rightEyeBrow_m = model.Difference(7, Model.NOSE_FIX) - 100;
            rightEyeBrow_l = model.Difference(5, Model.NOSE_FIX) - 100;


            leftDistance = ((leftEyeBrow_r + leftEyeBrow_m + leftEyeBrow_l) / 3) * 1;
            rightDistance = ((rightEyeBrow_r + rightEyeBrow_m + rightEyeBrow_l) / 3) * 1;

            leftDistance = leftDistance < MAX_TOL && leftDistance > MIN_TOL ? 0 : leftDistance;
            rightDistance = rightDistance < MAX_TOL && rightDistance > MIN_TOL ? 0 : rightDistance;

            dynamicMinMax(new double[]{leftDistance, rightDistance});     

            double[] diffs = convertValues(new double[] { leftDistance, rightDistance });
            
            
            model.setAU_Value(typeof(AU_BrowShift).ToString() + "_left", diffs[0]);
            model.setAU_Value(typeof(AU_BrowShift).ToString() + "_right", diffs[1]);


            // print debug-values 
            if (debug)
            {
                output = "BrowShift: " + "(" + (int)diffs[0] + ", " + (int)diffs[1] + ")("+ (int)MIN +", "+ (int)MAX +")";
            }
        }

        /*
        protected override void dynamicMinMax()                                                                     T
        {
            double temp = leftDistance > rightDistance ? rightDistance : leftDistance;
            Console.WriteLine(temp);

            MIN = MIN < temp ? MIN : temp;
            temp = leftDistance > rightDistance ? leftDistance : rightDistance;
            MAX = MAX < temp ? temp : MAX;
        }
        */

    }
}
