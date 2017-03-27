using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense.Modules
{
    class FaceRect : RSModule
    {
        public override void Work(Graphics g)
        {
            g.DrawRectangle(new Pen(model.DefaultStringBrush), new Rectangle(245, 100, 150, 250));
        }
    }
}
