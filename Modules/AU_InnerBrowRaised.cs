using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /*
     *Measuresif innerBrow is raised or lowered (each eye)
     *@author Anton 
     *@date 20.02.2017
     *@HogwartsHouse Slytherin  
     */
    class AU_InnerBrowRaised : RSModule
    {
        // Variables for logic

        private double[] innerBrow = new double[2];
        private double[] checkOuterBrow = new Double[2];
        private double leftDistance;
        private double rightDistance;
        private double leftEyeBrow;
        private double rightEyeBrow;

        // Variables for debugging

        // Default values
        public AU_InnerBrowRaised()
        {
            debug = true;
        }

        public override void Work(Graphics g)
        {
            /* Calculations */

            // calculates difference between nose and eybrow 
            innerBrow[0] = model.Difference(0, 26);  //left eyebrow
            innerBrow[1] = model.Difference(5, 26);  //right eyebrow 

            // calculates the difference between the Nullface and the currentface -> to check if the whole eyebrow is raised or lowered
            leftEyeBrow = model.DifferenceNullCurrent(9, Model.AXIS.Y);
            rightEyeBrow = model.DifferenceNullCurrent(4, Model.AXIS.Y);
            Console.WriteLine(leftEyeBrow + " , " + rightEyeBrow);

            if (leftEyeBrow < -0.004 && rightEyeBrow < -0.004)
            {
                Console.WriteLine("eyebrows up");
            }
            else if (leftEyeBrow > 0.003 && rightEyeBrow > 0.003)
            {
                Console.WriteLine("eyebrows down");
            }
            else
            {
                Console.WriteLine("eyebrows hoold");
                // 100 = 0
                // < 100 = negativ
                // > 100 = positiv
                // supoosed to calculate - 100 to get to 0 
                leftDistance = innerBrow[0];
                rightDistance = innerBrow[1];

            }


            // Update value in Model 
            model.setAU_Value(typeof(AU_InnerBrowRaised).ToString() + "_left", leftDistance);
            model.setAU_Value(typeof(AU_InnerBrowRaised).ToString() + "_right", rightDistance);


            // print debug-values 
            if (debug)
            {
                model.View.Debug_Y += 20;
                g.DrawString("Innerbrow: " + "(" + leftDistance + ", " + rightDistance + ")", model.DefaultFont, model.DefaultStringBrush, new Point(0, model.View.Debug_Y));
            }
        }
    }
}
