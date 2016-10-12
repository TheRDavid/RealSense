using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

/**
 * @author: Tanja
 * @author:
 */ 
namespace RealSense
{
    /* Shows how much you are smiling */
    class SmileModule : RSModule
    {
        private PXCMSenseManager senseManager;
        private PXCMFaceModule face;
        private PXCMHandModule hand;
        private PXCMHandConfiguration handConfig;
        private PXCMFaceConfiguration faceConfic;
        private PXCMHandData handData;
        private PXCMFaceData faceData;
        private PXCMHandData.GestureData gestureData;
        private bool handWaving;
        private bool smiling;
        private PXCMFaceData.ExpressionsData.FaceExpressionResult score;

        public override void Init(CameraView cv)
        {
            senseManager = cv.SenseManager;
            hand = (PXCMHandModule) cv.CreatePXCMModule(PXCMHandData.CUID);
            face = (PXCMFaceModule) cv.CreatePXCMModule(PXCMFaceData.CUID);
            Console.WriteLine("FaceTracker_Tanja: " + face.GetHashCode());
            Console.WriteLine("HandeTracker_Tanja: " + hand.GetHashCode());
            handConfig = hand.CreateActiveConfiguration();
            handConfig.EnableGesture("wave");
            handConfig.EnableAllAlerts();
            handConfig.ApplyChanges();
            faceConfic = face.CreateActiveConfiguration();
            faceConfic.QueryExpressions();
            faceConfic.EnableAllAlerts();
            faceConfic.ApplyChanges();
            PXCMFaceConfiguration.ExpressionsConfiguration expc = faceConfic.QueryExpressions();
            expc.Enable();
            expc.EnableAllExpressions();
            faceConfic.ApplyChanges();
        }

        public override void Work(Graphics g)
        {
            // Retrieve gesture data
            hand = senseManager.QueryHand();
            face = senseManager.QueryFace(); //


            if (hand != null)
            {
                // Retrieve the most recent processed data
                handData = hand.CreateOutput();
                handData.Update();
                handWaving = handData.IsGestureFired("wave", out gestureData);
            }


            if (face != null)
            {
                faceData = face.CreateOutput();
                faceData.Update();

                //surching faces 
                Int32 nfaces = faceData.QueryNumberOfDetectedFaces();
                for (Int32 i = 0; i < nfaces; i++)
                {

                    // Retrieve the data instance
                    PXCMFaceData.Face faceI = faceData.QueryFaceByIndex(i);
                    PXCMFaceData.ExpressionsData edata = faceI.QueryExpressions();

                    if (edata != null)
                    {
                        edata.QueryExpression(PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_SMILE, out score);
                        if (score.intensity >= 25)
                        {
                            smiling = true;
                        }
                        else smiling = false;

                        Console.WriteLine(i + ": " + score.intensity);
                    }
                }
                faceData.Dispose();
            }
        }
    }
}