using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense.Emotions
{
    class EM_Sadness : RSModule
    {
        // Variables for logic

        // Default values
        public EM_Sadness()
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
            //Sadness --> BrowShift, LipLine, (LipStreched), EyelidTight

            //percentage Sadness
            int p_brow = 0;
            //int p_lid = 50;
            int p_lipL = 90;
            int p_lipS = 0;
            int p_lid = 20;

            //brow Value 0-100
            double temp_left = model.AU_Values[typeof(ME_BrowShift).ToString() + "_left"];
            double temp_right = model.AU_Values[typeof(ME_BrowShift).ToString() + "_right"];
            double browValue = temp_left < temp_right ? temp_left : temp_right;
            browValue = browValue * p_brow / 100;

            //lid values
            double lidValue = model.AU_Values[typeof(ME_EyelidTight).ToString() + "_left"] + model.AU_Values[typeof(ME_EyelidTight).ToString() + "_right"];
            lidValue *= -1;
            lidValue *= p_lid;
            lidValue /= 100;

            //lipL Value 0 - -100
            double lipLValue = model.AU_Values[typeof(ME_LipLine).ToString()];
            lipLValue = lipLValue * -1 * p_lipL / 100;

            //lipS Value 0 - -100
            double lipSValue = model.AU_Values[typeof(ME_LipStretched).ToString()];
            lipSValue = lipSValue * -1 * p_lipS / 100;

            double sad = lidValue + lipLValue;
            sad = sad > 0 ? sad : 0;
            model.Emotions["Sadness"] = sad;

            // print debug-values 
            if (debug)
            {
                output = "Sadness: " + (int)sad + " LipL: " + (int)lipLValue + " Lid: " + lidValue + " LipS: " + (int)lipSValue + " Brow: " + browValue;
            }

        }
    }
}




