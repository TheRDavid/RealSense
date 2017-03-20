using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /*
     *Measures if Brow is raised or lowered (each eye)  
     *@author Anton 
     *@date 20.03.2017
     *@HogwartsHouse Slytherin  
     */
    class AU_BrowLowered : RSModule
    {
        // Variables for logic

       
        private double leftDistance;
        private double rightDistance;
        private double leftEyeBrow_r, leftEyeBrow_m, leftEyeBrow_l;
        private double rightEyeBrow_r, rightEyeBrow_m, rightEyeBrow_l;

        // Variables for debugging

        // Default values
        public AU_BrowLowered()
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


            leftDistance = (leftEyeBrow_r + leftEyeBrow_m + leftEyeBrow_l) / 3;
            rightDistance = (rightEyeBrow_r + rightEyeBrow_m + rightEyeBrow_l) / 3;


            Console.WriteLine(leftDistance + " , " + rightDistance);

         
            //here it gives back zero 
            // Update value in Model 
            model.setAU_Value(typeof(AU_InnerBrowRaised).ToString() + "_left", leftDistance);
            model.setAU_Value(typeof(AU_InnerBrowRaised).ToString() + "_right", rightDistance);


            // print debug-values 
            if (debug)
            {
                model.View.Debug_Y += 20;
                g.DrawString("Eyebrow: " + "(" + leftDistance + ", " + rightDistance + ")", model.DefaultFont, model.DefaultStringBrush, new Point(0, model.View.Debug_Y));
            }
        }
    }
}
