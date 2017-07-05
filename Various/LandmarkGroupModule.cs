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
    class LandmarkGroupModule : RSModule
    {

        // Pen which defines the appereance of the rect
        private Pen[] pens = { new Pen(Color.Blue), new Pen(Color.Red), new Pen(Color.Yellow),
        new Pen(Color.Green),new Pen(Color.Black),new Pen(Color.Pink),new Pen(Color.White)};

        private PXCMFaceData.LandmarksGroupType[] landmarkGroupTypes =
            {PXCMFaceData.LandmarksGroupType.LANDMARK_GROUP_JAW,
        PXCMFaceData.LandmarksGroupType.LANDMARK_GROUP_LEFT_EYE,
        PXCMFaceData.LandmarksGroupType.LANDMARK_GROUP_LEFT_EYEBROW,
        PXCMFaceData.LandmarksGroupType.LANDMARK_GROUP_MOUTH,
        PXCMFaceData.LandmarksGroupType.LANDMARK_GROUP_NOSE,
        PXCMFaceData.LandmarksGroupType.LANDMARK_GROUP_RIGHT_EYE,
        PXCMFaceData.LandmarksGroupType.LANDMARK_GROUP_RIGHT_EYEBROW};
        Font errorFont = new Font("Arial", 16);
        SolidBrush errorBrush = new SolidBrush(Color.Black);
        Rectangle errorRect = new Rectangle(150, 150, 200, 50);



        public override void Work(Graphics g)
        {
            if (model.FaceCurrent != null)
            {
                PXCMFaceData.LandmarkPoint[] points;

                for (int i = 0; i < landmarkGroupTypes.Length; i++)
                {
                    PXCMFaceData.LandmarksData lp = model.FaceCurrent.QueryLandmarks();
                    if (lp == null)
                    {
                        //    Console.WriteLine("LandmarksData null, goddamnit!!");
                        g.DrawString("LandmarksData null, goddamnit!!", errorFont, errorBrush, errorRect);
                        break;
                    }
                    lp.QueryPointsByGroup(landmarkGroupTypes[i], out points);
                    //Draw points
                    for (Int32 j = 0; j < points.Length; j++)
                    {
                        Point p = new Point();

                        p.X = (int)points[j].image.x;
                        p.Y = (int)points[j].image.y;

                        g.DrawEllipse(pens[i], points[j].image.x, points[j].image.y, 3, 3);
                    }
                }
            }
        }
    }
}