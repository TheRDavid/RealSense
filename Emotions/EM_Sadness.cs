using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense.Emotions
{

    /*
    *Measures the percentage value of sadness. 
    *@author Tanja 
    */
    class EM_Sadness : RSModule
    {
        /**
        * Initializes the EM, setting the debug-flag to true by default
        */
        public EM_Sadness()
        {
            debug = true;
        }


        /**
         * Computes the percentage Value of Sadness in the current Frame.
         * @param Graphics g for the view
         * */
        public override void Work(Graphics g)
        {
            //Sadness --> BrowShift, LipLine, (LipStreched), EyelidTight

            //percentage Sadness
            int p_brow = 0;
            //int p_lid = 50;
            int p_lipL = 60;
            int p_lipUp = 0;
            int p_lipS = 10;
            int p_lid = 40;

            int cornerPos = (int)(model.AU_Values[typeof(AU_LipCorner).ToString() + "_left"] + model.AU_Values[typeof(AU_LipCorner).ToString() + "_right"]) / 2;

            if (cornerPos > 10)
            {
                p_lipL = 70;
                p_lipUp = 20;
                p_lid = 20;
            }

            //brow Value 0-100
            double temp_left = model.AU_Values[typeof(AU_BrowShift).ToString() + "_left"];
            double temp_right = model.AU_Values[typeof(AU_BrowShift).ToString() + "_right"];
            double browValue = temp_left < temp_right ? temp_left : temp_right;
            if (model.Test) browValue = (temp_left + temp_right) / 2;
            browValue = browValue * p_brow / 100;

            //lid values
            double lidValue = model.AU_Values[typeof(AU_EyelidTight).ToString() + "_left"] + model.AU_Values[typeof(AU_EyelidTight).ToString() + "_right"];
            lidValue *= -1;
            lidValue *= p_lid;
            lidValue /= 100;

            //lipL Value 0 - -100
            double lipLValue = model.AU_Values[typeof(AU_LipLine).ToString()];
            lipLValue = lipLValue * -1 * p_lipL / 100;

            //lipS Value 0 - -100
            double lipSValue = model.AU_Values[typeof(AU_LipStretched).ToString()];
            lipSValue = lipSValue * -1 * p_lipS / 100;


            double lipUp = model.AU_Values[typeof(AU_LowerLipRaised).ToString()] * p_lipUp / 100;

            // Falls Corners durch Disgust, auf 0 setzen
            double hDiff = model.DifferenceByAxis(33, 35, Model.AXIS.Y, false) + model.DifferenceByAxis(39, 37, Model.AXIS.Y, false);

            if (hDiff < 0)
            {
                lipLValue = 0;
            }


            double sad = lidValue + lipLValue;
            sad = sad > 0 ? sad : 0;
            model.Emotions[Model.Emotion.SADNESS] = sad;


            // print debug-values 
            if (debug)
            {
                output = "Sadness: " + (int)sad + " LipL: " + (int)lipLValue + " Lid: " + (int)lidValue + " LipS: " + (int)lipSValue + " LipUp: " + (int)lipUp + " Brow: " + (int)browValue + " CornerP: " + (int)cornerPos;
            }

        }
    }
}




