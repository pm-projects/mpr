using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;


namespace Start_mpr
{
    public class log : StreamWriter
    {
        public bool debug;          // control indicator

        public
        log(string path, bool mode)
            : base(path, mode)
        {
            debug = false;
            base.WriteLine("\n\n\n\t\t");
            WriteLineTm("application is started\n", 0);
            base.Flush();
        }

        public void
        WriteLineTm(string s, params object[] arr)
        {
            base.Write("[" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "] " + s, arr);
            if (debug)                                               // to see result faster
                base.Flush();
        }
        public override void
        WriteLine(string s, params object[] arr)
        {
            // to write to log file. always    
            base.Write(s, arr);
            if (debug)                                               // to see result faster
                base.Flush();
        }

        public void
        wrLn(string s, params object[] arr)
        {
            // to write to log file if indicator debug is true
            if (debug)
            {
                base.Write("[" + DateTime.Now.ToLongTimeString() + "] " + s, arr);
                base.Flush();
            }
        }

        public void
        close()
        {
            base.WriteLine("\n\t\t\tapplication is closed: {0}: {1}\n",
                               DateTime.Now.ToShortDateString(),
                                  DateTime.Now.ToLongTimeString());
            base.Flush();
            base.Close();
        }

        /*
           public void
           wrEx ( string nm, SAException ex) 
           {
        // to write exeption in log file
                this.WriteLine("{2}: exception has arised: {0} \n\tfrom {1}\n",ex, ex.Source, nm); 
                this.WriteLine(" TargetSite: {0}\n", ex.TargetSite); 
                foreach (SAError er in ex.Errors){
                  this.WriteLine( 
                         "SAError: \n\tmessage: {0}, nativeError : {1}, \n\tsource: {2}, SqlState: {3}\n\n",
                         er.Message, er.NativeError, er.Source, er.SqlState); 
                }
                this.Flush();
           }

           public void
           wrEx ( string nm, DBExceptn ex) {
             wrEx (nm, ex, "DBException");
           }

           public void
           wrEx ( string nm, LoginExceptn ex) {
             wrEx (nm, ex, "LoginException");
           }

           public void
           wrEx ( string nm, ProjExceptn ex) {
             wrEx (nm, ex, "ProjException");
           }
        */
        public void
        wrEx(string txt, Exception ex)
        {
            this.WriteLine("{2}: exception has arised: {0} \n\tfrom {1}\n", ex, ex.Source, txt);
            this.WriteLine(" TargetSite: {0}\n", ex.TargetSite);
            this.Flush();
        }


    }
}