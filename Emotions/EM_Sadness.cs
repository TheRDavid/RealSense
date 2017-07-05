using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense.Emotions
{
    /*
     *Measures the percentage value of anger. 
     *@author Tanja 
     */
    class EM_Sadness : RSModule
    {
        // Variables for logic
        int percent = 100;

        // Default values
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
            //proportions Sadness
            int p_brow = 0;
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
            double browValue = (temp_left + temp_right) / 2;
            browValue = browValue * p_brow / percent;

            //lid values
            double lidValue = model.AU_Values[typeof(AU_EyelidTight).ToString() + "_left"] + model.AU_Values[typeof(AU_EyelidTight).ToString() + "_right"];
            lidValue *= -1;
            lidValue *= p_lid;
            lidValue /= 100;

            //lipL Value 0 - -100
            double lipLValue = model.AU_Values[typeof(AU_LipLine).ToString()];
            lipLValue = lipLValue * -1 * p_lipL / percent;

            //lipS Value 0 - -100
            double lipSValue = model.AU_Values[typeof(AU_LipStretched).ToString()];
            lipSValue = lipSValue * -1 * p_lipS / percent;


            double lipUp = model.AU_Values[typeof(AU_LowerLipRaised).ToString()] * p_lipUp / 100;

            // Falls Corners durch Disgust, auf 0 setzen
            double hDiff = Utilities.DifferenceByAxis(33, 35, Model.AXIS.Y, false) + Utilities.DifferenceByAxis(39, 37, Model.AXIS.Y, false);

            //sum all and save
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




