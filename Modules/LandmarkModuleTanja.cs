using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RealSense
{
    class LandmarkModuleTanja : RSModule
    {
        // Pen which defines the appereance of the rect
        private Pen pen = new Pen(Color.Blue);

        public override void Work(Graphics g)
        {
            if (model.FaceAktuell != null)
            {
                // get the landmark data
                PXCMFaceData.LandmarksData ldata = model.FaceAktuell.QueryLandmarks();
                PXCMFaceData.LandmarkPoint[] points;
                if (ldata != null)
                {
                    ldata.QueryPoints(out points);

                    //Draw points
                    for (Int32 j = 0; j < points.Length; j++)
                    {
                        Point p = new Point();
                        p.X = (int)points[j].image.x;
                        p.Y = (int)points[j].image.y;

                        g.DrawEllipse(pen, points[j].image.x, points[j].image.y, 2, 2);
                    }
                }
                else
                {
                    Console.WriteLine("Landmarks konnten nicht erstellt werden");
                }
            }
        }
    }
}
