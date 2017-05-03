using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense
{
    /*
     *Measures if complete brow is raised or lowered (each eye)  - Action Unit Number 4 
     *@author Anton 
     *@date 20.03.2017
     *@HogwartsHouse Slytherin  
     */
    class ME_InnerBrowShift : RSModule
    {
        private double left_dist = 0, right_dist = 0;

        public ME_InnerBrowShift()
        {
            debug = true;
        }
        public override void Work(Graphics g)
        {

            left_dist = model.Difference(0,4);
            left_dist += model.Difference(1, 4);
            left_dist += model.Difference(70, 4);

            right_dist = model.Difference(6, 9);
            right_dist += model.Difference(73, 9);
            right_dist += model.Difference(5, 9);

            int d_l = Convert.ToInt32(left_dist*1000) / 3000 - 100;
            int d_r = Convert.ToInt32(right_dist*1000) / 3000 - 100;

            model.setAU_Value(typeof(ME_InnerBrowShift).ToString() + "_left", d_l);
            model.setAU_Value(typeof(ME_InnerBrowShift).ToString() + "_right", d_r);

            // print debug-values 
            if (debug)
            {
                output = "InnerBrowShift: " + "(" + d_l + ", " + d_r + ")";
            }
        }
    }
}
