using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /*
     *Measures if complete brow is raised or lowered (each eye)  - Action Unit Number 4 
     *@author Anton 
     *@date 20.03.2017
     *@HogwartsHouse Slytherin  
     */
    class AU_BrowShift : RSModule
    {
        // Variables for logic


        private double[] leftDistances = new double[numFramesBeforeAccept];
        private double[] rightDistances = new double[numFramesBeforeAccept];
        private double leftEyeBrow_r, leftEyeBrow_m, leftEyeBrow_l;
        private double rightEyeBrow_r, rightEyeBrow_m, rightEyeBrow_l;

        // Variables for debugging

        // Default values
        public AU_BrowShift()
        {
            DEF_MIN = -7;
            DEF_MAX = 12;
            reset();
            MIN_TOL = -2;
            MAX_TOL = 1;
            debug = true;
            XTREME_MAX = 33;
            XTREME_MIN = -24;
        }

        public override void Work(Graphics g)
        {
            /* Calculations */

            // calculates the difference between the Nullface and the currentface -> to check if the whole eyebrow is raised or lowered

            leftEyeBrow_r = model.Difference(0, Model.NOSE_FIX) - 100;
            leftEyeBrow_m = model.Difference(2, Model.NOSE_FIX) - 100;
            leftEyeBrow_l = model.Difference(4, Model.NOSE_FIX) - 100;

            rightEyeBrow_r = model.Difference(9, Model.NOSE_FIX) - 100;
            rightEyeBrow_m = model.Difference(7, Model.NOSE_FIX) - 100;
            rightEyeBrow_l = model.Difference(5, Model.NOSE_FIX) - 100;

            double ld = ((leftEyeBrow_r + leftEyeBrow_m + leftEyeBrow_l) / 3) * 1;
            double rd = ((rightEyeBrow_r + rightEyeBrow_m + rightEyeBrow_l) / 3) * 1;

            if (framesGathered < numFramesBeforeAccept)
            {
                leftDistances[framesGathered] = ld;
                rightDistances[framesGathered++] = rd;
            }
            else
            {
                for (int i = 0; i < numFramesBeforeAccept; i++)
                {
                    leftDistances[i] = leftDistances[i] < MAX_TOL && leftDistances[i] > MIN_TOL ? 0 : leftDistances[i];
                    rightDistances[i] = rightDistances[i] < MAX_TOL && rightDistances[i] > MIN_TOL ? 0 : rightDistances[i];
                }
                for (int j = 0; j < leftDistances.Length; j++)
                {
                    Console.WriteLine("LD: " + leftDistances[j] + ", RD: " + rightDistances[j]);
                }
                double leftDistance = filteredAvg(leftDistances);
                double rightDistance = filteredAvg(rightDistances);

                dynamicMinMax(new double[] { leftDistance, rightDistance });

                double[] diffs = convertValues(new double[] { leftDistance, rightDistance });

                model.setAU_Value(typeof(AU_BrowShift).ToString() + "_left", diffs[0]);
                model.setAU_Value(typeof(AU_BrowShift).ToString() + "_right", diffs[1]);

                double eyeDiff = Math.Abs(model.CurrentFace[14].world.y - model.CurrentFace[22].world.y);
                eyeDiff = Math.Abs(model.DifferenceByAxis(14, 22, Model.AXIS.Y, true) * 1000);
                Console.WriteLine("Eye Diff: " + eyeDiff + "\t" + (eyeDiff > 15 ? "ungerade" : "gerade"));
                //Console.WriteLine("EyeDiff: " + eyeDiff + (eyeDiff > 0.01 ? "ungerade" : "gerade"));
                if (Model.calibrated && (MAX >= XTREME_MAX || MIN <= XTREME_MIN))
                {
                    Console.WriteLine("Pose: " + model.CurrentPoseDiff);
                    Console.WriteLine("MIN: " + MIN + ", MAX: " + MAX);
                    model.View.colorBitmap.Save("C:\\Users\\prouser\\Pictures\\Saved Pictures\\err.png");
                    for (int i = 0; i < 10; i++)
                        Console.WriteLine(i + ": " + model.CurrentFace[i].world.x + ","
                             + model.CurrentFace[i].world.y + ", "
                             + model.CurrentFace[i].world.z);
                    for (int i = 70; i < 76; i++)
                        Console.WriteLine(i + ": " + model.CurrentFace[i].world.x + ","
                             + model.CurrentFace[i].world.y + ", "
                             + model.CurrentFace[i].world.z);
                    Environment.Exit(0);
                }

                // print debug-values 
                if (debug)
                {
                    output = "BrowShift: " + "(" + (int)diffs[0] + ", " + (int)diffs[1] + ")(" + (int)MIN + ", " + (int)MAX + ")";
                }
                framesGathered = 0;
            }
        }

        /*
        protected override void dynamicMinMax()                                                                     T
        {
            double temp = leftDistance > rightDistance ? rightDistance : leftDistance;
            Console.WriteLine(temp);

            MIN = MIN < temp ? MIN : temp;
            temp = leftDistance > rightDistance ? leftDistance : rightDistance;
            MAX = MAX < temp ? temp : MAX;
        }
        */

    }
}
