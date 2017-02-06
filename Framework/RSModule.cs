using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RealSense
{
    /**
     * abstract module to implment the Init method of the model and contains the work method
     * 
     * @Author Tanja Witke
     */
    public abstract class RSModule
    {
        protected static Model model;
        protected bool debug = false;

        /**
        * initialise the model
        * @param m it is the model
        */
        public static void Init(Model m)
        {
            model = m;
        }

        /**
         *  Update every frame (do calculations, manipulate output Image)
         *  @param Graphics g
         */
        public abstract void Work(Graphics g);
    }
}
