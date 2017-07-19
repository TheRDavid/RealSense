using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense.Emotions
{
    class EM_Contempt : RSModule
    {

        private double[] distancesBrow = new double[numFramesBeforeAccept];
        private double[] distancesLip = new double[numFramesBeforeAccept];

        //double[] diffs = new double[] { 0, 0 };

        // Default values
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

        /*
         *  1 = inner brow raised -> BrowShift
            2 = outer brow raised -> BrowShift
            4 = brow lowered -> BrowShift
            5 = upper lid raised -> EyelidTight
            6 = cheeck raised -> CheeckRaised (not working)
            7 = lid tightened -> EyelidTight
            9 = nose wrinkled -> NoseWrinkled
            12 = lip corner pulled (up) -> LipCorner
            14 = grübchen -> none
            15 = lip corner lowered -> LipLine
            16 = lower lip lowered ->LowerLipLowered
            20 = lip stretched -> LipStretched
            23 = lip tightened -> LipsTightened
            26 = jaw drop -> JawDrap

            Verachtung (12 (R,L), 14(R,L)
            Trauer (1,4,15,(20?)
            Wut (4,5,6,23)
            Ekel (9,15,16,4)
            Überraschung (1,2,5B,26)
            Freude (6,12, 7)
            Angst (1,2,4,5,6,20,26)

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
                filterToleranceValues(distancesBrow);
                filterToleranceValues(distancesLip);

                double distanceBrow = filteredAvg(distancesBrow);
                double distanceLip = filteredAvg(distancesLip);

                dynamicMinMax(new double[] { distanceBrow });
                dynamicMinMax(new double[] { distanceLip });

                double[] diffs = convertValues(new double[] { distanceBrow, distanceLip });

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










