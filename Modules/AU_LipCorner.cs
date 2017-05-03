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
     * Problem besteht noch bei weit offenen Mund, keine Unterscheidung mehr zwischen Runter und Hoch why is that german ? 

     * Lip Corner down does not work so far, landmarkpoints at the lip corner are not tracked when they go down,
     * no further sdk settings found, maybe recognize patterns via opencv?
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
            DEF_MIN = null;
            DEF_MAX = 5;
            reset();
            MIN_TOL = -1;
            MAX_TOL = 0.5;
            debug = true;
            XTREME_MAX = 45;
            XTREME_MIN = -36;
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



            // print debug-values 
            if (debug)
            {
                output = "LipCorner: "  + d_l + ", " + d_r ;
            }
        }
    }
}