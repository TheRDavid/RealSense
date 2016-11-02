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
            model.AddModule(new AU_LipsPressedModule());

            Application.Run(new CameraView(model));
        }

    }
}
