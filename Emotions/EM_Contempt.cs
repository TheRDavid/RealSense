using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense.Emotions
{
    class EM_Contempt : RSModule
    {
        // Default values
        public EM_Contempt()
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
            //Contempt --> BrowShift, LipCorner

            //percentage Contempt
            int p_brow = 100;
            int p_lip = 100;

            //Maxs
            int browMax = 60;
            int lipMax = 50;

            //brow Difference (0-browMax) --> browMax entspricht 100
            double temp_left = model.AU_Values[typeof(ME_BrowShift).ToString() + "_left"];
            double temp_right = model.AU_Values[typeof(ME_BrowShift).ToString() + "_right"];
            double browValue = Math.Abs(temp_left-temp_right);
            //macht aus 0-100 (0 --> keine Differenz und 100 volle (aber nicht maximale.. (geht ja bis -100)) Differenz) ein 0-browMax
            //browValue = 100 * browValue / browMax; 
            browValue = browValue * p_brow / 100;

            //lipL Value 0 - -100
            temp_left = model.AU_Values[typeof(ME_LipCorner).ToString() + "_left"];
            temp_right = model.AU_Values[typeof(ME_LipCorner).ToString() + "_right"];
            double lipValue = Math.Abs(temp_left - temp_right);
            //macht aus 0-100 (0 --> keine Differenz und 100 volle (aber nicht maximale.. (geht ja bis -100)) Differenz) ein 0-lipMax
            //lipValue = 100 * lipValue / lipMax;
            lipValue = lipValue * p_lip / 100;

            double contempt = browValue + lipValue;
            contempt = contempt > 0 ? contempt : 0;
            model.Emotions["Contempt"] = contempt;

            // print debug-values 
            if (debug)
            {
                output = "Contempt: " + (int)contempt + " Brow: " + (int)browValue + " Lip: " + (int)lipValue + " L: " + (int)temp_left + " R: " + (int)temp_right;
            }

        }
    }
}









