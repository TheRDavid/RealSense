using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /*
     *@author David
     * 
     * Problem besteht noch bei weit offenen Mund, keine Unterscheidung mehr zwischen Runter und Hoch why is that german ? 

     * Lip Corner down does not work so far, landmarkpoints at the lip corner are not tracked when they go down,
     * no further sdk settings found, maybe recognize patterns via opencv?
     */
    class AU_LipLine : RSModule
    {
        // Variables for logic

        private double[] LipCorner = new double[2];
        private double leftDistance;
        private double rightDistance;
        private double leftLipCorner;
        private double rightLipCorner;

        // Variables for debugging

        // Default values
        public AU_LipLine()
        {
            debug = true;
        }

        public override void Work(Graphics g)
        {
            /* Calculations */

            double line = model.DifferenceByAxis(33, 44, Model.AXIS.Y, false);
            line += model.DifferenceByAxis(33, 43, Model.AXIS.Y, false);
            line += model.DifferenceByAxis(39, 41, Model.AXIS.Y, false);
            line += model.DifferenceByAxis(39, 40, Model.AXIS.Y, false);
            line *= 1000;

            // Update value in Model 
            model.setAU_Value(typeof(AU_LipLine).ToString() + "_line", line);

            // print debug-values 
            if (debug)
            {
                output = "LipLine: " + (int)line;
            }
        }
    }
}