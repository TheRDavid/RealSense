using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /* Lip tightened */
    class AU_TEMPLATE : RSModule
    {

        // Variables for logic

        private double[] lipUp = new double[6];
        private double[] lipLow = new double[6];
        private double distance;

        // Variables for debugging

        // Default values
        public AU_TEMPLATE()
        {
            debug = true;
        }

        public override void Work(Graphics g)
        {
            /* Calculations */
            lipUp[0] = model.Difference(35, 46);
            lipUp[1] = model.Difference(36, 47);
            lipUp[2] = model.Difference(37, 48);

            lipLow[0] = model.Difference(52, 43);
            lipLow[1] = model.Difference(51, 42);
            lipLow[2] = model.Difference(50, 41);

            distance = lipUp[0] + lipUp[1] + lipUp[2] + lipLow[0] + lipLow[1] + lipLow[2];
            distance /= 6;
            // 100 = 0
            // < 100 = negativ
            // > 100 = positiv
            distance -= 100;

            /**
             * Several types of action units
             * 1: [   0 - 100] -> action unit is either activated (>0) or not (==0)
             * 2: [-100 - 100] -> action unit is always activated, either positively (>=0) or negatively (>0)
             * Values can go above 100 and below -100, 100 is just a value to orient to
             **/

            model.setAU_Value(typeof(AU_TEMPLATE).ToString(), distance);

            if (debug)
            {
                model.View.Debug_Y += 20;
                g.DrawString("LipsThickness: " + distance, model.DefaultFont, model.DefaultStringBrush, new Point(0, model.View.Debug_Y));
            }
        }
    }
}
