using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense
{

    /*
    *Measures if jaw is dropped
    *@author René 
    *@date 21.03.2017
    *@HogwartsHouse Slytherin
    */
    class ME_JawDrop : RSModule
    // our huffelpuff actually ravenclaw nerd wants a note : when changing face position values change as well due to a new angle difference should not be big enough to falsify 

    {

        double chin_dist;
        private double[] chinDistances = new double[numFramesBeforeAccept];
        private string debug_message = "JawDrop: ";

        public ME_JawDrop()
        {
            DEF_MIN = 0;
            DEF_MAX = 20;
            reset();
            MIN_TOL = -1.5;
            MAX_TOL = 1.5;
            debug = true;
            XTREME_MAX = 62;
            XTREME_MIN = 0;
        }
        public override void Work(Graphics g)
        {
            /* calculations */
            
            chin_dist = (model.Difference(61, 26)) - 100;
  

            if (framesGathered < numFramesBeforeAccept)
            {
                chinDistances[framesGathered++] = chin_dist;
            }
            else
            {
                for (int i = 0; i < numFramesBeforeAccept; i++)
                {
                    chinDistances[i] = chinDistances[i] < MAX_TOL && chinDistances[i] > MIN_TOL ? 0 : chinDistances[i];
                }

                double distance = filteredAvg(chinDistances);

                dynamicMinMax(new double[] { distance });

                double[] diffs = convertValues(new double[] { distance });

                /* Update value in Model */
                model.setAU_Value(typeof(ME_JawDrop).ToString(), diffs[0]);

                /* print debug-values */
                if (debug)
                {
                    output = debug_message + "(" + diffs[0] + ")";
                }
                framesGathered = 0;
            }
        }
    }
}

