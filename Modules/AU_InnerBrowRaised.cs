using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /*
     * this works just perfect! 
     *@author Anton 
     *@date 20.02.2017
     *@HogwartsHouse Slytherin  
     */
    class AU_InnerBrowRaised : RSModule
    {
        // Variables for logic

        private double[] innerBrow = new double[2];
        private double[] checkOuterBrow = new Double[2];
        private double distance;
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


            leftEyeBrow = model.DifferenceNullCurrent(9, Model.AXIS.Y);
            rightEyeBrow =  model.DifferenceNullCurrent(4, Model.AXIS.Y);
            Console.WriteLine(leftEyeBrow + " , " + rightEyeBrow);


            // because it is never just zero i cant do it like that. 
            // so i need to find a better way
            if(leftEyeBrow < 0 && rightEyeBrow < 0 )
            {
                Console.WriteLine("eyebrows up");
            }else if(leftEyeBrow >0 && rightEyeBrow >0)
            {
                Console.WriteLine("eyebrows down");
            }else
            {
                Console.WriteLine("eyebrows hoold");
                //ónly here it is supposed to say if the inner brow is raised or lowered 

            }

            


            distance = innerBrow[0] + innerBrow[1];
            distance /= 2;
            // 100 = 0
            // < 100 = negativ
            // > 100 = positiv
            distance -= 100;

            model.setAU_Value(typeof(AU_InnerBrowRaised).ToString(), distance);

            if (debug)
            {
                model.View.Debug_Y += 20;
                g.DrawString("Innerbrow: " + distance, model.DefaultFont, model.DefaultStringBrush, new Point(0, model.View.Debug_Y));
            }
        }
    }
}
