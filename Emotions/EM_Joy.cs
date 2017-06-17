using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense.Emotions
{
    class EM_Joy : RSModule
    {
        int percent = 100;
        double[] smallerArray;

        // Default values
        public EM_Joy()
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
            //Joy --> EyelidTight, LipCorner

            //percentage Joy
            int p_lid = 20;
            int p_lip = 80;

            makeSmall();

            //lid Value 0 - -100 (Grenze bei lidMax)
            double temp_left = model.AU_Values[typeof(ME_EyelidTight).ToString() + "_left"];
            double temp_right = model.AU_Values[typeof(ME_EyelidTight).ToString() + "_right"];
            double lidValue = (temp_left + temp_right) / 2;
            lidValue = lidValue * -1 * p_lid / percent;

            //lip Value 0 - 100
            temp_left = Math.Abs(model.AU_Values[typeof(ME_LipCorner).ToString() + "_left"]);
            temp_right = Math.Abs(model.AU_Values[typeof(ME_LipCorner).ToString() + "_right"]);
            double lipValue = temp_left > temp_right ? temp_right : temp_left;
            //lipValue = (temp_left + temp_right) / 2;
            lipValue = lipValue * p_lip / percent;

            //lipL Value 0 - -100
            double lipLValue = model.AU_Values[typeof(ME_LipLine).ToString()];
            lipLValue = lipLValue * p_lip / percent;

            lipValue = lipValue > lipLValue ? lipValue : lipLValue;

            double joy = lidValue + lipValue;// + browValue;

            joy = joy > 0 ? joy : 0;

            model.Emotions[Model.Emotion.JOY] = joy;

            // print debug-values 
            if (debug)
            {
                //output = "Joy: " + (int)joy + " LipCorner: " + (int)lipValue + " LipLine: " + (int)lipLValue + " Eye: " + (int)lidValue; // + " Brow: " + browValue;
            }

        }

        private void makeSmall()
        {
            //Anger brows
            double temp_left = model.AU_Values[typeof(ME_BrowShift).ToString() + "_left"];
            double temp_right = model.AU_Values[typeof(ME_BrowShift).ToString() + "_right"];
            double browValue = (temp_left + temp_right) / 2;
            browValue = browValue * -1 - 30;

            //Disgust nose 
            double noseValue = model.AU_Values[typeof(ME_NoseWrinkled).ToString()];
            noseValue = noseValue * -1;

            //Sadness Lipline
            double lipLValue = model.AU_Values[typeof(ME_LipLine).ToString()];
            lipLValue = lipLValue * -1;

            //Contempt
            //not possible yet (ME needed)

            //Surprise Lid
            temp_left = model.AU_Values[typeof(ME_EyelidTight).ToString() + "_left"];
            temp_right = model.AU_Values[typeof(ME_EyelidTight).ToString() + "_right"];
            double eyeValue = (temp_left + temp_right) / 2;

            smallerArray = new double[] { browValue, lipLValue, eyeValue };
            percent = 100 + (int)(2 * smallerArray.Max());

            int lipLowered = (int)model.AU_Values[typeof(ME_LowerLipLowered).ToString()];
            if (lipLowered > 70)
                percent = 100 + 2 * lipLowered;
            percent = percent < 100 ? 100 : percent;

            if (debug)
            {
                //output = "Joy: " + (int)joy + " LipCorner: " + (int)lipValue + " LipLine: " + (int)lipLValue + " Eye: " + (int)lidValue; // + " Brow: " + browValue;
                output = " Smaller: " + percent + " brow: " + (int)browValue + " Lip: " + (int)lipLValue + " Lid: " + (int)eyeValue;
            }

        }
    }
}






