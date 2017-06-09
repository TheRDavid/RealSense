using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using RealSense;
using RealSense.Emotions;

namespace RealSense
{
    /**
     * this ist the main class. It start the model and adds all modules to our Model. 
     * 
     * @author Tanja Witke, David Rosenbusch
     * 
     * Interpretation:         0 = Normal
     *                       100 = Lip up
     */
    class Program
    {

        private enum MODE
        {
            ANALYZE, RUN, TEST
        }

        private static MODE mode = MODE.RUN;

        [STAThread]
        static void Main(string[] args)
        {
            if (mode == MODE.ANALYZE)
            {
                Application.Run(new AnalyzerView());
            }
            else
            {
                Model model = new Model(true);
                RSModule.Init(model);
                // Add ActionUnits

                model.AddModule(new FaceTrackerModule(null));
                model.AddModule(new ME_BrowShift());
                model.AddModule(new ME_EyelidTight());
                model.AddModule(new ME_LipsTightened());
                model.AddModule(new ME_JawDrop());
                model.AddModule(new ME_LipCorner());
                model.AddModule(new ME_LipLine());
                model.AddModule(new ME_LipStretched());
                model.AddModule(new ME_NoseWrinkled());
                model.AddModule(new ME_LowerLipLowered());
                model.AddModule(new ME_UpperLipRaised());
                model.AddModule(new ME_LowerLipRaised());
                //  model.AddModule(new ME_BearTeeth());

                model.AddModule(new EM_Joy());
                model.AddModule(new EM_Anger());
                model.AddModule(new EM_Contempt02());
                model.AddModule(new EM_Disgust());
                model.AddModule(new EM_Fear());
                model.AddModule(new EM_Sadness());
                model.AddModule(new EM_Surprise());

                // Default Modules
                model.AddModule(new Gauge_Module());
                model.AddModule(new FaceRecorder());
                model.AddModule(new EmotionSaver());
                Application.Run(new MultiFormContext(new Form[] { new CameraView(model, mode == MODE.TEST), new EmotionView(model) }));
            }
        }
    }
}
