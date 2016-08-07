using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections; 
using System.Timers;
using System.Threading;
using System.Media;

//using System.Collections.Specialized;
using System.Collections.Generic;

#pragma warning disable 219

namespace mpr_tst.src
{

  public  class window : Form               /// окно для отображения 
  {
     const string _nm = "window";


     const int margin = 10;                /// поля между отображением и границами окна
     int winSz  =  (gVars.debug)?50:85;         /// percent of screen
     public static  int ticks= 0;            /// how many ticks in a sec.
     int winX = 0;
     int winY = 0;
     Pen bPen;
     Pen rPen;
     Pen gPen;
     Pen xPen;
     Pen curr;
     Brush bBrush;
     Brush rBrush;
     Brush gBrush;
     Brush xBrush;

//     uint counter  = 0;
//     uint counter2  = 0;
     uint figure   = 0;
     uint color    = 0;
     int sz = 10;
     Graphics g ;
     Rectangle rc;
     static Brush curBrush;
     static Point [] plgn = null;
     uint demoTm = 128;   // full  time for figure
     uint noImage = 9;    // 
     uint adjustment = 0;    //       64 * 30 = 1920 ticks
                            //    if temp == 25 then demoTm = 76.8 ticks 20 ticks will be losted
                            // so I increase demoTm for first 20 demo for 1 tick

     uint yesImage = 128 - 9;    // 

     uint spdFgr  = 10;
     bool tnWork  = true;
     static pair p;
     static int   oldKeyDown = -100;
     static bool wrongKey = false;
	 
	 static SoundPlayer errorSound = new SoundPlayer(@"sounds/error.wav");
	 
	 CountDown cd; 				// Форма счетчика в начале

    public window()
    {
      const string _me = _nm+"::window";
      Screen scr  = Screen.PrimaryScreen;// Screen.Size;
      ClientSize = new System.Drawing.Size(scr.Bounds.Size.Width* winSz / 100, scr.Bounds.Size.Height* winSz / 100);
      StartPosition = FormStartPosition.CenterScreen;

      MaximizeBox = false;
      MinimizeBox = false;
      ControlBox = false;
      FormBorderStyle =  FormBorderStyle.FixedDialog;

      winX =  Size.Width;
      winY = Size.Height;
      rc = new Rectangle   (winX/2 - winX/sz, winY/2 - winY/sz, winX/2 + winX/sz, winY/2 + winY/sz);

      demoTm = (uint) (gVars.ticks * 30) / gVars.speed;
      yesImage = demoTm - noImage;  // aroung 0.1. 0.015625 * 7
      AutoScroll    = false;
      Paint   +=  new PaintEventHandler (_paint);
      Resize  +=  new EventHandler      (_resize);
      MouseUp +=  new MouseEventHandler (_up);
      KeyDown +=  new KeyEventHandler   (window._KeyDown);
      KeyUp   +=  new KeyEventHandler   (window._KeyUp);


      bPen = new Pen(Color.Black, 6);
      rPen = new Pen(Color.Red, 6);
      gPen = new Pen(Color.Green, 6);
      xPen = new Pen(Color.Silver, 6);
      bBrush = new SolidBrush (Color.Black);  
      rBrush = new SolidBrush (Color.Red );  
      gBrush = new SolidBrush (Color.Green);  
      xBrush = new SolidBrush (Color.Silver);  
      xBrush = new SolidBrush (this.BackColor);  
      int foo = (int)(100* gVars.interval * 16), inter = 0  ;
      gVars.lfl.wrLn ("{0}. start/finish/noImage/foo/error: {1}/{2}/{3}/{4}/'{5}'\n", _me, 
                gVars.speed, gVars.end, noImage, foo, gVars.error);

      for (int x = 3; x > 0; x--) {	
		
		if (gVars.sound)
			Console.Beep();
		
		cd = new CountDown();
		cd.count.Text = x.ToString();
		cd.ShowDialog();
		
        //Thread.Sleep(1000 - foo);
      }
	  
      gVars.startedTick =  DateTime.Now.Ticks;
      gVars.lfl.wrLn ("{0} I am here. start/finish/noImage: {1}/{2}/{3}\n", _me, 
                gVars.speed, gVars.end, noImage);
      {
        gVars.big.Enabled = true;
        spdFgr = 0;
        gVars.tnCnt  = 0;            
        demoTm = (uint) (gVars.ticks * 30) / gVars.speed;
        adjustment = (uint)  ((gVars.ticks * 30) -  (gVars.speed*demoTm));
        if (gVars.verbose)
          gVars.lfl.wrLn ("{0} I am here. start of speed/demotime : {1}/{2}\n", _me, gVars.speed, demoTm);
        gVars.tiny.Enabled = true; 
      }
    }

    public   void bgEvent(object source, ElapsedEventArgs e)
    {
      const string _me = _nm+"::bgEvent";
      gVars.bgCnt++;
      if (gVars.bgCnt >= 15 || spdFgr >= gVars.speed)  {
        tnWork = false;
        gVars.bgCnt = 0;
        gVars.lfl.wrLn ("{0} I am here. end of speed/figures for speed/ticks: {1}/{2}/{3:#.###}\n", 
                           _me, gVars.speed, spdFgr , (DateTime.Now.Ticks-gVars.startedTick)/10000);
        gVars.speed += 5;

        if (gVars.speed > gVars.end) {
          gVars.tiny.Enabled = false;
          gVars.big.Enabled = false;
          gVars.error = "";
          color = 0;
          Invalidate(rc);
          for (int x = 0; x < demoTm; x++) {  // one half of sec
            if (p.fgr < 0) {
              gVars.rslt.Add (p);
              p = new pair();
            }
            Thread.Sleep((int)gVars.tickL);
          }
          gVars.startedTick =  DateTime.Now.Ticks - gVars.startedTick;
          this.Close();
        }
        spdFgr = 0;
        color=0;
        gVars.tnCnt  = 0;            
        demoTm = (uint) (gVars.ticks * 30) / gVars.speed;
        adjustment = (uint)  ((gVars.ticks * 30) -  (gVars.speed*demoTm));
        yesImage = demoTm - noImage;  // aroung 0.1. 0.015625 * 7
        if (gVars.debug)
          gVars.lfl.wrLn ("{0} I am here. start of speed/demotime/adjustment : {1}/{2}/{3}\n",
                  _me, gVars.speed, demoTm, adjustment);
        tnWork = true;
      }
    }

    public   void tnEvent(object source, ElapsedEventArgs e)
    {
      const string _me = _nm+"::tnEvent";
      if (tnWork) {
        if (p.fgr < 0 ) {
          gVars.lfl.wrLn ("{0}: figure last/current: {1}/{2}\n", 
                 _me, gVars.rslt[gVars.rslt.Count-1].fgr, p.fgr );
            gVars.rslt.Add (p);
            p = new pair();
        }

        gVars.tnCnt  ++;      
/*
        if (gVars.verbose==false && gVars.bgCnt == 0 && gVars.tnCnt == 1) {
          gVars.lfl.wrLn ("{0} I am here. start of speed/demotime/ticks : {1}/{2}/{3:#.###}\n", 
                 _me, gVars.speed, demoTm, (DateTime.Now.Ticks-gVars.startedTick)/10000);
        }
*/
        if (gVars.tnCnt < yesImage) {
          if (color == 0) {
            color = 1;
            if (++spdFgr <= gVars.speed) {
              figure++;
              Invalidate(rc);
              p = new pair();
              p.fgr = (short) gVars.figures[figure];
              p.tm  = (ulong) (DateTime.Now.Ticks-gVars.startedTick)/10000; // to make one of hundreth
              gVars.rslt.Add (p);
              gVars.lfl.wrLn ("{0} I am here. add pair, result count/figure/ticks: {1}/{2}/{3}\n", 
                                  _me, gVars.rslt.Count, p.fgr, (DateTime.Now.Ticks-gVars.startedTick)/10000);
              p = new pair();
            }
          }
        }
        else  {
          if (color != 0) {
            color = 0;
            if (spdFgr <= gVars.speed)  {
              Invalidate(rc);
            }
            figure%=gVars.fgrsSz;  // to start the sequence  again
          }
        }

        if (gVars.tnCnt >= demoTm + ((spdFgr>adjustment)?0:1)   ) {
          if (gVars.debug)
            gVars.lfl.wrLn ("{0}: tickNo/fNo/figure: {1}/{2}/{3}\n", _me, gVars.tnCnt, figure, gVars.figures[figure]);
          gVars.tnCnt %=demoTm;
        }
      }
    }

   [System.Runtime.InteropServices.DllImport("user32.dll")]
   private static extern short GetAsyncKeyState(Keys vKey);

   static public void 
   _KeyDown (object sender, KeyEventArgs e)
   {
     string _me = _nm+"::_KeyDown";
     if (e.KeyCode == Keys.ShiftKey ) {
       if (Convert.ToBoolean(GetAsyncKeyState(Keys.LShiftKey))) {
         if (oldKeyDown != -10) {
           p.fgr = (short) -10;
           p.tm  = (ulong) (DateTime.Now.Ticks-gVars.startedTick) / 10000; // to make one of hundreth
         }
         oldKeyDown = -10;
       }
       if (Convert.ToBoolean(GetAsyncKeyState(Keys.RShiftKey))){
         if (oldKeyDown != -12) {
           p.fgr = (short) -12;
           p.tm  = (ulong) (DateTime.Now.Ticks-gVars.startedTick)/10000; // to make one of hundreth
           gVars.lfl.wrLn ("{0} I am here. ticks: {1}-{2}/10000 = {3}\n", _me, DateTime.Now.Ticks,
              gVars.startedTick, p.tm);
         }
         oldKeyDown = -12;
       }
     }
     else if (e.KeyCode == Keys.Escape) {
           gVars.lfl.wrLn("{0}.Esc: \n", _me);
           gVars.error = "esc";
           gVars.mwin.Close();
     }
     else {		 
		if (gVars.sound) 
			errorSound.Play();	
		
        wrongKey = true;
        oldKeyDown = -100;
     }
   }
   static public void 
   _KeyUp (object sender, KeyEventArgs e)
   {
     string _me = _nm+"::_KeyUp";
     gVars.lfl.wrLn ("{0}: wrongKey {1}\n", _me, wrongKey);
     if (wrongKey)  {
       wrongKey = false;
     }
     else
     {
       p.fgr = (short) -100;
       p.tm  = (ulong) (DateTime.Now.Ticks-gVars.startedTick)/10000; // to make one of hundreth
     }
     oldKeyDown = -100;
   }

    public void _resize (Object sender, EventArgs e) {
      const string _me = _nm+"::_paint";
      gVars.lfl.wrLn ("{0} I am here. \n", _me);
      winX =  Size.Width;
      winY = Size.Height;
      rc = new Rectangle   (winX/2 - winX/sz, winY/2 - winY/sz, winX/2 + winX/sz, winY/2 + winY/sz);
      Invalidate(rc);
    }


    public void _up (Object sender, MouseEventArgs e) {    ///
      const string _me = _nm+"::_up";
    }

    public void _paint (Object sender, PaintEventArgs e) {
      const string _me = _nm+"::_paint";
      winX = Size.Width;
      winY = Size.Height;
      if (gVars.verbose)
        gVars.lfl.wrLn ("{0} I am here. tickNo/figure: {1}/{2}\n", _me, gVars.tnCnt, figure);
      curr = rPen;
      g =  e.Graphics;
      putImage  (gVars.figures[figure], color);
	  
	 // if (gVars.sound) Console.Beep();
    }


    public void putImage (uint figure, uint c){
      const string _me = _nm+"::putImage";
      if (gVars.debug)
        gVars.lfl.wrLn ("{0} I am here. color/figure/tick/ticks: {1}/{2}/{3}/{4} \n", _me, c, figure, gVars.tnCnt, 
                            (DateTime.Now.Ticks-gVars.startedTick)/10000);
      if (plgn == null) {
          plgn = new Point[3];
          plgn[0] = new Point (winX/2, winY/2);
          plgn[1] = new Point (winX/2 + winX/20, winY/2+winY/25);
          plgn[2] = new Point (winX/2-winX/20, winY/2-winY/25);
      }

      int x=winX/2- (winX/sz), y=winY/2 - (winY/sz), w=winX/(sz/2), h =winY/(sz/2);
      if (c == 0) {
        curBrush = xBrush;
        g.FillRectangle (curBrush, x,  y, w, h); 
      }
      else {
				 
        curBrush = bBrush;
        switch (figure) {
          case 0: 
            g.FillEllipse(curBrush, x,  y, w, h);     break;
          case 1: 
            plgn[0] = new Point ( winX/2,              winY/2-winY/sz);
            plgn[1] = new Point ( winX/2 + winX/sz,    winY/2+winY/sz);
            plgn[2] = new Point ( winX/2 - winX/sz,    winY/2+ winY/sz );
            g.FillPolygon (curBrush, plgn);     break;
          case 2: 
            g.FillRectangle (curBrush, x,  y, w, h);  break;
          default:
            g.FillEllipse(rBrush, x,  y, w, h);     break;
        }
		
		if (gVars.sound)
			Console.Beep();	
		
        if (gVars.debug || gVars.developer)
          g.DrawString(gVars.speed.ToString() +"/"+ spdFgr.ToString() , new Font("Times New Roman", 20), rBrush, winX/2 - winX/sz, winY/2-winY/sz);	
      }
    }
}}
