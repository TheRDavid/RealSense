using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /**
     *Measures the percentage value of anger. 
     *@author Tanja 
     */
    class EM_Anger : RSModule
    {
        // Variables for logic

        /**
        * Initializes the EM, setting the debug-flag to true by default
        */
        public EM_Anger()
        {
            debug = true;
        }

        /**
     * Computes the percentage Value of Anger in the current Frame.
     * @param Graphics g for the view
     * */

        public override void Work(Graphics g)
        {
            int percent = 100;
            //Anger 4+5+7+23 --> BrowShift, EyelidTight, LipsTightened

            //percentage Anger
            int p_brow = 65;
            int p_lid = 20;
            int p_lip = 25;



            //brow Value
            double temp_left = model.AU_Values[typeof(AU_BrowShift).ToString() + "_left"];
            double temp_right = model.AU_Values[typeof(AU_BrowShift).ToString() + "_right"];
            double browValue = (temp_left + temp_right) / 2;
            browValue = browValue * -1 * p_brow / percent;

            //lid Value
            temp_left = model.AU_Values[typeof(AU_EyelidTight).ToString() + "_left"];
            temp_right = model.AU_Values[typeof(AU_EyelidTight).ToString() + "_right"];
            double lidValue = temp_left > temp_right ? temp_left : temp_right;
            if (model.Test) lidValue = (temp_left + temp_right) / 2;
            lidValue = lidValue * -1 * p_lid / percent;

            //lip Value
            double lipValue = model.AU_Values[typeof(AU_LipsTightened).ToString()];
            lipValue = lipValue * -1 * p_lip / percent;

            double anger = (browValue > 0 ? browValue : 0) + lidValue + lipValue;
            anger = anger > 0 ? anger : 0;
            model.Emotions[Model.Emotion.ANGER] = anger;

            // print debug-values 
            if (debug)
            {
                output = "Anger: " + (int)anger + ", Brow: " + (int)browValue + ", Lid: " + (int)lidValue + ", lip: " + (int)lipValue;
            }

        }


    }
}
