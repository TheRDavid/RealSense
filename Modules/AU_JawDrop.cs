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
    class AU_JawDrop : RSModule
        // our huffelpuff actually ravenclaw nerd wants a note : when changing face position values change as well due to a new angle difference should not be big enough to falsify 

    {
 
        double chin_dist;
      

        public AU_JawDrop()
        {
            DEF_MIN = 0;
            DEF_MAX = 20;
            reset();
            MIN_TOL = -1.5;
            MAX_TOL = 1.5;
            debug = true;
            XTREME_MAX = 62;
            XTREME_MIN = 0 
        }
        public override void Work(Graphics g)
        {
            /* calculations */

            //chin = model.DifferenceNullCurrent(61, Model.AXIS.Y);
            chin_dist = (model.Difference(61, 26)) - 100;

            chin_dist = chin_dist < MAX_TOL && chin_dist > MIN_TOL ? 0 : chin_dist;

            chin_dist = filterExtremeValues(chin_dist);

            dynamicMinMax(new double[] { chin_dist });

            double[] diffs = convertValues(new double[] { chin_dist });

            /* Update value in Model */
            model.setAU_Value(typeof(AU_JawDrop).ToString(), diffs[0]);

            /* print debug-values */
            if (debug)
            {
                output = "jaw dropped: " + diffs[0];
            }
        }
    }
}

