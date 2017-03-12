using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RealSense
{
    /**
     * Short description
     * @author me 
     * @HogwartsHouse Hufflepuff
     */ 
    class AU_TEMPLATE : RSModule
    {

        // variables for logic
        
        private double distance;

        // variables for debugging

        private string debug_message = "message: ";

        /**
         * Sets default-values
         */ 
        public AU_TEMPLATE()
        {
            debug = true;
        }

        /**
         * Detailed description
         * see Images/xyz for a visual description
         */ 
        public override void Work(Graphics g)
        {
            /* calculations */

            distance = 42;

            /* Update value in Model */
            model.setAU_Value(typeof(AU_TEMPLATE).ToString(), distance);

            /* print debug-values */
            if (debug)
            {
                model.View.Debug_Y += 20; // new row
                g.DrawString(debug_message + distance, model.DefaultFont, model.DefaultStringBrush, new Point(0, model.View.Debug_Y));
            }
        }
    }
}
