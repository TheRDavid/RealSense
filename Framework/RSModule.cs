using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
/**
 * @author: David
 * @author:
 */
namespace RealSense
{
    /**
     * Abstract Module
     */
    abstract class RSModule
    {
        // Reference to globally used SenseManager
        protected PXCMSenseManager senseManager;

        /**
         * Miscarriage
         */
        public void doThings()
        {
            Console.WriteLine("Blub");
        }

        // Initialise 'n stuff
        public abstract void Init(CameraView cv);

        // Update every frame (do calculations, manipulate output Image)
        public abstract void Work(Graphics g);


    }



}
