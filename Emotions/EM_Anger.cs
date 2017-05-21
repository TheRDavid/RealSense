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
            15 = lip corner lowered -> LipCorner
            16 = lower lip lowered ->LowerLipLowered
            20 = lip stretched -> LipStretched
            23 = lip tightened -> LipsTightened
            26 = jaw drop -> JawDrap
         * */

        public override void Work(Graphics g)
        {
            //Anger 4+5+7+23 --> BrowShift, EyelidTight, LipsTightened

            /**
             * Anleitung: 
             * 1. Welche ME werden benoetigt?
             * 2. Prozenteinfluss der MEs festlegen
             * 3. fuer jede ME folgende Berechnung
             *  3.1 Werte aus dem Model holen
             *  3.2 gegebenenfalls benoetigten Wert berechnen
             *  3.3 bei negativen Werten mit -1 multiplizieren (aufpassen mit den Gegenwerten... diese schliessen die ME allerdings meistens aus und sind daher ok/berechtigt?)
             *  3.4 mit Prozentanteil kombinieren (* Anteil / 100)
             * 4. Werte addieren und ins Model schreiben
             * 
             * */

            //percentage Anger
            int p_brow = 65;
            int p_lid = 20;
            int p_lip = 25;
            int p_nose = 0;

            int nw = (int)model.AU_Values[typeof(ME_NoseWrinkled).ToString()];

            if (nw < -20)
            {
                p_nose = 30;
                p_brow = 55;
                p_lid = 10;
                p_lip = 15;
            }

            //Lid too tight Vars

            int lidMax = 90;
            int newLid = -10;

            //brow Value
            double temp_left = model.AU_Values[typeof(ME_BrowShift).ToString() + "_left"];
            double temp_right = model.AU_Values[typeof(ME_BrowShift).ToString() + "_right"];
            double browValue = temp_left > temp_right ? temp_left : temp_right;
            browValue = browValue * -1 * p_brow / 100;

            //lid Value
            temp_left = model.AU_Values[typeof(ME_EyelidTight).ToString() + "_left"];
            temp_right = model.AU_Values[typeof(ME_EyelidTight).ToString() + "_right"];
            double lidValue = temp_left > temp_right ? temp_left : temp_right;
            //Lid too tight
            lidValue = temp_left > -lidMax || temp_right > -lidMax ? lidValue : newLid;
            lidValue = lidValue * -1 * p_lid / 100;

            //lip Value
            double lipValue = model.AU_Values[typeof(ME_LipsTightened).ToString()];
            lipValue = lipValue * -1 * p_lip / 100;

            //nose Value
            double noseValue = -nw * p_nose / 100;

            // SUBTRACT JOY


            
            // END SUBTRACT JOY

            double anger = (browValue > 0 ? browValue : 0) + lidValue + lipValue + noseValue;
            anger = anger > 0 ? anger : 0;
            model.Emotions["Anger"] = anger;

            // print debug-values 
            if (debug)
            {
                output = "Anger: " + (int)anger + ", Brow: " + (int)browValue + ", Lid: " + (int)lidValue + ", lip: " + (int)lipValue + ", nose: " + (int)noseValue;
            }

        }
    }
}
