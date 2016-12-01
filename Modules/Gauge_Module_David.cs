using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RealSense
{
    public class Gauge_Module_David : RSModule
    {
        private const int numFaces = 30;
        private bool calibrate = false;
        private bool guInit = false;
        private PXCMFaceData.LandmarkPoint[][] cFaces = new PXCMFaceData.LandmarkPoint[numFaces][];
        private PXCMFaceData.LandmarkPoint[] finalFace = new PXCMFaceData.LandmarkPoint[80];
        private int index = 0;
        private Button calibrateButton = new Button();
        private void guiInit()
        {
            guInit = true;
            calibrateButton.Bounds = new Rectangle(20, 530, 500, 30);
            calibrateButton.Text = "Calibrate";
            calibrateButton.Click +=
                new System.EventHandler(delegate { calibrate = true; });
            model.View.AddComponent(calibrateButton);
            for (int i = 0; i < finalFace.Length; i++)
                finalFace[i] = new PXCMFaceData.LandmarkPoint();
        }

        public override void Work(Graphics g)
        {
            if (!guInit) guiInit();
            if (calibrate)
            {
                calibrate = index != numFaces - 1;
                if (model.FaceAktuell != null)
                {
                    // get the landmark data

                    if (model.Lp == null) return;
                    Console.WriteLine("Calibrate #" + index);
                    model.Lp.QueryPoints(out cFaces[index++]);
                }
                if (!calibrate)
                {
                    int landMarkNr = 0, faceNr = 0;
                    for (int i = 0; i < numFaces; i++)
                        printFace("Face " + i, cFaces[i]);
                    //Console.WriteLine("FI0: " + cFaces[landMarkNr].Length);
                    for (; landMarkNr < cFaces[faceNr].Length; landMarkNr++)
                    {
                        for (faceNr = 0; faceNr < cFaces.Length - 1; faceNr++)
                        {
                            finalFace[landMarkNr].world.x += cFaces[faceNr][landMarkNr].world.x;
                            finalFace[landMarkNr].world.y += cFaces[faceNr][landMarkNr].world.y;
                            finalFace[landMarkNr].world.z += cFaces[faceNr][landMarkNr].world.z;
                            //Console.WriteLine("F" + faceNr + "L" + landMarkNr + "-" + cFaces[faceNr][landMarkNr].world.x + "," + cFaces[faceNr][landMarkNr].world.y+"," + cFaces[faceNr][landMarkNr].world.z);
                        }
                    }
                    for(int i = 0; i < finalFace.Length; i++)
                    {
                        finalFace[i].world.x /= numFaces-1;
                        finalFace[i].world.y /= numFaces-1;
                        finalFace[i].world.z /= numFaces-1;
                    }
                    printFace("Final Face: ", finalFace);
                    model.NormalFace = finalFace;
                    index = 0;
                    finalFace = new PXCMFaceData.LandmarkPoint[80];
                    for (int i = 0; i < finalFace.Length; i++)
                        finalFace[i] = new PXCMFaceData.LandmarkPoint();
                    cFaces = new PXCMFaceData.LandmarkPoint[numFaces][];
                }
            }
        }

        private void printFace(string v, PXCMFaceData.LandmarkPoint[] face)
        {
            Console.WriteLine(v);
            for(int i = 0; i < face.Length; i++)
            {
                PXCMFaceData.LandmarkPoint p = face[i];
                Console.WriteLine(i + ": " + p.world.x + ", " + p.world.y + ", " + p.world.z);
            }
        }
    }
}
