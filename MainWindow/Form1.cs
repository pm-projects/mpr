using System;
using System.Reflection;

using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

using System.IO;
using System.Collections.Generic;
using Start_mpr.Sqldb;
using System.Text;
using System.Collections.Specialized;

#pragma warning disable 219
namespace Start_mpr
{
    public partial class Form1 : Form
    {
        SqlManager manager = null;
        public static bool dlgRslt = false;

        const string _nm = "mWin";
		
        int winSz = (gVars.debug) ? 50 : 85;         /// percent of screen
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
        Process pr = null;
        Graphics g;

        public Form1()
        {	
            InitializeComponent();
            Form1.dlgRslt = false;
            if(gVars.lfl == null)
                gVars.lfl = new log(
                  Path.GetFileNameWithoutExtension(new Uri(Assembly.GetEntryAssembly().CodeBase).LocalPath)
                     +".log"                
                , true);

    
            const string _me = _nm + "::MWin";
            gVars.lfl.wrLn("{0} I am here.\n", _me);

            Screen scr = Screen.PrimaryScreen;// Screen.Size;
            ClientSize = new System.Drawing.Size(scr.Bounds.Size.Width * winSz / 100, scr.Bounds.Size.Height * winSz / 100);
			this.Size = new Size(500, 400);
            StartPosition = FormStartPosition.CenterScreen;

            winX = Size.Width;
            winY = Size.Height;
          
            AutoScroll = false;
            Paint += new PaintEventHandler(_paint);
            Resize += new EventHandler(_resize);
            
            
            bPen = new Pen(Color.Black, 6);
            rPen = new Pen(Color.Red, 6);
            gPen = new Pen(Color.Green, 6);
            xPen = new Pen(Color.Silver, 6);
            bBrush = new SolidBrush(Color.Black);
            rBrush = new SolidBrush(Color.Red);
            gBrush = new SolidBrush(Color.Green);
            xBrush = new SolidBrush(Color.Silver);
            xBrush = new SolidBrush(this.BackColor);
            int foo = (int)(100 * gVars.interval * 16), inter = 0;
            gVars.lfl.wrLn("{0} I am here. \n", _me);
            
            //sepResultMI.Click += new EventHandler(_separate);
            totalMI.Click += new EventHandler(_total);
            exitMI.Click += new EventHandler(_app_exit);
            
            classicalTestMI.Click += new EventHandler(_test1);
            shortTestMI.Click += new EventHandler(_test2);

            slowSpeedMI.Click += new EventHandler(_slow);
            middleSpeedMI.Click += new EventHandler(_middle);
            maxSpeedMI.Click += new EventHandler(_max);
            fastSpeedMI.Click += new EventHandler(_fast);
            testMI.Click += new EventHandler(_slow1);

            aboutMI.Click += new EventHandler(_app_about);
            infoMI.Click += new EventHandler(_app_info);
            resultMI.Click += new EventHandler(_app_res);

            //toolStripStatusLabel1.Text += Auth.FirstName + " " + Auth.LastName;
			toolStripStatusLabel1.Text += "Анонимный Аноним";
			Auth.FirstName = "Анонимный";
			Auth.LastName  = "Аноним";

            manager = SqlManager.Instance;
            manager.setConnectionString(@"Data Source=localdb\Result.db;Version=3;");
			
			setLocale(1);
        }

        public void _app_exit(object sender, System.EventArgs e)
        {          
			var confirmResult =  MessageBox.Show("Are you sure?  :(", "Exit",
                                     MessageBoxButtons.YesNo);
									 
			if (confirmResult == DialogResult.Yes) { 
				if (pr != null && !pr.HasExited) pr.Kill();
				Close();
			}
        }
        public void _app_about(object sender, System.EventArgs e)
        {
            MessageBox.Show(
             gVars.name + ". Version:" + gVars.version, "О программе", MessageBoxButtons.OK, MessageBoxIcon.Information
             );
        }
        public void _app_info(object sender, System.EventArgs e) {
            Overwiew infoDialog = new Overwiew();
			infoDialog.Show();
        }

        public void _total(object sender, System.EventArgs e){
								
			_Process.input_result(gVars.outFile, 
					gVars.speedFile, gVars.totalFile, pr);
			
			ResultDataGrid rdg = new ResultDataGrid(gVars.speedFile,
													gVars.totalFile);
			rdg.ShowDialog();
        }

        public void _app_res(object sender, System.EventArgs e){		
		   new UserGuide().ShowDialog();
        }

        public void _slow(object sender, System.EventArgs e)
        {			
            const string _me = _nm + "::_slow";
            gVars.lfl.wrLn("{0} I am here.\n", _me);
            start("-e 20");
			
            if (!pr.HasExited)
                pr.PriorityClass = ProcessPriorityClass.RealTime;

            if (gVars.debug)
            {
                for (int j = 1; j < 10 && !pr.HasExited; j++)
                {
                    Thread.Sleep(1000);
                    gVars.lfl.wrLn("{0} I am here. PriorityClass : {1}\n", _me, pr.PriorityClass);
                }
            }
        }
        public void _slow1(object sender, System.EventArgs e)
        {
            const string _me = _nm + "::_slow";

            start("-e 15");

            if (gVars.debug)
            {
                for (int j = 1; j < 10 && !pr.HasExited; j++)
                {
                    Thread.Sleep(1000);
                    gVars.lfl.wrLn("{0} I am here. PriorityClass : {1}\n", _me, pr.PriorityClass);
                }
            }
        }

        public void _max(object sender, System.EventArgs e) {
            start("-b 80 -e 90");
            this.Focus();
        }

        public void _middle(object sender, System.EventArgs e){
            start("-b 45 -e 80");
            this.Focus();
        }
        public void _fast(object sender, System.EventArgs e){
            start("-b 75 -e 80");
            this.Focus();
        }
        public void _test1(object sender, System.EventArgs e){
            start("-b 15 -e 80");
            this.Focus();
        }

        public void _test2(object sender, System.EventArgs e)
        {
            start("-b 40 -e 80");
            this.Focus();
        }


        public void start(string flags)
        {  // sprintf c#
            //String s = String.Format("{0} -b {1} -e {2}", flags, b, e);
			if (developerMI.Checked)
				flags += " -dev";
			if (soundMI.Checked)
				flags += " -s"; 
			
            if (pr != null && !pr.HasExited) pr.Kill();
            pr = System.Diagnostics.Process.Start("mpr_tst.cs.exe", flags);
            if (!pr.HasExited)
                pr.PriorityClass = ProcessPriorityClass.RealTime;
        }


        public void _resize(Object sender, EventArgs e)
        {
            const string _me = _nm + "::_paint";
            gVars.lfl.wrLn("{0} I am here. \n", _me);
            winX = Size.Width;
            winY = Size.Height;
            Invalidate();
        }
        
        public void _up(Object sender, MouseEventArgs e)
        {    ///
            const string _me = _nm + "::_up";
        }

        public void _paint(Object sender, PaintEventArgs e)
        {
            const string _me = _nm + "::_paint";
            winX = Size.Width;
            winY = Size.Height;
            if (gVars.verbose)
                gVars.lfl.wrLn("{0} I am here. \n", _me);
            curr = rPen;
            g = e.Graphics;
        }
    
        private void writeToDbItem_Click(object sender, EventArgs e)
        {		
            FileStream mprOut = File.Open(gVars.outFile, FileMode.Open, FileAccess.Read);

            BinaryReader br = new BinaryReader(mprOut);
               
            long size = br.BaseStream.Length;
            byte[] buffer = br.ReadBytes((int)size);
			
            manager.POST(buffer);
			
			mprOut.Close(); //???
        }
		
        private void сменитьПользователяToolStripMenuItem_Click(object sender, EventArgs e){
			Auth auth = new Auth();
			if (auth.ShowDialog() == DialogResult.OK)
				toolStripStatusLabel1.Text = String.Format("Пользователь: {0} {1}", 
													        Auth.FirstName, Auth.LastName);
        }

        public static DialogResult get_DialogResult(bool res){
            if (res){
                return DialogResult.OK;
            }else {
                return DialogResult.Cancel;
            }
        }
		
		void DropDown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
		{
            if(e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)  
		    {  
			   e.Cancel = true;  
		    } 
		}
		
        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {		
            manager = SqlManager.Instance;
            DataBaseSettingsForm setting_frm = new DataBaseSettingsForm(manager.getConnectionString());
            if(setting_frm.ShowDialog() == DialogResult.OK){
                manager.setConnectionString(setting_frm.getConnectionString());
            }
        }

        private void прочитатьПоследнийРезультутСБазыДанныхToolStripMenuItem_Click(object sender, EventArgs e){
            ResultList dlg = new ResultList(this.pr);
            dlg.ShowDialog();
        }
		
		private void developerMI_Click(object sender, EventArgs e){
			developerMI.Checked = !developerMI.Checked;
			gVars.developer = developerMI.Checked;
		}
		
		private void soundMI_Click(object sender, EventArgs e){
			soundMI.Checked = !soundMI.Checked;
		}    
		
		private void commonMI_Click(object sender, EventArgs e){
			Settings st = new Settings();
			st.ShowDialog();
			
			SetSettings.setAllSettings(this);
		}
			
		private void enMI_Click(object sender, EventArgs e){
			uncheckLangItems();
			enMI.Checked = true;
			setLocale(0);
		}  
		
		private void ruMI_Click(object sender, EventArgs e){
			uncheckLangItems();
			ruMI.Checked = true;
			setLocale(1);
		}
		
		private void uaMI_Click(object sender, EventArgs e){
			uncheckLangItems();
			uaMI.Checked = true;
			setLocale(2);
		}
		
		private void uncheckLangItems() {
			foreach (ToolStripMenuItem menu in langMItem.DropDownItems) {
				menu.Checked = false;
			}
		}
		
		private void setLocale(int langId) {
			StringDictionary dict = Locale.getDictionaryById(langId);
			
			resultMItem.Text = dict[LocaleKeys.file];
			totalMI.Text = dict[LocaleKeys.lastResult];
			сменитьПользователяToolStripMenuItem.Text = dict[LocaleKeys.changeUser];
			exitMI.Text = dict[LocaleKeys.exit];
			testMItem.Text = dict[LocaleKeys.test];
			classicalTestMI.Text = dict[LocaleKeys.classicalTest];
			shortTestMI.Text = dict[LocaleKeys.shortTest];
			practiceMItem.Text = dict[LocaleKeys.practice];
			slowSpeedMI.Text = dict[LocaleKeys.slowTest];
			middleSpeedMI.Text = dict[LocaleKeys.middleTest];
			fastSpeedMI.Text = dict[LocaleKeys.fastTest];
			результатыToolStripMenuItem.Text = dict[LocaleKeys.results];
			записатьВАрхивToolStripMenuItem.Text = dict[LocaleKeys.writeLastToDB];
			прочитатьПоследнийРезультутСБазыДанныхToolStripMenuItem.Text = dict[LocaleKeys.showResults];
			infoMItem.Text = dict[LocaleKeys.info];
			aboutMI.Text = dict[LocaleKeys.about];
			infoMI.Text = dict[LocaleKeys.theory];
			optionsMItem.Text = dict[LocaleKeys.settings];
			настройкиToolStripMenuItem.Text = dict[LocaleKeys.dataBase];
			developerMI.Text = dict[LocaleKeys.devMode];
			soundMI.Text = dict[LocaleKeys.sound];
			langMItem.Text = dict[LocaleKeys.lang];	
			resultMI.Text = dict[LocaleKeys.userGuide];	
			maxSpeedMI.Text = dict[LocaleKeys.maxSpeedTest];	
			testMI.Text = dict[LocaleKeys.uniformTest];	
			
		}
    }
	
	static class _Process{
		public static void _run_process(ProcessStartInfo startInfo, Process pr){
			startInfo.CreateNoWindow = true;
			startInfo.UseShellExecute = false;
			if (pr != null && !pr.HasExited) pr.Kill();
			pr = System.Diagnostics.Process.Start(startInfo);			
			pr.WaitForExit();
		}
		
		public static void input_result(string inFile, string spFile, string tlFile, Process pr){
			_Process._run_process(new ProcessStartInfo("mprc.exe", 
						 string.Format("-h -r -i {0} -o {1}", 
						 inFile, spFile)), pr);
						 
			_Process._run_process(new ProcessStartInfo("mprc.exe", 
						 string.Format("-g -r -i {0} -o {1}", 
						 inFile, tlFile)), pr);
		}
	}
}
