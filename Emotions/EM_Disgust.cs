using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense.Emotions
{
    /*
     *Measures the percentage value of disgust. 
     *@author Tanja 
     */
    class EM_Disgust : RSModule
    {
        // Variables for logic
        int percent = 100;

        // Default values
        public EM_Disgust()
        {
            debug = true;
        }

        /**
         * Computes the percentage Value of Disgust in the current Frame.
         * @param Graphics g for the view
         * */
        public override void Work(Graphics g)
        {
            //proportions Disgust
            int p_brow = 15;
            int p_nose = 40;
            int p_lipLine = 5;
            int p_lipLowered = 0;
            int p_upperLip = 40;
            //int p_lipStr = 

            //reduce();

            //brow Value
            double temp_left = model.AU_Values[typeof(ME_BrowShift).ToString() + "_left"];
            double temp_right = model.AU_Values[typeof(ME_BrowShift).ToString() + "_right"];
            double browValue = (temp_left + temp_right) / 2;
            browValue = browValue * -1 * p_brow / percent;

            //NoseWrinkled (0 - -100)
            double noseValue = model.AU_Values[typeof(ME_NoseWrinkled).ToString()];
            noseValue = noseValue * -1 * p_nose / 100;

            //lipLine Value 0 - -100
            double lipLineValue = model.AU_Values[typeof(ME_LipLine).ToString()];
            lipLineValue = lipLineValue * -1 * p_lipLine / percent;

            //LowerLip 0-100
            double lipLoweredValue = model.AU_Values[typeof(ME_LowerLipLowered).ToString()];
            lipLoweredValue = lipLoweredValue * p_lipLowered / percent;

            //upperLip 0-100
            double upperLipValue = model.AU_Values[typeof(ME_UpperLipRaised).ToString()];
            upperLipValue = upperLipValue * p_upperLip / percent;

            // Falls Corners durch Disgust, auf 0 setzen
            double hDiff = Utilities.DifferenceByAxis(33, 35, Model.AXIS.Y, false) + Utilities.DifferenceByAxis(39, 37, Model.AXIS.Y, false);
            if (hDiff > 0)
            {
                lipLineValue = 0;
                upperLipValue = 0;
            }

            //sum all and save
            double disgust = browValue + noseValue + lipLoweredValue + lipLineValue + upperLipValue;
            disgust = disgust > 0 ? disgust : 0;
            disgust = disgust < 100 ? disgust : 100;
            model.Emotions[Model.Emotion.DISGUST] = disgust;

            // print debug-values 
            if (debug)
            {
                output = "Disgust: " + (int)disgust + " Brow: " + (int)browValue + " Nose: " + (int)noseValue + " LipUpper: " + (int)upperLipValue;
            }

        }

        /**
         * Reduces the value boundaries of the emotion value. 
         * A reduced value doesn't reach the 100 anymore. This is happening if an AU_value is active that doesn't match with this emotion.
         *  
         * */
        private void reduce()
        {
            //lipS Value 0 - -100
            double lipSValue = model.AU_Values[typeof(ME_LipStretched).ToString()];
            lipSValue = lipSValue * -1;
            if (lipSValue < 60)
            {
                percent = (int)(100 + 1.5 * (60 - lipSValue));
            }


        }
    }
}






