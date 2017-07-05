using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /*
     *Measures the percentage value of anger. 
     *@author Tanja 
     */
    class EM_Anger : RSModule
    {
        // Variables for logic
        int percent = 100;

        // Default values
        public EM_Anger()
        {
            debug = true;
        }

        /**
         * Computes the percentage Value of Anger in the current Frame.
         * @param Graphics g for the view
         * */
        public override void Work(Graphics g)
        {
            //proportions Anger
            int p_brow = 65;
            int p_lid = 20;
            int p_lip = 25;

            //reduce();

            //brow Value
            double temp_left = model.AU_Values[typeof(AU_BrowShift).ToString() + "_left"];
            double temp_right = model.AU_Values[typeof(AU_BrowShift).ToString() + "_right"];
            double browValue = (temp_left + temp_right) / 2;
            browValue = browValue * -1 * p_brow / percent;

            //lid Value
            temp_left = model.AU_Values[typeof(AU_EyelidTight).ToString() + "_left"];
            temp_right = model.AU_Values[typeof(AU_EyelidTight).ToString() + "_right"];
            double lidValue = (temp_left + temp_right) / 2;
            lidValue = lidValue * -1 * p_lid / percent;

            //lip Value
            double lipValue = model.AU_Values[typeof(AU_LipsTightened).ToString()];
            lipValue = lipValue * -1 * p_lip / percent;

            //sum all and save
            double anger = (browValue > 0 ? browValue : 0) + lidValue + lipValue;
            anger = anger > 0 ? anger : 0;
            model.Emotions[Model.Emotion.ANGER] = anger;

            // print debug-values 
            if (debug)
            {
                output = "Anger: " + (int)anger + ", Brow: " + (int)browValue + ", Lid: " + (int)lidValue + ", lip: " + (int)lipValue;
            }

        }

        /**
         * Reduces the value boundaries of the emotion value. 
         * A reduced value doesn't reach the 100 anymore. This is happening if an AU_value is active that doesn't match with this emotion.
         *  
         * */
        private void reduce()
        {
            //Anger

            int lowerLipRaised = (int)model.AU_Values[typeof(AU_LowerLipRaised).ToString()];
            if (lowerLipRaised < -80)
            {
                percent = (int)(100 - lowerLipRaised * 0.5);
            }

        }
    }
}
