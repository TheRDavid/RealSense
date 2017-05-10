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
        protected static int numFramesBeforeAccept = 20;
        protected int framesGathered = 0;

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

        /**
         * called in dynamicMinMax
         */
        protected double filteredAvg(double[] values)
        {
            double average = 0, numAverages = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] > XTREME_MAX) average += XTREME_MAX;
                else if (values[i] < XTREME_MIN) average += XTREME_MIN;
                else average += values[i];
                numAverages++;
            }

            average /= numAverages;

            return average;
        }

        protected void filterToleranceValues(double[] values)
        {
            for(int i = 0; i < values.Length; i++)
            {
                Console.WriteLine("Found " + values[i]);
                values[i] = values[i] < MAX_TOL && values[i] > MIN_TOL ? 0 : values[i];
            }
        }

        protected void dynamicMinMax(double[] dist)
        {
            if (model.CurrentPoseDiff > 10)
            { output = ""; return; }
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
