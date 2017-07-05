using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace RealSense
{
    /**
     * abstract module to implment the Init method of the model and contains the work method and all methods to compute needed values.
     * 
     * @Author Tanja Witke, David Rosenbusch
     */
    public abstract class RSModule
    {
        protected double MIN = 0, MAX = 0, MIN_TOL, MAX_TOL, XTREME_MAX, XTREME_MIN;
        protected static double DEF_MIN, DEF_MAX;
        protected static Model model;
        public String output = "";
        protected bool debug = false;
        protected static int numFramesBeforeAccept = 20;
        protected int framesGathered = 0;
        public int[] triggers = { };

        /**
        * initialise the model
        * @param m it is the model
        */
        public static void Init(Model m)
        {
            model = m;
        }

        /**
         * Custom method when triggered
         **/
        public virtual void keyTrigger(int key)
        {
            //can be overriden
        }

        /**
         *  Update every frame (do calculations, manipulate output Image)
         *  @param Graphics g
         */
        public abstract void Work(Graphics g);

        /**
         * Resets the Min and the Max value.
         * 
         * */
        public void reset()
        {
            MIN = DEF_MIN;
            MAX = DEF_MAX;
        }

        /**
         * Getter of the debug value
         * 
         * */
        public bool Debug
        {
            get { return debug; }
            set { debug = value; }
        }
    }

}
