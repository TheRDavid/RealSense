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
    class AU_LipsTightened : RSModule
    {
        double topDownDistance;
        double upperLip;
        double bottomLip;
        public AU_LipsTightened()
        {
            debug = true;
        }

        public override void Work(Graphics g)
        {
            upperLip = model.DifferenceNullCurrent(36, Model.AXIS.Y);
            bottomLip = model.DifferenceNullCurrent(50, Model.AXIS.Y);

            topDownDistance = (upperLip + bottomLip) / 2;

            model.setAU_Value(typeof(AU_LipsTightened).ToString() + "_upperBottomLip", topDownDistance);

            if (debug)
            {
                model.View.Debug_Y += 20; // new row
                g.DrawString("lips tightened" + topDownDistance, model.DefaultFont, model.DefaultStringBrush, new Point(0, model.View.Debug_Y));
            }
        }


    }
}
