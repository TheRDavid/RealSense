using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
/**
 * @author: Tobi
 * @author:
 */
namespace RealSense
{
    class LandmarkTypeModule : RSModule
    {

        // Pen which defines the appereance of the rect
        private Pen pen = new Pen(Color.Yellow);

        private Font font = new Font("Arial", 18);
        private SolidBrush stringBrush = new SolidBrush(Color.Red);
        private Rectangle stringRect = new Rectangle(30, 400, 420, 50);

        private bool guInit = false;
        private TrackBar selectionBar = new TrackBar();
        private int crossThreadValue = 0;

        private void guiInit()
        {
            guInit = true;
            selectionBar.Bounds = new Rectangle(50, 490, 500, 30);
            selectionBar.Maximum = 32;
            selectionBar.Minimum = 0;
            selectionBar.Value = 0;
            selectionBar.ValueChanged +=
                new System.EventHandler(UpdateValue);
            model.View.AddComponent(selectionBar);
        }

        private void UpdateValue(object sender, System.EventArgs e)
        {
            crossThreadValue = selectionBar.Value;
        }

        public override void Work(Graphics g)
        {
            if (!guInit)
                guiInit();
            if (model.FaceCurrent != null)
            {

                PXCMFaceData.LandmarksData lp = model.FaceCurrent.QueryLandmarks();
                if (lp == null)
                {
                    //Console.WriteLine("LandmarksData null, goddamnit!!");
                    g.DrawString("LandmarksData null, goddamnit!!", font, stringBrush, stringRect);
                    return;
                }
                PXCMFaceData.LandmarkPoint lPoint;
                if (((PXCMFaceData.LandmarkType)crossThreadValue) == PXCMFaceData.LandmarkType.LANDMARK_NOT_NAMED)
                {
                    for (int i = 1; i < 33; i++)
                    {

                        lp.QueryPoint(lp.QueryPointIndex((PXCMFaceData.LandmarkType)i), out lPoint);

                        Point p = new Point();

                        p.X = (int)lPoint.image.x;
                        p.Y = (int)lPoint.image.y;

                        g.DrawEllipse(pen, lPoint.image.x - 2, lPoint.image.y - 2, 4, 4);
                    }
                }
                else
                {
                    lp.QueryPoint(lp.QueryPointIndex((PXCMFaceData.LandmarkType)crossThreadValue), out lPoint);

                    Point p = new Point();

                    p.X = (int)lPoint.image.x;
                    p.Y = (int)lPoint.image.y;

                    g.DrawEllipse(pen, lPoint.image.x - 2, lPoint.image.y - 2, 4, 4);
                }

            }
            g.DrawString(((PXCMFaceData.LandmarkType)crossThreadValue).ToString(), font, stringBrush, stringRect);
        }

    }
}