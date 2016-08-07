using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Start_mpr
{ 
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
		                                                                 
		
        [STAThread]
        static void Main(string[] args)
        {                       
            foreach(string arg in args){
            	if (arg == "-?") {
					MessageBox.Show("Help");
					return;
				}
					
			}
	                        
					
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new Auth());
            Application.Run(new Form1());
        }
    }
}
