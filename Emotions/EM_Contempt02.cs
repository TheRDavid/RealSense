using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense.Emotions
{
    /*
     *Measures the percentage value of contempt. 
     *@author Tanja 
     */
    class EM_Contempt : RSModule
    {
        // Variables for logic
        int percent = 100;
        private double[] distancesBrow = new double[numFramesBeforeAccept];
        private double[] distancesLip = new double[numFramesBeforeAccept];

        /**
         * Initializes the EM, setting the debug-flag to true by default
         * Also sets up a default boundary of max-, min, extreme- and tolerance-values
         */
        public EM_Contempt()
        {
            debug = true;
            DEF_MIN = -1;
            DEF_MAX = 5;
            reset();
            MIN_TOL = -1;
            MAX_TOL = 1;
            XTREME_MAX = 30;
            XTREME_MIN = -1;
        }

        /**
         * Computes the percentage Value of Anger in the current Frame.
         * @param Graphics g for the view
         * */
        public override void Work(Graphics g)
        {
            //proportions Contempt
            int p_brow = 70;
            int p_lip = 50;

            //reduce();

            //Diffs
            double brow_diff = Utilities.DifferenceByAxis(2, 7, Model.AXIS.Y, false);
            brow_diff = Math.Abs(brow_diff * 1000);
            double lip_diff = Utilities.DifferenceByAxis(33, 39, Model.AXIS.Y, false);
            lip_diff = Math.Abs(lip_diff * 1000);

            if (framesGathered < numFramesBeforeAccept)
            {
                distancesBrow[framesGathered] = brow_diff;
                distancesLip[framesGathered++] = lip_diff;
            }
            else
            {
                // Update value in Model 
                if (model.CurrentPoseDiff < model.PoseMax)
                {
                    double brow_Value = Utilities.ConvertValue(distancesBrow, MAX, MIN, MAX_TOL, MIN_TOL, XTREME_MAX, XTREME_MIN) * p_brow / percent;
                    double lip_Value = Utilities.ConvertValue(distancesLip, MAX, MIN, MAX_TOL, MIN_TOL, XTREME_MAX, XTREME_MIN) * p_lip / percent;
                    double contempt = brow_Value + lip_Value;
                    model.Emotions[Model.Emotion.CONTEMPT] = contempt;
                }

                // print debug-values 
                if (debug)
                {

                    output = "Contempt02: ";

                }

                framesGathered = 0;
            }
        }
    }
}










