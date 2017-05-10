using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace RealSense
{
    /**
     * this ist the main class. It start the model and adds all modules to our Model. 
     * 
     * @author Tanja Witke, David Rosenbusch
     */
    class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            Model model = new Model();
            RSModule.Init(model);
            // Add ActionUnits

             model.AddModule(new FaceTrackerModule(null));
            // model.AddModule(new ME_BrowShift());
            // model.AddModule(new ME_EyelidTight());
            // model.AddModule(new ME_LipsTightened());
            // model.AddModule(new EM_Anger());
            // model.AddModule(new ME_JawDrop());
             model.AddModule(new ME_LipCorner());
            // model.AddModule(new ME_LipLine());
            // model.AddModule(new ME_LipStretched());
            // model.AddModule(new ME_LowerLipLowered());
            // model.AddModule(new ME_UpperLipRaised());

            // Default Modules
            model.AddModule(new Gauge_Module());
            Application.Run(new CameraView(model));
        }

    }
}
