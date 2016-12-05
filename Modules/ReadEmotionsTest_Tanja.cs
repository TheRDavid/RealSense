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
            if (anger >= 50)
            {
                Console.WriteLine("anger: " + anger);
            }
        }
    }
}


