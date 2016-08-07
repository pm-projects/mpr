
using System;
using System.Net;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions; 
using System.Text; 
using System.IO;
using System.Globalization;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using System.Timers;

using System.Windows.Forms;



using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using mpr_tst.src;

/*
[assembly: AssemblyProduct("mpr_win32")]
[assembly: AssemblyTitle("to test of mobility of thinking")]
*/

namespace mpr_tst{
 public class Program
 /// Класс главной функции 

 {
  const string _nm = "app";
  public static double tnTckL=1.0;
  public static uint bgCnt=0, tnCnt=1;
  public static long startedTick=0;
  public static long prevTick=0;
  public static System.Timers.Timer   big  ; //new System.Timers.Timer(10 * 1000);
  public static System.Timers.Timer   tiny ;//= new System.Timers.Timer(10 * 1000);
  static bool tckChk  = false;
  static bool tckChk1  = false;
  static bool tckChk2  = false;
 [STAThread]


  static int Main(string[] ars) {
    int rc =  CNST.ERR;
    string ret    = "nothing to do";
    string me_    = "main";
    TimeSpan elapsedSpan = new TimeSpan(1);
    string iniF   = "";
    {

  /*    System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly(); 
      System.Version ver = asm.GetName().Version;
      me_ = asm.GetName().Name;
      gVars.name = me_;
      gVars.version = string.Format("{0}.{1}.{2}", ver.Major.ToString(),
                                ver.Minor.ToString(), ver.Build.ToString() ); 
      asm = System.Reflection.Assembly.LoadFrom("./mpr.dll");
      ver = asm.GetName().Version;  
      string dbv =  string.Format("{0}.{1}.{2}", ver.Major.ToString(), ver.Minor.ToString(), ver.Build.ToString());
      gVars.version  += " (dll:"+dbv+")";
  */              gVars.version += "1.0.0";

    }
    
    Thread.CurrentThread.CurrentCulture = new CultureInfo( gVars.cI, false );
    Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";

    for (int i = 0; i < ars.Length; i++){
      if (args.isArgs(ars[i])) {
        if (args.isArgs (ars[i], "?", "h",   "help"))
          goto   error;
        else if (args.isArgs (ars[i], "b", "begin")) {
          i++;
          if (i >= ars.Length ) goto error;
          gVars.start =  (uint)(convert.atoui(ars[i]) /5) *5;
          if (gVars.start < 15)  gVars.start = 15;  
        }
        else if (args.isArgs (ars[i], "c",   "check"))    {
          Program.tckChk = true;
          Program.tckChk1 = false;
          Program.tckChk2 = false;

        }
        else if (args.isArgs (ars[i], "c1",   "check"))    {
          Program.tckChk1 = true;
          Program.tckChk  = false;
          Program.tckChk2 = false;

        }
        else if (args.isArgs (ars[i], "c2",   "check"))    {
          Program.tckChk2 = true;
          Program.tckChk  = false;
          Program.tckChk1 = false;
          if (++i >= ars.Length ) goto error;
          tnTckL =  convert.atod(ars[i]);
        }
        else if (args.isArgs (ars[i], "d",   "debug"))    {
          gVars.debug = true;
        }
        else if (args.isArgs (ars[i], "dev",   "developer"))    {
          gVars.developer = true;
        }
        else if (args.isArgs (ars[i], "e", "end")) {
          i++;
          if (i >= ars.Length ) goto error;
          gVars.end=  (uint)(convert.atoui(ars[i]) /5) *5;             
          if (gVars.end > 80)  gVars.end = 80;  
        }
        else if (args.isArgs (ars[i], "s",   "sound"))  {  
          gVars.sound  = true;
        }
        else if (args.isArgs (ars[i], "v",   "verbose"))  {  
          gVars.verbose  = true;
        }
        else if (args.isArgs  (ars[i], "ver", "version"))  {  
          Console.WriteLine("#" + me_ +
             "("+ gVars.version +") ");
          return 0;
        }
      }
      else 
        iniF = ars[i];
    }

///    if (iniF == null || iniF == "") goto nothing;

      try {

        StreamReader r = new StreamReader  ("data\\mpr.dat", System.Text.Encoding.ASCII, false, 512);
        gVars.figures = new uint [gVars.fgrsSz+1];
        string buf;
        gVars.figures[0] = 0;
        for ( int i = 1; i <= gVars.fgrsSz ;) {
          while (r.EndOfStream == false) {
            buf = r.ReadLine();
            if (buf[0]== '*')
              continue;
            else {
              gVars.figures[i++] = convert.atoui(buf);
              break;
            }
          }
          if (r.EndOfStream) {
            r.Close();
            gVars.lfl.wrLn ( "wrong numbers of figures {0}", i);
            goto nothing;
          }
        }
        r.Close();
        gVars.lfl = new log(gVars.name+".log", true);
        gVars.lfl.debug = gVars.debug;
      }
      catch (Exception ex)      {   
        gVars.lfl.wrLn ( "while opening file {0} exeption is {1}",  iniF, ex);
        goto nothing;
      }


      ret = "";

      gVars.lfl.wrLn ("{0} ({1}) is started;  (debug trace is {2}); start/finish: {3}/{4}\n", 
                       gVars.name, gVars.version, (gVars.debug?"on":"off"), gVars.speed, gVars.end);

      if (Program.tckChk) {
        Program.big  = new System.Timers.Timer(10 * 1000);
        Program.tiny = new System.Timers.Timer(10 * 1000);
        Program.big.Elapsed += new ElapsedEventHandler(Program.bgEvent);
        Program.tiny.Elapsed += new ElapsedEventHandler(Program.tnEvent);

        for (double tickL = 15.10; tickL > 14.950; tickL-=0.010) {
           Program.tiny.Interval = tickL;
           Program.tnCnt = 1;
           Program.tiny.Enabled = false;
           Program.bgCnt = 0;
           Program.big.Enabled = false;

           Program.big.Enabled = true;
           while (bgCnt < 11){
             Thread.Sleep(200);
           }
          gVars.lfl.WriteLineTm( gVars.name + ": result:  tickL/ticks(bg/tn) {0}/{2}/{1}/ {0}*{1}={3}\n", 
              tickL, Program.tnCnt,  Program.bgCnt,
                                              tickL * Program.tnCnt);
        }
      }
      else if (Program.tckChk1) {
        Program.big  = new System.Timers.Timer(10 * 1000);
        Program.tiny = new System.Timers.Timer(10 * 1000);
        Program.big.Elapsed += new ElapsedEventHandler(Program.bgEvent);
        Program.tiny.Elapsed += new ElapsedEventHandler(Program.tnEvent);

        Program.startedTick=0;
        for (int i = 1; i< 2; i++) {
           Program.tiny.Interval = (int)1;
           Program.tnCnt = 1;
           Program.tiny.Enabled = false;
           Program.bgCnt = 0;
           Program.big.Enabled = false;

           Program.big.Enabled = true;
           while (bgCnt < 11){
             Thread.Sleep(100);
           }
        }
      }
      else if (Program.tckChk2) {
        Program.big  = new System.Timers.Timer(10 * 1000);
        Program.tiny = new System.Timers.Timer(10 * 1000);
        Program.big.Elapsed += new ElapsedEventHandler(Program.bgEvent);
        Program.tiny.Elapsed += new ElapsedEventHandler(Program.tnEvent);

        {
           Program.tiny.Interval = tnTckL;
           Program.tnCnt = 1;
           Program.tiny.Enabled = false;
           Program.bgCnt = 0;
           Program.big.Enabled = false;

           Program.big.Enabled = true;
           while (bgCnt < 11){
             Thread.Sleep(100);
           }
          gVars.lfl.WriteLineTm( gVars.name + ": result:  tickL/ticks(bg/tn) {0}/{2}/{1}/ calculated time: {0}*{1}={3} / real time: 15.625*{1}={4}\n", 
                                    tnTckL, Program.tnCnt,  Program.bgCnt,  tnTckL * Program.tnCnt,  15.625 * Program.tnCnt);
        }
      }
      else      {                      // work is here
        gVars.speed = gVars.start;
        gVars.bgCnt   = 0;
        gVars.tnCnt   = 0;
        gVars.big     =  new System.Timers.Timer(2 * 1000);  // 2 secs
        gVars.tiny    =  new System.Timers.Timer(15);  // 
        gVars.rslt    =  new List<pair> ();
        gVars.mwin = new window();
        gVars.big.Elapsed += new ElapsedEventHandler(gVars.mwin.bgEvent);
        gVars.tiny.Elapsed += new ElapsedEventHandler(gVars.mwin.tnEvent);
        Application.Run(gVars.mwin);
        ret = gVars.error;
        if (ret.Length == 0) {
          rc = CNST.OK;
          pair z;

          for ( int x = 0; x < gVars.rslt.Count && gVars.debug; x++){
            z = gVars.rslt[x];
            gVars.lfl.WriteLine( gVars.name + ": fgr/tm[{0}]: {1}/{2} ", x, z.fgr, z.tm);
          }

          StreamWriter o = new StreamWriter (gVars.outFile, false);

          ulong prev = (ulong)gVars.startedTick;
          for (uint i=0, j = gVars.start, sp = gVars.start; i < (uint)gVars.rslt.Count; i++, prev = z.tm ) {
            z = gVars.rslt[(int)i];
            if (z.fgr >= 0) {
              if (j-- >= sp ) {
                o.Write("\nf{0:00}", sp);
              }
              if (j <= 0){
                sp+=5;
                j = sp;
              }
              o.Write("\n{0}", z.fgr);
            }
            else if (z.fgr == -10)
              o.Write(",0_{0:0.00}",  (z.tm-prev)/1000.0);
            else if (z.fgr == -12)
              o.Write(",2_{0:0.00}",  (z.tm-prev)/1000.0);
            else if (z.fgr == -100)
              o.Write(",{0:0.00}",  (z.tm-prev)/1000.0);
            else {
              o.Write("\n#wrong elem: eNo/val/ticks: {0}/{1}/{2}",  i, z.fgr, z.tm);
            }
			
          }
          o.Write("\n#eof");
          o.Close ();
        }
        else 
          rc = CNST.WPAR;
        elapsedSpan = new TimeSpan((long)gVars.startedTick);

      }

     gVars.lfl.WriteLineTm( gVars.name + ": return code/string/secs: {0}/'{1}'/{2:N2} ", 
          rc, ret, elapsedSpan.TotalSeconds);
     gVars.lfl.Close();
     return rc;
  nothing:
     Console.WriteLine( gVars.name + ": return code is {0}, return string is '{1}' ", rc, ret);
     return rc;
   error:
     string msg = "\n utility: "+"\n" + gVars.name + 
       "    [-d]  [-v]  [-ver] [-b xxx] [-e yyy]" +
                  "\nwhere:\n"+
                  "\t -d : to debug \n" +
                  "\t -c : to change tiny tick  from 15.10 to 14.950 with the step -0.010 \n" +
                  "\t -c1 : to check how many  DateTime.Ticks in a 15 msec tick \n" +
                  "\t -c2 ff.gg: to set ff.gg tick \n" +
                  "\t -b xxx: to set start speed,  xxx isin {15, 20, 25, ..., 80}\n" +
                  "\t -e yyy: to set finish speed, yyy isin {15, 20, 25, ..., 80}\n" +
                  "\t -ver : to show version number\n"+
                  "\t -s : to make a sound \n" +
                   " the output file is .mpr.out" +
                   ""
               ;
                  
            MessageBox.Show("usage " + gVars.name + "(" + gVars.version + ")" + msg);

  
     return CNST.CRACH;
  }

  public  static void bgEvent(object source, ElapsedEventArgs e)
  {
    const string _me = _nm+"::bgEvent";
    Program.bgCnt++;
    gVars.lfl.WriteLineTm( "{0}: tickNo bg/tn: {1}/{2}\n", _me, Program.bgCnt, Program.tnCnt);

    if (Program.bgCnt > 11)  {
      Program.tiny.Enabled = false;  
      if(Program.tckChk ||Program.tckChk2)
        gVars.lfl.WriteLineTm( "{0}: stop tickNo bg/tn: {1}/{2}\n", _me, Program.bgCnt, Program.tnCnt);
      Program.big.Enabled  = false; 
      if(Program.tckChk1)
        gVars.lfl.WriteLineTm( "{0}: tickNo tn/wi: {1}/{2}\n", _me,  
            Program.tnCnt,             (DateTime.Now.Ticks-Program.startedTick)/10); 
    }    // to have 100 secs

    if (Program.bgCnt == 1)  {
      Program.tnCnt  = 0;            
      Program.tiny.Enabled = true; 
      Program.startedTick =  DateTime.Now.Ticks;
      Program.prevTick = Program.startedTick/10;
    }
  }

  public  static  void tnEvent(object source, ElapsedEventArgs e)
  {
    const string _me = _nm+"::tnEvent";
    Program.tnCnt++;
    if(Program.tckChk1) {
      gVars.lfl.WriteLine( "{0}: tickNo tn/wi/delta: {1}/{2}/{3}\n", _me,  
          Program.tnCnt,     (DateTime.Now.Ticks-Program.startedTick)/10, 
                (DateTime.Now.Ticks-Program.startedTick)/10-Program.prevTick ); // to make 1 1000000th of a sec
          Program.prevTick = (DateTime.Now.Ticks-Program.startedTick)/10;
    }
  }
}}






                        