using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /*
     *Measures if complete brow is raised or lowered (each eye)  - Action Unit Number 4 
     *@author Anton 
     *@date 20.03.2017
     *@HogwartsHouse Slytherin  
     */
    class EM_Anger : RSModule
    {
        // Variables for logic

        // Default values
        public EM_Anger()
        {
            DEF_MIN = -7;
            DEF_MAX = 12;
            reset();
            MIN_TOL = -2;
            MAX_TOL = 1;
            debug = true;
            XTREME_MAX = 300;
            XTREME_MIN = -200;
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
            15 = lip corner lowered -> LipCorner
            16 = lower lip lowered ->LowerLipLowered
            20 = lip stretched -> LipStretched
            23 = lip tightened -> LipsTightened
            26 = jaw drop -> JawDrap
         * */

        public override void Work(Graphics g)
        {
            //Anger 4+5+7+23 --> Brow Lowerer, Upper Lid Raiser, Lid Tightener, Lip Tightener

            //percentage Anger
            int p_brow = 40;
            int p_lid = 30;
            int p_lip = 30;

            //brow Value
            double temp_left = model.getAU_Value(typeof(ME_BrowShift).ToString() + "_left");
            double temp_right = model.getAU_Value(typeof(ME_BrowShift).ToString() + "_right");
            double browValue = temp_left > temp_right ? temp_left : temp_right;
            browValue = browValue * -1 * p_brow / 100; 

            //lid Value
            temp_left = model.getAU_Value(typeof(ME_EyelidTight).ToString() + "_left");
            temp_right = model.getAU_Value(typeof(ME_EyelidTight).ToString() + "_right");
            double lidValue = temp_left > temp_right ? temp_left : temp_right;
            lidValue = lidValue * -1 * p_lid / 100;

            //lip Value
            double lipValue = model.getAU_Value(typeof(ME_LipsTightened).ToString() + "_upperBottomLip");
            lipValue = lipValue * -1 * p_lip / 100;

            double anger = browValue + lidValue + lipValue;
            model.Emotions["Anger"] = anger;

            // print debug-values 
            if (debug)
            {
                output = "Anger: " + anger;
            }

        }
    }
}
