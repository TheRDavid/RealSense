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
    class FaceTrackerModule : RSModule
    {

        // Pen which defines the appereance of the rect
        private Pen pen = new Pen(Color.Blue);
        private Font font = new Font("Arial", 6);
        private SolidBrush stringBrush = new SolidBrush(Color.Red);
        private int[] idx = null;

        public FaceTrackerModule(int[] idx)
        {
            this.idx = idx;
        }

        public override void Work(Graphics g)
        {
            if (model.FaceAktuell != null)
            {
                // get the landmark data
                PXCMFaceData.LandmarksData ldata = model.FaceAktuell.QueryLandmarks();
                PXCMFaceData.LandmarkPoint[] points;
                
                ldata.QueryPoints(out points);

                if(idx == null)

                //Draw points
                for (Int32 j = 0; j < points.Length; j++)
                {
                    Point p = new Point();
                    p.X = (int)points[j].image.x;
                    p.Y = (int)points[j].image.y;
                    
                    g.DrawString("" + j, font, stringBrush, new PointF(p.X, p.Y));


                    g.DrawEllipse(pen, points[j].image.x, points[j].image.y, 2, 2);
                }

                else
                foreach(int i in idx)
                {
                    Point p = new Point();
                    p.X = (int)points[i].image.x;
                    p.Y = (int)points[i].image.y;

                    g.DrawString("" + i, font, stringBrush, new PointF(p.X, p.Y));


                    g.DrawEllipse(pen, points[i].image.x, points[i].image.y, 2, 2);
                }

            }
        }
    }
}