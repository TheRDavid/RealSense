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
            DEF_MAX = 0;
            reset();
            MIN_TOL = -1;
            MAX_TOL = 1;
            debug = true;
            XTREME_MAX = 0;
            XTREME_MIN = -16.5;
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
                for (int i = 0; i < topDownDistances.Length; i++)
                {
                    topDownDistances[i] = topDownDistances[i] < MAX_TOL && topDownDistances[i] > MIN_TOL ? 0 : topDownDistances[i];
                }

                double topDownDistance = filteredAvg(topDownDistances);
                double[] diffs = new double[] { topDownDistance };
                dynamicMinMax(diffs);
                diffs = convertValues(diffs);

                // Update value in Model 
                model.setAU_Value(typeof(ME_LipsTightened).ToString(), diffs[0]);
                if (debug)
                {
                    output = "Lips tightened: " + diffs[0];
                }
                framesGathered = 0;
            }


        }



    }
}
