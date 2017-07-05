using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /*
     * Measures if complete brow is raised or lowered (each eye)  - Action Unit Number 4 
     * @author Anton 
     * @date 20.03.2017
     * @HogwartsHouse Slytherin  
     * 
     * Interpretation:      -100 = Brows down (grumpy af)
     *                       100 = Brows up
     */
    class ME_BrowShift : RSModule
    {
        // Variables for logic
        private double leftEyeBrow_r, leftEyeBrow_m, leftEyeBrow_l;
        private double rightEyeBrow_r, rightEyeBrow_m, rightEyeBrow_l;
        private double[] leftDistances = new double[numFramesBeforeAccept];
        private double[] rightDistances = new double[numFramesBeforeAccept];

        // Default values
        public ME_BrowShift()
        {
            DEF_MIN = -7;
            DEF_MAX = 12;
            reset();
            MIN_TOL = -2;
            MAX_TOL = 1;
            debug = false;
            XTREME_MAX = 33;
            XTREME_MIN = -24;


            model.AU_Values[typeof(ME_BrowShift).ToString() + "_left"] = 0;
            model.AU_Values[typeof(ME_BrowShift).ToString() + "_right"] = 0;
        }

        /**
         * @Override 
         * Computes the percentage Value of BrowShift in the current Frame.
         * @param Graphics g for the view
         * */
        public override void Work(Graphics g)
        {
            //Get Values from AU's
            leftEyeBrow_r = Utilities.Difference(0, Model.NOSE_FIX) - 100;
            leftEyeBrow_m = Utilities.Difference(2, Model.NOSE_FIX) - 100;
            leftEyeBrow_l = Utilities.Difference(4, Model.NOSE_FIX) - 100;

            rightEyeBrow_r = Utilities.Difference(9, Model.NOSE_FIX) - 100;
            rightEyeBrow_m = Utilities.Difference(7, Model.NOSE_FIX) - 100;
            rightEyeBrow_l = Utilities.Difference(5, Model.NOSE_FIX) - 100;

            //Average
            double ld = ((leftEyeBrow_r + leftEyeBrow_m + leftEyeBrow_l) / 3);
            double rd = ((rightEyeBrow_r + rightEyeBrow_m + rightEyeBrow_l) / 3);

            //Gather Frames
            if (framesGathered < numFramesBeforeAccept)
            {
                leftDistances[framesGathered] = ld;
                rightDistances[framesGathered++] = rd;
            }
            else
            {
                if (model.CurrentPoseDiff < model.PoseMax)
                {
                    //set Values
                    model.AU_Values[typeof(ME_BrowShift).ToString() + "_left"] = Utilities.ConvertValue(leftDistances, MAX, MIN, MAX_TOL, MIN_TOL, XTREME_MAX, XTREME_MIN);
                    model.AU_Values[typeof(ME_BrowShift).ToString() + "_right"] = Utilities.ConvertValue(rightDistances, MAX, MIN, MAX_TOL, MIN_TOL, XTREME_MAX, XTREME_MIN);
                }
            }

            // print debug-values 
            if (debug)
            {
                output = "BrowShift: " + "(" + (int)model.AU_Values[typeof(ME_BrowShift).ToString() + "_left"] + ", " + (int)model.AU_Values[typeof(ME_BrowShift).ToString() + "_right"] + ")(" + (int)MIN + ", " + (int)MAX + ")";
            }

            framesGathered = 0;
        }
    }

}

