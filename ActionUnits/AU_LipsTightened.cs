﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense
{

    /*
     * Measures if lips are tightened 
     * @author René 
     * @date 21.03.2017
     * @HogwartsHouse Slytherin 
     * 
     * Interpretation:      -100 = Tight af
     *                         0 = normal
     *                       100 = doesn't usually happen
   */
    class ME_LipsTightened : RSModule
    {
        // Variables for logic
        double[] topDownDistances = new double[numFramesBeforeAccept];
        double upperLip;
        double bottomLip;

        // Default values
        public ME_LipsTightened()
        {

            DEF_MIN = -3;
            DEF_MAX = 1;
            reset();
            MIN_TOL = -1;
            MAX_TOL = 1;
            debug = true;
            XTREME_MAX = 0;
            XTREME_MIN = -16.5;
            model.AU_Values[typeof(ME_LipsTightened).ToString()] = 0;
        }

        /** 
         * @Override 
         * Calculates difference of the lip and the nose.
         * @param Graphics g for the view
         */
        public override void Work(Graphics g)
        {
            //Get Values from AU's
            upperLip = (Utilities.Difference(36, Model.NOSE_FIX) - 100);
            bottomLip = (Utilities.Difference(50, Model.NOSE_FIX) - 100);

            double tdDist = (upperLip + bottomLip) / 2;

            //Gather Frames
            if (framesGathered < numFramesBeforeAccept)
            {
                topDownDistances[framesGathered++] = tdDist;
            }
            else
            {
                // Update value in Model 
                if (model.CurrentPoseDiff < model.PoseMax)
                    model.AU_Values[typeof(ME_LipsTightened).ToString()] = Utilities.ConvertValue(topDownDistances, MAX, MIN, MAX_TOL, MIN_TOL, XTREME_MAX, XTREME_MIN);
                if (debug)
                {
                    output = "LipsTightened: " + "(" + (int)model.AU_Values[typeof(ME_LipsTightened).ToString()] + ")(" + (int)MIN + ", " + (int)MAX + ")";
                }
                framesGathered = 0;
            }


        }



    }
}