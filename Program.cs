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
            //model.AddModule(new AU_NoseWrinkled());
            //model.AddModule(new AU_BrowLowered());
            //model.AddModule(new AU_LipStretched());
            //model.AddModule(new AU_LowerLipLowered());
            //model.AddModule(new AU_UpperLipRaised());
            model.AddModule(new AU_LipCorner());

            // Default Modules
            model.AddModule(new Gauge_Module());
            model.AddModule(new FaceTrackerModule(null));
            Application.Run(new CameraView(model));
        }

    }
}
