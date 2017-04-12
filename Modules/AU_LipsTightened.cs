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

        bool b = false;
        public AU_LipsTightened()
        {
            debug = true;
        }

        public override void Work(Graphics g)
        {
            upperLip = (model.Difference(36, Model.NOSE_FIX) - 100);
            bottomLip = (model.Difference(50, Model.NOSE_FIX) - 100);

           topDownDistance = (upperLip + bottomLip) / 2;
            int d = Convert.ToInt32(topDownDistance);

            model.setAU_Value(typeof(AU_LipsTightened).ToString() + "_upperBottomLip", d);

            if (debug)
            {
              
                output = "Lips tightened: " + d;
                
            }
        }



    }
}
