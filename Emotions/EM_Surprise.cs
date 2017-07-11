using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense.Emotions
{
    /*
     *Measures the percentage value of anger. 
     *@author Tanja 
     */
    class EM_Surprise : RSModule
    {
        // Variables for logic
        int percent = 100;
        
        /**
         * Initializes the EM, setting the debug-flag to true by default
         */
        public EM_Surprise()
        {
            debug = true;
        }

        /**
         * Computes the percentage Value of Surprise in the current Frame.
         * @param Graphics g for the view
         * */
        public override void Work(Graphics g)
        {
            //proportions Surprise
            int p_brow = 45;
            int p_eye = 35;
            int p_jaw = 40;

            //brow Value 0-100
            double temp_left = model.AU_Values[typeof(AU_BrowShift).ToString() + "_left"];
            double temp_right = model.AU_Values[typeof(AU_BrowShift).ToString() + "_right"];
            double browValue = (temp_left + temp_right) / 2;
            browValue = browValue * p_brow / percent;

            //eye Value 0-100
            temp_left = model.AU_Values[typeof(AU_EyelidTight).ToString() + "_left"];
            temp_right = model.AU_Values[typeof(AU_EyelidTight).ToString() + "_right"];
            double eyeValue = (temp_left + temp_right) / 2;
            eyeValue = eyeValue * p_eye / percent;

            //jaw 0-100
            double jawValue = model.AU_Values[typeof(AU_JawDrop).ToString()];
            jawValue = jawValue * p_jaw / percent;

            //sum all and save
            double surprise = browValue + eyeValue + jawValue;
            surprise = surprise > 0 ? surprise : 0;
            model.Emotions[Model.Emotion.SURPRISE] = surprise;

            // print debug-values 
            if (debug)
            {
                output = "Surprise: " + (int)surprise;
            }
        }
    }
}



