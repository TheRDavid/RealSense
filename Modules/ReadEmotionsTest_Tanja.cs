using System.Text;
using System.Drawing;
using System;

namespace RealSense
{
    class ReadEmotionsTest_Tanja : RSModule
    {
        private int anger;

        public override void Work(Graphics g)
        {
            //if (model.Anger == null) return;
            anger = model.Anger[0] * 100 + model.Anger[1] * 10 + model.Anger[2];
            Console.WriteLine(anger);
            if (anger > 100)
            {
                Console.WriteLine("anger: " + anger);
            }
        }
    }
}


