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
            model.AddModule(new FaceTrackerModule());
            model.AddModule(new HandTrackerModule());
            model.AddModule(new SmileModule());
            model.AddModule(new FaceTrackerModule_Tobi());
            model.AddModule(new FaceTrackModule_Anton());

            Application.Run(new CameraView(model));
        }

    }
}
