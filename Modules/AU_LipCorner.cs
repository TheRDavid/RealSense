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
            LipCorner[0] = model.Difference(33, 26);  //left LipCorner
            LipCorner[1] = model.Difference(39, 26);  //right LipCorner

            // Update value in Model 
            model.setAU_Value(typeof(AU_LipCorner).ToString() + "_left", LipCorner[0]);
            model.setAU_Value(typeof(AU_LipCorner).ToString() + "_right", LipCorner[1]);

            if (LipCorner[0] < 98 && LipCorner[0] >95 && LipCorner[1] < 98 && LipCorner[1] >95)
            {
                Console.WriteLine("Normal Lächeln");
            } else if (LipCorner[0] < 95 && LipCorner[1] < 95)
            {
                Console.WriteLine("Voll Lächeln");
            } else if (LipCorner[0] < 98 && LipCorner[1] > 97|| LipCorner[1] < 98 && LipCorner[0] > 97)
            {
                Console.WriteLine("Halb Lächeln");
            } else if (LipCorner[0] > 103 && LipCorner[0] < 109 && LipCorner[1] > 103 && LipCorner[1] < 109)
            {
                Console.WriteLine("Normal Trauer");
            }
            else if (LipCorner[0] > 108 && LipCorner[1] > 108)
            {
                Console.WriteLine("Voll Trauer");
            }


            // print debug-values 
            if (debug)
            {
                model.View.Debug_Y += 20;
                g.DrawString("LipCorner: " + "(" + LipCorner[0] + ", " + LipCorner[1] + ")", model.DefaultFont, model.DefaultStringBrush, new Point(0, model.View.Debug_Y));
            }
        }
    }
}