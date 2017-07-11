using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
/**
 * @author: Tobi
 * @author:
 */
namespace RealSense
{
    /**
     * Displays the landmark-points of the current subject within the UI
     * @author: David Rosenbusch
     * @HogwartsHouse Hufflepuff
     */ 
    class FaceTrackerModule : RSModule
    {
        // Pen which defines the appereance of the rect
        private Pen pen = new Pen(Color.Blue);
        private Font font = new Font("Arial", 9);
        private SolidBrush stringBrush = new SolidBrush(Color.Red);
        private int[] idx = null;

        /**
         * Specifies which indexes to display
         */ 
        public FaceTrackerModule(int[] idx)
        {
            this.idx = idx;
        }

        /**
         * Displays landmark-points (all if none were specified)
         * @param Graphics g for drawing inside the containing UI
         */ 
        public override void Work(Graphics g)
        {
            if (model.FaceCurrent != null)
            {
                // get the landmark data

                if (model.Lp == null) return;
                PXCMFaceData.LandmarkPoint[] points = model.CurrentFace;

                if (idx == null)

                    //Draw points
                    for (Int32 j = 0; j < points.Length; j++)
                    {
                        Point p = new Point();
                        p.X = (int)points[j].image.x;
                        p.Y = (int)points[j].image.y;

                        g.DrawString("" + j, font, stringBrush, new PointF(p.X, p.Y));


                        g.DrawEllipse(pen, points[j].image.x, points[j].image.y, 4, 4);
                    }

                else
                    foreach (int i in idx)
                    {
                        Point p = new Point();
                        p.X = (int)points[i].image.x;
                        p.Y = (int)points[i].image.y;

                        g.DrawString("" + i, font, stringBrush, new PointF(p.X, p.Y));


                        g.DrawEllipse(pen, points[i].image.x, points[i].image.y, 4, 4);
                    }

            }
        }
    }
}