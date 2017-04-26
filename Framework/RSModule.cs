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
        protected double MIN, MAX, MIN_TOL, MAX_TOL;
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

        protected void dynamicMinMax(double[] dist)                    //Tanja
        {
            if (model.CurrentPoseDiff > 10) return;
            Console.WriteLine("\n\nAllDiff: " + model.CurrentPoseDiff);
            Console.Write("\tRollDiff: " + model.CurrentRollDiff);
            Console.Write("\tYawDiff: " + model.CurrentYawDiff);
            Console.Write("\tPitchDiff: " + model.CurrentPitchDiff);
            Console.Write("\nPitch + Yaw: " + (model.CurrentYawDiff + model.CurrentPitchDiff));
            Console.Write("\tRoll + Yaw: " + (model.CurrentYawDiff + model.CurrentRollDiff));
            Console.Write("\tRoll + Pitch: " + (model.CurrentPitchDiff + model.CurrentRollDiff));
            double temp = dist.Min();
            MIN = MIN < temp ? MIN : temp;
            temp = dist.Max();
            Console.WriteLine("\n"+MIN + ", " + MAX);
            MAX = MAX < temp ? temp : MAX;

        }

        public void reset()
        {
            MIN = DEF_MIN;
            MAX = DEF_MAX;
        }
    }

}
