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


    {
        double jawDistance;
        double chin;
        double referenceLM;

        public AU_JawDrop()
        {
            debug = true;
        }
        public override void Work(Graphics g)
        {
            /* calculations */

            chin = model.DifferenceNullCurrent(61, Model.AXIS.Y);
            referenceLM = model.DifferenceNullCurrent(26, Model.AXIS.Y);

            jawDistance = (chin + referenceLM) / 2;
            /* Update value in Model */
            model.setAU_Value(typeof(AU_TEMPLATE).ToString(), jawDistance);

            /* print debug-values */
            if (debug)
            {
                model.View.Debug_Y += 20; // new row
                g.DrawString("jaw dropped: " + jawDistance, model.DefaultFont, model.DefaultStringBrush, new Point(0, model.View.Debug_Y));
            }
        }
    }
}

