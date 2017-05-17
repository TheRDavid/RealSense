using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense.Emotions
{
    class EM_Joy : RSModule
    {
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
            int p_lid = 25;
            int p_lipC = 40;
            int p_lipL = 35;


            //Line: hoch lächeln (60-90) (grinsen höher)
            //Line bei traurig: -40/-50 Streched neutral


            //Lid too tight Vars
            int lidMax = 50;
            int newLid = -10;

            //lid Value 0 - -100 (Grenze bei lidMax)
            double temp_left = model.AU_Values[typeof(ME_EyelidTight).ToString() + "_left"];
            double temp_right = model.AU_Values[typeof(ME_EyelidTight).ToString() + "_right"];
            double lidValue = temp_left > temp_right ? temp_left : temp_right;
            //Lid too tight
            lidValue = temp_left > -lidMax || temp_right > -lidMax ? lidValue : newLid;
            lidValue = lidValue * -1 * p_lid / 100;

            //lipC Value 0 - 100
            temp_left = model.AU_Values[typeof(ME_LipCorner).ToString() + "_left"];
            temp_right = model.AU_Values[typeof(ME_LipCorner).ToString() + "_right"];
            double lipCValue = (temp_left + temp_right) / 2;
            lipCValue = lipCValue * p_lipC / 100;

            //lipL Value 0 - -100
            double lipLValue = model.AU_Values[typeof(ME_LipLine).ToString()];
            lipLValue = lipLValue * p_lipL / 100;

            double joy = lidValue + lipCValue + lipLValue;
            joy = joy > 0 ? joy : 0;
            model.Emotions["Joy"] = joy;

            // print debug-values 
            if (debug)
            {
                output = "Joy: " + (int)joy;
            }

        }
    }
}






