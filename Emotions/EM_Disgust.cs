using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense.Emotions
{
    class EM_Disgust : RSModule
    {
        int percent = 100;

        // Default values
        public EM_Disgust()
        {
            debug = true;
        }

        /*
         *  1 = inner brow raised -> BrowShift
            2 = outer brow raised -> BrowShift
            4 = brow lowered -> BrowShift
            5 = upper lid raised -> EyelidTight
            6 = cheeck raised -> CheeckRaised (not working)
            7 = lid tightened -> EyelidTight
            9 = nose wrinkled -> NoseWrinkled
            12 = lip corner pulled (up) -> LipCorner
            14 = grübchen -> none
            15 = lip corner lowered -> LipLine
            16 = lower lip lowered ->LowerLipLowered
            20 = lip stretched -> LipStretched
            23 = lip tightened -> LipsTightened
            26 = jaw drop -> JawDrap

            Verachtung (12 (R,L), 14(R,L)
            Trauer (1,4,15,(20?)
            Wut (4,5,6,23)
            Ekel (9,15,16,4)
            Überraschung (1,2,5B,26)
            Freude (6,12, 7)
            Angst (1,2,4,5,6,20,26)

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

          //  makeSmall();

            //Grenzen

            int noseMax = 50;

            //brow Value
            double temp_left = model.AU_Values[typeof(ME_BrowShift).ToString() + "_left"];
            double temp_right = model.AU_Values[typeof(ME_BrowShift).ToString() + "_right"];
            double browValue = temp_left > temp_right ? temp_left : temp_right;
            if (model.Test) browValue = (temp_left + temp_right) / 2;
            browValue = browValue * -1 * p_brow / percent;

            //NoseWrinkled (0 - -100)
            double noseValue = model.AU_Values[typeof(ME_NoseWrinkled).ToString()];
            //noseValue = 100 * noseValue / noseMax;
            noseValue = noseValue * -1 * p_nose / 100;                                                             //warum neg? Muss noch im ME korrigiert werden!!! Gruss Tanja

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

        private void makeSmall()
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






