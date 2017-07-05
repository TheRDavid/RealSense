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
     * this ist the main class. It starts the model and adds all modules to our Model. 
     * 
     * @author Tanja Witke, David Rosenbusch
     * 
     *
     */
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            //Application.Run(new AnalyzerView());

            Model model = new Model(true);
            RSModule.Init(model);
            Utilities.Init(model);

            // Add ActionUnits
            model.AddModule(new AU_BrowShift());
            model.AddModule(new AU_EyelidTight());
            model.AddModule(new AU_LipsTightened());
            model.AddModule(new AU_JawDrop());
            model.AddModule(new AU_LipCorner());
            model.AddModule(new AU_LipLine());
            model.AddModule(new AU_LipStretched());
            model.AddModule(new AU_NoseWrinkled());
            model.AddModule(new AU_LowerLipLowered());
            model.AddModule(new AU_UpperLipRaised());
            model.AddModule(new AU_LowerLipRaised());

            model.AddModule(new EM_Joy());
            model.AddModule(new EM_Anger());
            model.AddModule(new EM_Contempt());
            model.AddModule(new EM_Disgust());
            model.AddModule(new EM_Fear());
            model.AddModule(new EM_Sadness());
            model.AddModule(new EM_Surprise());

            // Default Modules
            model.AddModule(new Gauge_Module());
            model.AddModule(new FaceRecorder());

            //Start Application
            Application.Run(new CameraView(model, Model.MODE.RUN));
        }
    }
}

