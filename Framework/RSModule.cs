using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RealSense
{
    /**
     * abstract module to implment the Init method of the model and contains the work method
     * 
     * @Author Tanja Witke
     */
    public abstract class RSModule
    {
        protected double MIN = 0, MAX = 0, MIN_TOL, MAX_TOL, XTREME_MAX, XTREME_MIN /*dayum*/;
        protected static double DEF_MIN, DEF_MAX;
        protected static Model model;
        public String output = "";
        protected bool debug = false;

        /**
        * initialise the model
        * @param m it is the model
        */
        public static void Init(Model m)
        {
            model = m;
        }

        /**
         *  Update every frame (do calculations, manipulate output Image)
         *  @param Graphics g
         */
        public abstract void Work(Graphics g);
        //protected abstract void dynamicMinMax();                                      T
        //protected abstract double[] convertValues(double[] vars);                     T

        protected double[] convertValues(double[] vars)
        {
            double[] ret = new double[vars.Length];

            for (int i = 0; i < vars.Length; i++)
            {
                if (vars[i] >= 0)
                    ret[i] = vars[i] * 100 / MAX;
                else
                    ret[i] = vars[i] * 100 / -MIN;
            }

            return ret;
        }

        protected double filterExtremeValues(double value)
        {
            if (value > XTREME_MAX) value = XTREME_MAX;
            else if (value < XTREME_MIN) value = XTREME_MIN;
            // Console.WriteLine("XTREME: " + XTREME_MAX + ", " + XTREME_MIN + " -> " + value);
            return value;
        }

        protected void dynamicMinMax(double[] dist)                    //Tanja
        {
            if (model.CurrentPoseDiff > 10) return; // haha kindof important ;)
        /*    Console.WriteLine("Left: " + model.EmotionValue(typeof(AU_BrowShift).ToString() + "_left"));
            Console.WriteLine("Right: " + model.EmotionValue(typeof(AU_BrowShift).ToString() + "_right"));
            Console.WriteLine("\n\n\n\n###################################\nAllDiff: " + model.CurrentPoseDiff);
            Console.Write("\tRollDiff: " + model.CurrentRollDiff);
            Console.Write("\tYawDiff: " + model.CurrentYawDiff);
            Console.Write("\tPitchDiff: " + model.CurrentPitchDiff);
            Console.Write("\nPitch + Yaw: " + (model.CurrentYawDiff + model.CurrentPitchDiff));
            Console.Write("\tRoll + Yaw: " + (model.CurrentYawDiff + model.CurrentRollDiff));
            Console.Write("\tRoll + Pitch: " + (model.CurrentPitchDiff + model.CurrentRollDiff));
            for (int i = 0; i < 10; i++)
                Console.WriteLine(i + ": " + model.CurrentFace[i].world.x + ","
                     + model.CurrentFace[i].world.y + ", "
                     + model.CurrentFace[i].world.z);
            for (int i = 70; i < 76; i++)
                Console.WriteLine(i + ": " + model.CurrentFace[i].world.x + ","
                     + model.CurrentFace[i].world.y + ", "
                     + model.CurrentFace[i].world.z);*/
            double temp = dist.Min();
            MIN = MIN < temp ? MIN : temp;
            temp = dist.Max();
            //   Console.WriteLine("\n" + MIN + ", " + MAX);
            MAX = MAX < temp ? temp : MAX;

        }

        public void reset()
        {
            MIN = DEF_MIN;
            MAX = DEF_MAX;
        }
    }

}
