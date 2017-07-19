using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense
{

    /*
   *Measures if lips are tightened 
   *@author René 
   *@date 21.03.2017
   *@HogwartsHouse Slytherin 
     * 
     * Interpretation:      -100 = Tight af
     *                         0 = normal
     *                       100 = doesn't usually happen
   */
    class AU_LipsTightened : RSModule
    {
        double[] topDownDistances = new double[numFramesBeforeAccept];
        double upperLip;
        double bottomLip;

        public AU_LipsTightened()
        {
            //values correct
            DEF_MIN = -3;
            DEF_MAX = 1;
            reset();
            MIN_TOL = -1;
            MAX_TOL = 1;
            debug = true;
            XTREME_MAX = 0;
            XTREME_MIN = -16.5;
            model.AU_Values[typeof(AU_LipsTightened).ToString()] = 0;
        }

        public override void Work(Graphics g)
        {

            upperLip = (model.Difference(36, Model.NOSE_FIX) - 100);
            bottomLip = (model.Difference(50, Model.NOSE_FIX) - 100);

            double tdDist = (upperLip + bottomLip) / 2;
            if (framesGathered < numFramesBeforeAccept)
            {
                topDownDistances[framesGathered++] = tdDist;
            }
            else
            {
                filterToleranceValues(topDownDistances);

                double topDownDistance = filteredAvg(topDownDistances);
                double[] diffs = new double[] { topDownDistance };
                dynamicMinMax(diffs);
                diffs = convertValues(diffs);

                // Update value in Model 
                if (model.CurrentPoseDiff < model.PoseMax)
                    model.AU_Values[typeof(AU_LipsTightened).ToString()] = diffs[0];
                if (debug)
                {
                    output = "LipsTightened: " + "(" + (int)model.AU_Values[typeof(AU_LipsTightened).ToString()] + ")(" + (int)MIN + ", " + (int)MAX + ")";
                }
                framesGathered = 0;
            }


        }



    }
}
