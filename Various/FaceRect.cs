using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense
{
    /**
     * Draws a rectangle, sorrounding the subject's face
     * @author: David Rosenbusch
     * @HogwartsHouse Hufflepuff
     */
    class FaceRect : RSModule
    {
        /** 
         * @Override 
         * Draws a rectangle... wow
         * @param Graphics g for the view
         */
        public override void Work(Graphics g)
        {
            g.DrawRectangle(new Pen(model.DefaultStringBrush), new Rectangle(245, 100, 150, 250));
        }
    }
}
