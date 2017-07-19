using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense.Emotions
{
    class EM_Fear : RSModule
    {
        // Default values
        public EM_Fear()
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
            //Fear --> BrowShift, EyelidTight, LipStreched, JawDrop

            //percentage Fear
            int p_brow = 30;
            int p_eye = 50;
            int p_lip = 10;
            int p_jaw = 40;

            //brow Value 0-100
            double temp_left = model.AU_Values[typeof(AU_BrowShift).ToString() + "_left"];
            double temp_right = model.AU_Values[typeof(AU_BrowShift).ToString() + "_right"];
            double browValue = temp_left < temp_right ? temp_left : temp_right;
            if (model.Test) browValue = (temp_left + temp_right) / 2;
            browValue = browValue * p_brow / 100;

            //eye Value 0-100
            temp_left = model.AU_Values[typeof(AU_EyelidTight).ToString() + "_left"];
            temp_right = model.AU_Values[typeof(AU_EyelidTight).ToString() + "_right"];
            double eyeValue = temp_left < temp_right ? temp_left : temp_right;
            if (model.Test) eyeValue = (temp_left + temp_right) / 2;
            eyeValue = eyeValue * p_eye / 100;

            //lipLine Value 0-100
            double lipValue = model.AU_Values[typeof(AU_LipStretched).ToString()];
            lipValue = lipValue * p_lip / 100;

            //jaw 0-100
            double jawValue = model.AU_Values[typeof(AU_JawDrop).ToString()];
            jawValue = jawValue * p_jaw / 100;

            double fear = browValue + eyeValue + lipValue + jawValue;
            fear = fear > 0 ? fear : 0;
            model.Emotions[Model.Emotion.FEAR] = fear;

            // print debug-values 
            if (debug)
            {
                output = "Fear: " + (int)fear;
            }

        }
    }
}









