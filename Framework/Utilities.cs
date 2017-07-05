using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace RealSense
{


    /**
     * Provides all methods that will be used by the modules.
     * 
     * @author Tanja Witke
     */
    public class Utilities
    {
        private static Model model;

        public static void Init(Model model)
        {
            Utilities.model = model;
        }

        /**
             * Calculates the percentage Value depending on the new Min/Max boundarys. 
             * @param distances collected from several frames
             * @returns the percentage value
             * */
        public static double ConvertValue(double[] distances, double MAX, double MIN, double MAX_TOL, double MIN_TOL, double XTREME_MAX, double XTREME_MIN)
        {
            FilterToleranceValues(distances, MAX_TOL, MIN_TOL);
            double distance = FilteredAvg(distances, XTREME_MAX, XTREME_MIN);
            DynamicMinMax(distance, MAX, MIN);
            if (distance >= 0) return distance * 100 / MAX;
            else return distance * 100 / -MIN;
        }

        /**
         * Returns the total difference of axis-specific distance between two points
         * @param i01,i02 which are the current points to calculate the difference
         * @param axis which is the specific axis to work with
         * @param absolute defines wether or not the absolute difference should be returned or not
         */
        public static double DifferenceByAxis(int i01, int i02, Model.AXIS axis, bool absolute)
        {
            return NullFaceBetweenByAxis(i01, i02, axis, absolute) - BetweenByAxis(i01, i02, axis, absolute);
        }

        /**
         * Calculates the axis-specific difference of the points from the ABSOLUTENullFace
         * @param i01,i02  which are the current points to calculate the difference
         * @param axis which is the specific axis to work with
         * @param absolute defines wether or not the absolute difference should be returned or not
         */
        public static double NullFaceBetweenByAxis(int i01, int i02, Model.AXIS axis, bool absolute)
        {
            double result = 0;
            switch (axis)
            {
                case Model.AXIS.X: result = model.NullFace[i02].world.x - model.NullFace[i01].world.x; break;
                case Model.AXIS.Y: result = model.NullFace[i02].world.y - model.NullFace[i01].world.y; break;
                case Model.AXIS.Z: result = model.NullFace[i02].world.z - model.NullFace[i01].world.z; break;
            }
            return absolute ? Math.Abs(result) : result;
        }

        /**
         * Calculates the axis-specific difference of the points from the ABSOLUTENullFace
         * @param i01,i02  which are the current points to calculate the difference
         * @param axis which is the specific axis to work with
         * @param absolute defines wether or not the absolute difference should be returned or not
         */
        public static double BetweenByAxis(int i01, int i02, Model.AXIS axis, bool absolute)
        {
            double result = 0;
            switch (axis)
            {
                case Model.AXIS.X: result = model.CurrentFace[i02].world.x - model.CurrentFace[i01].world.x; break;
                case Model.AXIS.Y: result = model.CurrentFace[i02].world.y - model.CurrentFace[i01].world.y; break;
                case Model.AXIS.Z: result = model.CurrentFace[i02].world.z - model.CurrentFace[i01].world.z; break;
            }
            return absolute ? Math.Abs(result) : result;
        }

        /**
         * calculates the percentage of the difference of distance between two points
         * @param i01,i02  which are the current points to calculate the difference
         * @returns double between 0 and 100
         */
        public static double Difference(int i01, int i02)
        {
            return 100 / NullFaceBetween(i01, i02) * Between(i01, i02); // calculates the percent (rule of three)
        }

        /**
         * David
         * 
         * */
        public static double DifferenceNullCurrent(int i01, Model.AXIS axis)
        {
            double result = 0;
            switch (axis)
            {
                case Model.AXIS.X: result = (model.NullFace[Model.NOSE_FIX].world.x - model.NullFace[i01].world.x) - (model.CurrentFace[Model.NOSE_FIX].world.x - model.CurrentFace[i01].world.x); break;
                case Model.AXIS.Y: result = (model.NullFace[Model.NOSE_FIX].world.y - model.NullFace[i01].world.y) - (model.CurrentFace[Model.NOSE_FIX].world.y - model.CurrentFace[i01].world.y); break;
                case Model.AXIS.Z: result = (model.NullFace[Model.NOSE_FIX].world.z - model.NullFace[i01].world.z) - (model.CurrentFace[Model.NOSE_FIX].world.z - model.CurrentFace[i01].world.z); break;
            }
            return result;
        }

        /**
         * calculates the differenc of the points from the ABSOLUTENullFace
         * @param i01,i02  which are the current points to calculate the difference 
         */
        public static double NullFaceBetween(int i01, int i02)
        {
            if (model.NullFace[i01].world.x != 0) // -------->wofür die abfrage ?  wäre es nicht sinnvoller auf null zu prüfen ?  Tanja said the nullface is never null, so that was the reason why she used 0 instead of null
            {
                double a = Math.Abs(model.NullFace[i02].world.y - model.NullFace[i01].world.y);
                double b = Math.Abs(model.NullFace[i02].world.x - model.NullFace[i01].world.x);
                double c = Math.Abs(model.NullFace[i02].world.z - model.NullFace[i01].world.z);
                return Math.Sqrt(a * a + b * b + c * c);  //vector analysis of the length (Schuett ahu!) 
            }
            throw new NullReferenceException();
        }
        /**
         * calculates the difference between the two points of the current frame
         * @param i01,i02  which are the current points to calculate the difference 
         */
        public static double Between(int i01, int i02)
        {
            PXCMFaceData.LandmarkPoint point01 = null;
            PXCMFaceData.LandmarkPoint point02 = null;

            if (model.Lp != null || !model.Stream)
            {
                point01 = model.CurrentFace[i01];
                point02 = model.CurrentFace[i02];

                double a = Math.Abs(point02.world.y - point01.world.y);
                double b = Math.Abs(point02.world.x - point01.world.x);
                double c = Math.Abs(point02.world.z - point01.world.z);
                return Math.Sqrt(a * a + b * b + c * c);
            }
            throw new NullReferenceException();

        }

        /**
         * Calculates the average value.
         * @param values all given values
         * @returns double average of all given values
         */
        private static double FilteredAvg(double[] values, double XTREME_MAX, double XTREME_MIN)
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

        /**
         * David
         * 
         * */
        private static void FilterToleranceValues(double[] values, double MAX_TOL, double MIN_TOL)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i] < MAX_TOL && values[i] > MIN_TOL ? 0 : values[i];
            }
        }

        /**
         * Puts a new Min/Max value if the dist is higher/lower than the old one.
         * @param dist value to compare
         * */
        private static void DynamicMinMax(double dist, double MAX, double MIN)
        {
            if (model.CurrentPoseDiff > model.PoseMax) return;
            MIN = MIN < dist ? MIN : dist * 0.9;
            MAX = MAX < dist ? dist * 0.9 : MAX;
        }

    }
}