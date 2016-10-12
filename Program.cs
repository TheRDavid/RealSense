using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

/**
 * @author: David
 */ 
namespace RealSense
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create modules be forehand blubasdasd
            // Test @Rene
            List<RSModule> modules = new List<RSModule>();
            modules.Add(new FaceTrackerModule());
            modules.Add(new HandTrackerModule());
            modules.Add(new SmileModule());
            modules.Add(new FaceTrackModule_Anton());
            modules.Add(new FaceTrackerModule_Tobi());
            Application.Run(new CameraView(640, 480, 30, modules)); // Start View
        }

    }
}
