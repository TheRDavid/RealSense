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
        double topDownDistance;
        double upperLip;
        double bottomLip;

        bool b = false;
        public ME_LipsTightened()
        {
            debug = true;
        }

        public override void Work(Graphics g)
        {
            upperLip = (model.Difference(36, Model.NOSE_FIX) - 100);
            bottomLip = (model.Difference(50, Model.NOSE_FIX) - 100);

            topDownDistance = (upperLip + bottomLip) / 2;

            topDownDistance = topDownDistance < MAX_TOL && topDownDistance > MIN_TOL ? 0 : topDownDistance;

            topDownDistance = filterExtremeValues(topDownDistance);

            dynamicMinMax(new double[] { topDownDistance });

            double[] diffs = convertValues(new double[] { topDownDistance });

            // Update value in Model 
            model.setAU_Value(typeof(ME_LipsTightened).ToString() + "_upperBottomLip", diffs[0]);

            if (debug)
            {
              
                output = "Lips tightened: " + diffs[0];
                
            }
        }



    }
}
