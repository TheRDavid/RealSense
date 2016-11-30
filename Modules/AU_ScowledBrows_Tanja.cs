using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RealSense
{
    class AU_ScowledBrows_Tanja : RSModule
    {
        private Font font = new Font("Arial", 18);
        private SolidBrush stringBrush = new SolidBrush(Color.Red);
        private int time = 40;
        private bool init = true;

        public override void Work(Graphics g)
        {
            try
            {
                if (time > 0)
                {
                    time--;
                    g.DrawString(time.ToString(), font, stringBrush, new PointF(20, 20));
                }
                else if (init)
                {
                    //model.calibrateFace();
                    //init = false;
                }
                else
                {
                    g.DrawString((model.normalFaceBetween(0,7) - model.between(0,7)) + " " + (model.normalFaceBetween(0, 29) - model.between(0, 29)), font, stringBrush, new PointF(20, 60));
                    if (model.normalFaceBetween(0,7) - model.between(0,7) >= 0.0007 && model.normalFaceBetween(0, 29) - model.between(0, 29) >= 0.001)
                    {
                        g.DrawString("scrowled Brows", font, stringBrush, new PointF(20, 50));
                    }
                }
            }catch (NullReferenceException e)
            {
                g.DrawString("No Face", font, stringBrush, new PointF(20, 20));
                return;
            }
        }
    }
}
