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
    class FaceTrackerModule_Tobi : RSModule
    {
        // Stuff for Facesrxdtd detection
        private PXCMFaceModule module;
        private PXCMFaceData data;

        private PXCMFaceConfiguration fg;
        // Pen which defines the appereance of the rect
        private Pen pen = new Pen(Color.Blue);

        public override void Init(PXCMSenseManager sManager)
        {
            senseManager = sManager;
            // Get a face instance here (or inside the AcquireFrame/ReleaseFrame loop) for configuration
            module = senseManager.QueryFace();
            // face is a PXCMFaceModule instance
            fg = module.CreateActiveConfiguration();


            // Set to enable all alerts
            fg.EnableAllAlerts();
            // Apply changes
            fg.ApplyChanges();
            //fg.Update();



        }
        public override void Work(Graphics g)
        {
            data = module.CreateOutput();
            data.Update();
            // Get the number of tracked faces
            Int32 nfaces = data.QueryNumberOfDetectedFaces();

            Console.WriteLine("Number of faces : " + nfaces);
            for (Int32 i = 0; i < nfaces; i++)
            {

                // all faces in the picture

                PXCMFaceData.Face face = data.QueryFaceByIndex(i);

                //face location 
                PXCMFaceData.DetectionData ddata = face.QueryDetection();

                // Retrieve the face landmark data instance

                PXCMFaceData.Face landmark = data.QueryFaceByIndex(i);
                PXCMFaceData.LandmarksData ldata = landmark.QueryLandmarks();



                // work on DetectionData

                PXCMRectI32 rect;
                ddata.QueryBoundingRect(out rect);

                //draw rect
                Rectangle rectangle = new Rectangle(rect.x, rect.y, rect.w, rect.h); // Convert to Rectangle
                g.DrawRectangle(pen, rectangle); // Draw




                // get the landmark data
                PXCMFaceData.LandmarkPoint[] points;
                ldata.QueryPoints(out points);





                //g.DrawImage(points[0].image,);

                for (Int32 j = 0; j < points.Length; j++)
                {
                    //Point p = new Point();
                    // p = points[0].ToString;
                    Point p = new Point();
                    p.X = (int)points[j].image.x;
                    p.Y = (int)points[j].image.y;

                    g.DrawEllipse(pen, points[j].image.x, points[j].image.y, 2, 2);


                }


            }
            data.Dispose();
        }
    }
}