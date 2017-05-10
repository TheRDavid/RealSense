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
    class ME_LipsTightened : RSModule
    {
        double[] topDownDistances = new double[numFramesBeforeAccept];
        double upperLip;
        double bottomLip;

        public ME_LipsTightened()
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
            model.AU_Values[typeof(ME_LipsTightened).ToString()] = 0;
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
                Console.WriteLine("tddist1: " + topDownDistance);
                double[] diffs = new double[] { topDownDistance };
                dynamicMinMax(diffs);
                Console.WriteLine("tddist2: " + diffs[0]);
                diffs = convertValues(diffs);
                Console.WriteLine("tddist3: " + diffs[0]);

                // Update value in Model 
                model.AU_Values[typeof(ME_LipsTightened).ToString()] = diffs[0];
                if (debug)
                {
                    output = "LipsTightened: " + "(" + (int)diffs[0] + ")(" + (int)MIN + ", " + (int)MAX + ")";
                }
                framesGathered = 0;
            }


        }



    }
}
