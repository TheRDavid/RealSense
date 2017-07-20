using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense.Emotions
{

    /*
      *Measures the percentage value of fear. 
      *@author Tanja 
      */
    class EM_Fear : RSModule
    {
        /**
          * Initializes the EM, setting the debug-flag to true by default
          */
        public EM_Fear()
        {
            debug = true;
        }

        /**
      * Computes the percentage Value of fear in the current Frame.
      * @param Graphics g for the view
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









