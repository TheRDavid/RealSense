using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RealSense
{
    /**
     * This class is the (AU)Autounfall scowled eyebrows class.
     * Which checks the distance between the two eyebrows and the relativ distance with the nose.
     *  
     * @author Tanja Witke
     */
    class AU_ScowledBrows : RSModule
    {
        private Font arialFont = new Font("Arial", 18);
        private Brush redBrush = new SolidBrush(Color.Red);


        /** 
        * @Override 
        * calculates the position of the eybrows if they are closer together. 
        * It is calculated relativ to the distance with the nose. 
        */
        public override void Work(Graphics g)
        {
            //g.DrawString(model.difference(0,29).ToString(), font, stringBrush, new PointF(20, 60));

            double augenbrauenAbstand = model.Difference(0, 5);
            double augenBraueNaseAbstand = model.Difference(0, 29);

            if (augenbrauenAbstand < 99 && augenBraueNaseAbstand < 90)
            {
                model.Emotions[Model.ANGER] += 50;
            }                
        }
    }
}
