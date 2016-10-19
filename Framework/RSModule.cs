using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RealSense
{
    /**
     * Abstract Module
     */
    public abstract class RSModule
    {
        protected static Model model;

        public static void Init(Model m)
        {
            model = m;
        }

        // Update every frame (do calculations, manipulate output Image)
        public abstract void Work(Graphics g);
    }
}
