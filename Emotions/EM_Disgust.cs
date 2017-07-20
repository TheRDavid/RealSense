using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense.Emotions
{     /*
   *Measures the percentage value of disgust. 
   *@author Tanja 
   */
    class EM_Disgust : RSModule
    {
   

        /**
    * Initializes the EM, setting the debug-flag to true by default
    * Also sets up a default boundary of max-, min, extreme- and tolerance-values
    */
        public EM_Disgust()
        {
            debug = true;
        }


        /**
             * Computes the percentage Value of disgust in the current Frame.
             * @param Graphics g for the view
             * */
        public override void Work(Graphics g)
        {
            //Disgust --> BrowShift, NoseWrinkled, LipLine, LowerLipLowered, UpperLipRaised
            //neg lipStreched

            //percentage Disgust
            int p_brow = 15;
            int p_nose = 40;
            int p_lipLine = 5;
            int p_lipLowered = 0;
            int p_upperLip = 40;
            //int p_lipStr = 

            //Grenzen

            int noseMax = 50;

            //brow Value
            double temp_left = model.AU_Values[typeof(AU_BrowShift).ToString() + "_left"];
            double temp_right = model.AU_Values[typeof(AU_BrowShift).ToString() + "_right"];
            double browValue = temp_left > temp_right ? temp_left : temp_right;
            if (model.Test) browValue = (temp_left + temp_right) / 2;
            browValue = browValue * -1 * p_brow / 100;

            //NoseWrinkled (0 - -100)
            double noseValue = model.AU_Values[typeof(AU_NoseWrinkled).ToString()];
            //noseValue = 100 * noseValue / noseMax;
            noseValue = noseValue * -1 * p_nose / 100;                                                           

            //lipLine Value 0 - -100
            double lipLineValue = model.AU_Values[typeof(AU_LipLine).ToString()];
            lipLineValue = lipLineValue * -1 * p_lipLine / 100;

            //LowerLip 0-100
            double lipLoweredValue = model.AU_Values[typeof(AU_LowerLipLowered).ToString()];
            lipLoweredValue = lipLoweredValue * p_lipLowered / 100;

            //upperLip 0-100
            double upperLipValue = model.AU_Values[typeof(AU_UpperLipRaised).ToString()];
            upperLipValue = upperLipValue * p_upperLip / 100;

            //lipS Value 0 - -100
            double lipSValue = model.AU_Values[typeof(AU_LipStretched).ToString()];
            lipSValue = lipSValue * -1;
            lipSValue = lipSValue < 0 ? lipSValue : 0;


            // Falls Corners durch Disgust, auf 0 setzen
            double hDiff = model.DifferenceByAxis(33, 35, Model.AXIS.Y, false) + model.DifferenceByAxis(39, 37, Model.AXIS.Y, false);
            if (hDiff > 0)
            {
                lipLineValue = 0;
                upperLipValue = 0;
            }

            double disgust = browValue + noseValue + lipLoweredValue + lipLineValue + upperLipValue;// + lipSValue; 
            disgust = disgust > 0 ? disgust : 0;
            disgust = disgust < 100 ? disgust : 100;

            model.Emotions[Model.Emotion.DISGUST] = disgust;

            // print debug-values 
            if (debug)
            {
                output = "Disgust: " + (int)disgust + " Brow: " + (int)browValue + " Nose: " + (int)noseValue + " LipUpper: " + (int)upperLipValue;// + " LipS: " + lipSValue;
            }

        }
    }
}






