using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /*
     * this works just perfect! 
     *@author Anton 
     *@date 20.02.2017
     *@HogwartsHouse Slytherin  
     */
    class AU_InnerBrowRaised : RSModule
    {
        // Variables for logic

        private double[] innerBrow = new double[2];
        private double[] checkOuterBrow = new Double[2];
        private double NullFace_y;
        private double NullFace_x;
        private double distance;

        // Variables for debugging

        // Default values
        public AU_InnerBrowRaised()
        {
            debug = true;
        }

        public override void Work(Graphics g)
        {
            /* Calculations */
            innerBrow[0] = model.Difference(0, 26);  //hinschreiben welche zeilöe das ist
            innerBrow[1] = model.Difference(5, 26);

            //beide punkte holen und y abfragen 
            //(abstand beim nullface erst berechnen, wenn aktueller dann groß davon abweicht ist nur das eine oben) -> sollte im kontruktor passieren 
            NullFace_y = model.NullFace[4].world.y;
            NullFace_x = model.NullFace[9].world.y;

            Console.WriteLine(NullFace_x + "," + NullFace_y);

            distance = innerBrow[0] + innerBrow[1];
            distance /= 2;
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

            model.setAU_Value(typeof(AU_InnerBrowRaised).ToString(), distance);

            if (debug)
            {
                model.View.Debug_Y += 20;
                g.DrawString("Innerbrow: " + distance, model.DefaultFont, model.DefaultStringBrush, new Point(0, model.View.Debug_Y));
            }
        }
    }
}
