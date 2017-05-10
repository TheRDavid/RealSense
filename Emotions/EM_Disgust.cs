using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense.Emotions
{
    class EM_Disgust : RSModule
    {
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
            //Disgust --> BrowShift, NoseWrinkled, LipLine, LowerLipLowered

            //percentage Anger
            int p_brow = 20;
            int p_nose = 35;
            int p_lipLine = 20;
            int p_lipLowered = 25;

            //brow Value
            double temp_left = model.AU_Values[typeof(ME_BrowShift).ToString() + "_left"];
            double temp_right = model.AU_Values[typeof(ME_BrowShift).ToString() + "_right"];
            double browValue = temp_left > temp_right ? temp_left : temp_right;
            browValue = browValue * -1 * p_brow / 100;

            //NoseWrinkled (0 - -100)
            double noseValue = model.AU_Values[typeof(ME_NoseWrinkled).ToString()];
            noseValue = noseValue * -1 * p_nose / 100;

            //lipLine Value 0 - -100
            double lipLineValue = model.AU_Values[typeof(ME_LipLine).ToString()];
            lipLineValue = lipLineValue * -1 * p_lipLine / 100;

            //LowerLip 0-100
            double lipLoweredValue = model.AU_Values[typeof(ME_LowerLipLowered).ToString()];
            lipLoweredValue = lipLoweredValue * -1 * p_lipLine / 100;

            double disgust = browValue + noseValue + lipLoweredValue + lipLineValue;
            disgust = disgust > 0 ? disgust : 0;
            model.Emotions["Disgust"] = disgust;

            // print debug-values 
            if (debug)
            {
                output = "Disgust: " + disgust;
            }

        }
    }
}






