using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace RealSense
{
    class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            Model model = new Model();
            RSModule.Init(model);
            // Create modules beforehand
            //model.AddModule(new LandmarkGroupModuleDavid());
            model.AddModule(new FaceTrackerModule(null));
            //model.AddModule(new AU_LipsPressedModule_David());
            //model.AddModule(new AU_EyelidTightModul_Anton());
            model.AddModule(new AU_LipsThicknessModul_Tobi());
        //   model.AddModule(new Modules.AU_MouthRect_Rene()); //Warum muss ich Modules.modulname schreiben?

            Application.Run(new CameraView(model));
        }

    }
}
