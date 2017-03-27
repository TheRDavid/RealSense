using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /*
     *Measures if Brow is raised or lowered (each eye)  - Action Unit Number 4 
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
            debug = true;
        }

        public override void Work(Graphics g)
        {
            /* Calculations */

            // calculates the difference between the Nullface and the currentface -> to check if the whole eyebrow is raised or lowered
            leftEyeBrow_r = model.DifferenceNullCurrent(0, Model.AXIS.Y);
            leftEyeBrow_m = model.DifferenceNullCurrent(2, Model.AXIS.Y);
            leftEyeBrow_l = model.DifferenceNullCurrent(4, Model.AXIS.Y);

            rightEyeBrow_r = model.DifferenceNullCurrent(9, Model.AXIS.Y);
            rightEyeBrow_m = model.DifferenceNullCurrent(7, Model.AXIS.Y);
            rightEyeBrow_l = model.DifferenceNullCurrent(5, Model.AXIS.Y);

            leftEyeBrow_r = model.Difference(0, Model.NOSE_FIX) - 100;
            leftEyeBrow_m = model.Difference(2, Model.NOSE_FIX) - 100;
            leftEyeBrow_l = model.Difference(4, Model.NOSE_FIX) - 100;

            rightEyeBrow_r = model.Difference(9, Model.NOSE_FIX) - 100;
            rightEyeBrow_m = model.Difference(7, Model.NOSE_FIX) - 100;
            rightEyeBrow_l = model.Difference(5, Model.NOSE_FIX) - 100;


            leftDistance = ((leftEyeBrow_r + leftEyeBrow_m + leftEyeBrow_l) / 3)*1;
            rightDistance = ((rightEyeBrow_r + rightEyeBrow_m + rightEyeBrow_l) / 3)* 1;

            int d_l = Convert.ToInt32(leftDistance);
            int d_r = Convert.ToInt32(rightDistance);


            // Console.WriteLine(leftDistance + " , " + rightDistance);


            //here it gives back zero 
            // Update value in Model 
            model.setAU_Value(typeof(AU_BrowShift).ToString() + "_left", d_l);
            model.setAU_Value(typeof(AU_BrowShift).ToString() + "_right", d_r);


            // print debug-values 
            if (debug)
            {
                output = "BrowShift: " + "(" + d_l + ", " + d_r + ")";
            }
        }
    }
}
