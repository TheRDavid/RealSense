using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    class TEMPLATE_AU_LipsThicknessModul : RSModule
    {

        // Variables for logic

        private double[] lipUp = new double[6];
        private double[] lipLow = new double[6];
        private double distance;
        private Dictionary<double, int> grades = new Dictionary<double, int>(); // Key = distance, Value = change of emotion... value (duh)

        // Variables for debug

        // Default values
        public TEMPLATE_AU_LipsThicknessModul()
        {
            grades.Add(92, 30);
            grades.Add(96, 20);
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


            int inc = 0;

            foreach (KeyValuePair<double, int> entry in grades)
            {
                if (distance <= entry.Key)
                {
                    inc += entry.Value;
                    break;
                }
            }

            model.Emotions[Model.ANGER] += inc;

            if (debug)
            {
                model.View.Debug_Y += 20;
                g.DrawString("LipsThickness increase by " + inc + " -> " + distance, model.DefaultFont, model.DefaultStringBrush, new Point(0, model.View.Debug_Y));
            }
        }
    }
}
