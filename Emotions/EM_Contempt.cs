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

        private double[] distancesBrow = new double[numFramesBeforeAccept];
        private double[] distancesLip = new double[numFramesBeforeAccept];

        //double[] diffs = new double[] { 0, 0 };

        // Default values

        /**
    * Initializes the EM, setting the debug-flag to true by default
    * Also sets up a default boundary of max-, min, extreme- and tolerance-values
    */
        public EM_Contempt()
        {
            debug = true;
            DEF_MIN = -1;
            DEF_MAX = 5;
            Reset();
            MIN_TOL = -1;
            MAX_TOL = 1;
            XTREME_MAX = 30;
            XTREME_MIN = -1;
        }

        /**
       * Computes the percentage Value of contempt in the current Frame.
       * @param Graphics g for the view
       * */
        public override void Work(Graphics g)
        {
            //Contempt02 --> BrowShift, LipCorner

            //percentage Contempt
            int p_brow = 70;
            int p_lip = 50;

            //Diffs
            double brow_diff = model.DifferenceByAxis(2, 7, Model.AXIS.Y, false);
            brow_diff = Math.Abs(brow_diff * 1000);
            double lip_diff = model.DifferenceByAxis(33, 39, Model.AXIS.Y, false);
            lip_diff = Math.Abs(lip_diff * 1000);

            if (framesGathered < numFramesBeforeAccept)
            {
                distancesBrow[framesGathered] = brow_diff;
                distancesLip[framesGathered++] = lip_diff;
            }
            else
            {
                FilterToleranceValues(distancesBrow);
                FilterToleranceValues(distancesLip);

                double distanceBrow = FilteredAvg(distancesBrow);
                double distanceLip = FilteredAvg(distancesLip);

                DynamicMinMax(new double[] { distanceBrow });
                DynamicMinMax(new double[] { distanceLip });

                double[] diffs = ConvertValues(new double[] { distanceBrow, distanceLip });

                double brow_Value = diffs[0] * p_brow / 100;
                double lip_Value = diffs[1] * p_lip / 100;

                double contempt = brow_Value + distanceLip;

                // Update value in Model 

                if (model.CurrentPoseDiff < model.PoseMax)
                {
                    model.Emotions[Model.Emotion.CONTEMPT] = contempt;
                }

                // print debug-values 

                if (debug)
                {
                    
                    output = "Contempt02: " + contempt;

                }

                framesGathered = 0;
            }
        }
    }
}










