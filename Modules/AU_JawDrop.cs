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
            debug = true;
        }
        public override void Work(Graphics g)
        {
            /* calculations */

            chin = model.DifferenceNullCurrent(61, Model.AXIS.Y);
            
            /* Update value in Model */
            model.setAU_Value(typeof(AU_JawDrop).ToString(), chin);

            /* print debug-values */
            if (debug)
            {
                model.View.Debug_Y += 20; // new row
                g.DrawString("jaw dropped: " + chin, model.DefaultFont, model.DefaultStringBrush, new Point(0, model.View.Debug_Y));
            }
        }
    }
}

