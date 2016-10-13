using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
/**
 * @author: Anton
 * @author: Ne try if it works due to Visual Studio 
 *  you can delete it if you want to, just trying something - cheers buddy 
 */
namespace RealSense
{
    /**
     * Instance of an abstract RealSenseModule (see RSModule.cs)
     * Draws a Rectangle around every Face it finds
     */
    class FaceTrackModule_Anton : RSModule
    {
        public override void Work(Graphics g)
        {
            // Get the number of tracked faces
            Int32 nfaces = model.FaceData.QueryNumberOfDetectedFaces();
        }
    }
}
