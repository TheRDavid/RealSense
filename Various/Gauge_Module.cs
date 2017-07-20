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

    /**
 * 
 * Module used to calibrate the user's nullface by averaging over a set amount of frames (numFaces)
 * 
 * @author: David Rosenbusch
 * @HogwartsHouse Hufflepuff
 */
    public class Gauge_Module : RSModule
    {
        private const int numFaces = 150;
        public bool calibrate = false;
        private bool guInit = false;
        private PXCMFaceData.LandmarkPoint[][] cFaces = new PXCMFaceData.LandmarkPoint[numFaces][];
        private PXCMFaceData.LandmarkPoint[] finalFace = new PXCMFaceData.LandmarkPoint[80];
        private PXCMFaceData.PoseEulerAngles[] cAngles = new PXCMFaceData.PoseEulerAngles[numFaces];
        private PXCMFaceData.PoseEulerAngles finalAngle = new PXCMFaceData.PoseEulerAngles();
        private int index = 0;
        private Button calibrateButton = new Button();
        public bool frameUpdate = true;

        /**
         * Inits the UI
         */
        private void GuiInit()
        {
            guInit = true;
            calibrateButton.Bounds = new Rectangle(20, 1140, 500, 30);
            calibrateButton.Text = "Calibrate";
            calibrateButton.Click +=
                new System.EventHandler(delegate
                {
                    calibrate = true;
                    model.View.ResetModules = true;
                });
            model.View.AddComponent(calibrateButton);
            for (int i = 0; i < finalFace.Length; i++)
                finalFace[i] = new PXCMFaceData.LandmarkPoint();

            Thread updaterThread = new Thread(this.Update);
            updaterThread.Start();
            debug = true;
        }

        /**
        * Gathers calibration-data.
        * When a new frame is shot AND still calibrating (button was pressed), adds a new face to the FilteredAvg.
        * After having numFaces faces, calculate...
        * Runs outside the Camera Thread
        */
        public void Update()
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

                            cAngles[index] = model.CurrentPose;
                            if (model.Lp == null) return;
                            cFaces[index++] = model.CurrentFace;
                         //   Console.WriteLine(numFaces + " / " + index + " * 100");
                            model.calibrationProgress = (double)index / (double)numFaces  * 100.0;
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

                        for (int i = 0; i < finalFace.Length; i++)
                        {
                            finalFace[i].world.x /= numFaces - 1;
                            finalFace[i].world.y /= numFaces - 1;
                            finalFace[i].world.z /= numFaces - 1;
                        }

                        foreach(PXCMFaceData.PoseEulerAngles a in cAngles)
                        {
                            finalAngle.pitch += a.pitch;
                            finalAngle.roll += a.roll;
                            finalAngle.yaw += a.yaw;
                        }

                        finalAngle.pitch /= numFaces;
                        finalAngle.roll /= numFaces;
                        finalAngle.yaw /= numFaces;

                        model.NullPose = finalAngle;

                        model.NullFace = finalFace;
                        Model.calibrated = true;

                        index = 0;
                        finalFace = new PXCMFaceData.LandmarkPoint[80];
                        for (int i = 0; i < finalFace.Length; i++)
                            finalFace[i] = new PXCMFaceData.LandmarkPoint();
                        cFaces = new PXCMFaceData.LandmarkPoint[numFaces][];
                     //   Console.WriteLine("DOne Calibrating");

                    }
                }
            }
        }
        /**
          * Initializes UI + simple output while calibrating
          * @param Graphics g for the view
          */
        public override void Work(Graphics g)
        {
            if (!guInit) GuiInit();
            frameUpdate = true;
            if (debug && calibrate)
            {
                output = "Calibrating";
            }
            else if (!calibrate) output = "";
        }

        /**
         * Prints Face-Values (for debugging only)
         */
        private void printFace(string v, PXCMFaceData.LandmarkPoint[] face)
        {
            //  Console.WriteLine(v);
            for (int i = 0; i < face.Length; i++)
            {
                PXCMFaceData.LandmarkPoint p = face[i];
                //   Console.WriteLine(i + ": " + p.world.x + ", " + p.world.y + ", " + p.world.z);
            }
        }
    }
}
