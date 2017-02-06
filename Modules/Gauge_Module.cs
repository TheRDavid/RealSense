using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RealSense
{
    public class Gauge_Module : RSModule
    {
        private const int numFaces = 44;
        private bool calibrate = false;
        private bool guInit = false;
        private PXCMFaceData.LandmarkPoint[][] cFaces = new PXCMFaceData.LandmarkPoint[numFaces][];
        private PXCMFaceData.LandmarkPoint[] finalFace = new PXCMFaceData.LandmarkPoint[80];
        private int index = 0;
        private Button calibrateButton = new Button();
        public bool frameUpdate = true;

        /**
         * Inits the GUI
         */
        private void guiInit()
        {
            guInit = true;
            calibrateButton.Bounds = new Rectangle(20, 530, 500, 30);
            calibrateButton.Text = "Calibrate";
            calibrateButton.Click +=
                new System.EventHandler(delegate
                {
                    calibrate = true;
                });
            model.View.AddComponent(calibrateButton);
            for (int i = 0; i < finalFace.Length; i++)
                finalFace[i] = new PXCMFaceData.LandmarkPoint();

            Thread updaterThread = new Thread(this.update);
            updaterThread.Start();
            debug = true;
        }

        /**
         * when a new frame is shot AND still calibrating (button was pressed), adds a new face to the average.
         * After having numFaces faces, calculate...
         * Runs outside the Camera Thread
         */
        public void update()
        {
            while (!model.View.IsDisposed)
            {
                if (calibrate)
                {
                    calibrate = index != numFaces;
                    if (frameUpdate)
                        if (model.FaceAktuell != null)
                        {
                            frameUpdate = false;
                            // get the landmark data

                            if (model.Lp == null) return;
                            Console.WriteLine("Calibrate #" + index);
                            cFaces[index++] = model.CurrentFace;
                        }
                    if (!calibrate)
                    {
                        int landMarkNr = 0, faceNr = 0;
                        for (; landMarkNr < cFaces[faceNr].Length; landMarkNr++)
                        {
                            for (faceNr = 0; faceNr < cFaces.Length - 1; faceNr++)
                            {
                                finalFace[landMarkNr].world.x += cFaces[faceNr][landMarkNr].world.x;
                                finalFace[landMarkNr].world.y += cFaces[faceNr][landMarkNr].world.y;
                                finalFace[landMarkNr].world.z += cFaces[faceNr][landMarkNr].world.z;
                                //Console.WriteLine("F" + faceNr + "L" + landMarkNr + "-" + cFaces[faceNr][landMarkNr].world.x + "," + cFaces[faceNr][landMarkNr].world.y + "," + cFaces[faceNr][landMarkNr].world.z);
                            }
                        }
                        // Get the elapsed time as a TimeSpan value.
                        for (int i = 0; i < finalFace.Length; i++)
                        {
                            finalFace[i].world.x /= numFaces - 1;
                            finalFace[i].world.y /= numFaces - 1;
                            finalFace[i].world.z /= numFaces - 1;
                        }
                        model.NullFace = finalFace;

                        index = 0;
                        finalFace = new PXCMFaceData.LandmarkPoint[80];
                        for (int i = 0; i < finalFace.Length; i++)
                            finalFace[i] = new PXCMFaceData.LandmarkPoint();
                        cFaces = new PXCMFaceData.LandmarkPoint[numFaces][];

                    }
                }
            }
        }
        // Sets frameUpdate to true, since work is called for each frame
        public override void Work(Graphics g)
        {
            if (!guInit) guiInit();
            frameUpdate = true;
            if(debug && calibrate)
            {
                model.View.Debug_Y += 20;
                g.DrawString("Calibrating", model.DefaultFont, model.DefaultStringBrush, new Point(0, model.View.Debug_Y));
            }
        }

        // for debugging only
        private void printFace(string v, PXCMFaceData.LandmarkPoint[] face)
        {
            Console.WriteLine(v);
            for (int i = 0; i < face.Length; i++)
            {
                PXCMFaceData.LandmarkPoint p = face[i];
                Console.WriteLine(i + ": " + p.world.x + ", " + p.world.y + ", " + p.world.z);
            }
        }
    }
}
