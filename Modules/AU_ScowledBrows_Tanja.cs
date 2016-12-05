using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RealSense
{
    class AU_ScowledBrows_Tanja : RSModule
    {
        private Font arialFont = new Font("Arial", 18);
        private Brush redBrush = new SolidBrush(Color.Red);

        public override void Work(Graphics g)
        {
            //g.DrawString(model.difference(0,29).ToString(), font, stringBrush, new PointF(20, 60));

            double augenbrauenAbstand = model.Difference(0, 5);
            double augenBraueNaseAbstand = model.Difference(0, 29);

            if (augenbrauenAbstand < 99 && augenBraueNaseAbstand < 90)
            {
                g.DrawString("scrowled Brows", arialFont, redBrush, new PointF(20, 30));
                model.Anger += 50;
            }                
        }
    }
}
