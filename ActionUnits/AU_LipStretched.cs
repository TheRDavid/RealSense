using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /**
     * Measures whether the lips are stretched 
     * @author Tobias Schramm
     * @HogwartsHouse Hufflepuff
     * 
     * Interpretation:      -100 = Kissing
     *                         0 = Relaxed
     *                       100 = Frogface
     */
    class ME_LipStretched : RSModule
    {

        // variables for logic
        private double lips_corner_distance;
        double[] lips_corner_distances = new double[numFramesBeforeAccept];
        private string debug_message = "LipStretched: ";

        // Default values
        public ME_LipStretched()
        {
            DEF_MIN = -13;
            DEF_MAX = 13;
            reset();
            MIN_TOL = -2;
            MAX_TOL = 0.5;
            debug = true;
            XTREME_MAX = 60;
            XTREME_MIN = -45;
            model.AU_Values[typeof(ME_LipStretched).ToString()] = 0;
        }

        /**
         * @Override 
         * Calculates the difference between the two lip corners
         * @param Graphics g for the view
         **/
        public override void Work(Graphics g)
        {
            //Get Values from AU's
            lips_corner_distance = (Utilities.Difference(33, 39) - 100);

            //Gather Frames
            if (framesGathered < numFramesBeforeAccept)
            {
                lips_corner_distances[framesGathered++] = lips_corner_distance;
            }
            else
            {
                /* Update value in Model */
                if (model.CurrentPoseDiff < model.PoseMax)
                    model.AU_Values[typeof(ME_LipStretched).ToString()] = Utilities.ConvertValue(lips_corner_distances, MAX, MIN, MAX_TOL, MIN_TOL, XTREME_MAX, XTREME_MIN);

                /* print debug-values */
                if (debug)
                {
                    output = debug_message + "(" + (int)model.AU_Values[typeof(ME_LipStretched).ToString()] + ") (" + (int)MIN + ", " + (int)MAX + ")";
                }
                framesGathered = 0;
            }
        }
    }
}
