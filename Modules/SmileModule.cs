using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RealSense
{
    /* Shows how much you are smiling */
    class SmileModule : RSModule
    {
        private PXCMFaceData.ExpressionsData.FaceExpressionResult score;

        public override void Work(Graphics g)
        {
            if (model.Edata != null)
            {
                model.Edata.QueryExpression(PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_SMILE, out score);
                g.DrawString("Smile: " + score.intensity, new Font("Arial", 16), new SolidBrush(Color.Black), new Rectangle(150, 150, 200, 50));
            }
        }
    }
}