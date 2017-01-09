using System.Text;
using System.Drawing;
using System;

namespace RealSense
{
    /**
     * This class was so far just for our Presentation. 
     * it shows the basic concept of how we want to do it. 
     * 
     * @author Tanja Witke
     */ 
    class ReadEmotionsTest_Tanja : RSModule
    {
        private Font font = new Font("Arial", 18);
        private SolidBrush stringBrush = new SolidBrush(Color.Red);

        public override void Work(Graphics g)
        {
            model.Emotions[Model.ANGER] += 50;
            for (int i = 0; i < model.eNames.Length; i++)
            {
                int e = model.Emotions[i];
                g.DrawString(model.eNames[i] + ": " + (e <= 100 ? e : 100), font, stringBrush, new PointF(20, 50 + i * 50));
            }
            
        }
    }
}


