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
 
        double chin;
      

        public AU_JawDrop()
        {
            DEF_MIN = null;
            DEF_MAX = 20;
            reset();
            MIN_TOL = -1.5;
            MAX_TOL = 1.5;
            debug = true;
            XTREME_MAX = 62;
            XTREME_MIN = null 
        }
        public override void Work(Graphics g)
        {
            /* calculations */

            //chin = model.DifferenceNullCurrent(61, Model.AXIS.Y);
            chin = (model.Difference(61, 26)) - 100;
            int d = Convert.ToInt32(chin);

            /* Update value in Model */
            model.setAU_Value(typeof(AU_JawDrop).ToString(), d);

            /* print debug-values */
            if (debug)
            {
                output = "jaw dropped: " + d;
            }
        }
    }
}

