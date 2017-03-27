using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /*
     *@author Marlon
     * 
     * Problem besteht noch bei weit offenen Mund, keine Unterscheidung mehr zwischen Runter und Hoch
     * 
     */
    class AU_LipCorner : RSModule
    {
        // Variables for logic

        private double[] LipCorner = new double[2];
        private double leftDistance;
        private double rightDistance;
        private double leftLipCorner;
        private double rightLipCorner;

        // Variables for debugging

        // Default values
        public AU_LipCorner()
        {
            debug = true;
        }

        public override void Work(Graphics g)
        {
            /* Calculations */

            // calculates difference between nose and LipCorner 
            LipCorner[0] = -((model.Difference(33, 26)) - 100);  //left LipCorner
            LipCorner[1] = -((model.Difference(39, 26)) - 100);  //right LipCorner

            int d_l = Convert.ToInt32(LipCorner[0]);
            int d_r = Convert.ToInt32(LipCorner[1]);

            // Update value in Model 
            model.setAU_Value(typeof(AU_LipCorner).ToString() + "_left", d_l);
            model.setAU_Value(typeof(AU_LipCorner).ToString() + "_right", d_r);

            if (LipCorner[0] < 98 && LipCorner[0] >95 && LipCorner[1] < 98 && LipCorner[1] >95)
            {
               // Console.WriteLine("Normal Laecheln");
            } else if (LipCorner[0] < 95 && LipCorner[1] < 95)
            {
               // Console.WriteLine("Voll Laecheln");
            } else if (LipCorner[0] < 98 && LipCorner[1] > 97|| LipCorner[1] < 98 && LipCorner[0] > 97)
            {
            //    Console.WriteLine("Halb Laecheln");
            } else if (LipCorner[0] > 103 && LipCorner[0] < 109 && LipCorner[1] > 103 && LipCorner[1] < 109)
            {
              //  Console.WriteLine("Normal Trauer");
            }
            else if (LipCorner[0] > 108 && LipCorner[1] > 108)
            {
               // Console.WriteLine("Voll Trauer");
            }


            // print debug-values 
            if (debug)
            {
                output = "LipCorner: "  + d_l + ", " + d_r ;
            }
        }
    }
}