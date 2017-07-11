using System.Threading;
using System.Windows.Forms;

namespace RealSense
{
    /*
     * ApplicationContext for managing multiple contexts (windows)
     * @author Tanja 
     */
    public class MultiFormContext : ApplicationContext
    {
        private int openForms;

        /**
         * Initializes an ApplicationContext with a variety of WindowsForms and displays each of them
         * @param Graphics g for the view
         */
        public MultiFormContext(params Form[] forms)
        {
            openForms = forms.Length;

            foreach (var form in forms)
            {
                form.FormClosed += (s, args) =>
                {
                    //When we have closed the last of the "starting" forms, 
                    //end the program.
                    if (Interlocked.Decrement(ref openForms) == 0)
                        ExitThread();
                };

                form.Show();
            }
        }
    }
}