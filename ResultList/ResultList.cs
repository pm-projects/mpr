using Start_mpr.Sqldb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Start_mpr
{
    public partial class ResultList : Form
    {	
        SqlManager manager = null;
        ArrayList array;
        Process pr = null;
        private int selected_row = -1;

        public ResultList(Process pr) {
			
            InitializeComponent();
            this.pr = pr;
			this.StartPosition = FormStartPosition.CenterScreen;
			
            manager = SqlManager.Instance;
            array = manager.GET();					// Arihmetic overflow exception
			
            foreach(MyResult res in array) {
                dataGridView1.Rows.Add(new object[] { res.fio, res.date.ToString() });
            }
			
            if(array.Count == 0)
            {
                button1.Enabled = false;
                button2.Enabled = false;
            }
			
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e){
            selected_row = dataGridView1.CurrentRow.Index;
        }

        private void button2_Click(object sender, EventArgs e) {
            manager.DELETE_BY_ID(((MyResult)array[selected_row]).id);
            dataGridView1.Rows.RemoveAt(selected_row);
        }

        private void button3_Click(object sender, EventArgs e){
            manager.DELETE_ALL();
        }

        private void button1_Click(object sender, EventArgs e)
        {	
            string path = string.Format("{0} \\{1}", 
			    Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), gVars.tmpOutFile);
            
            if(File.Exists(path))
                File.Delete(path);
            Stream s = File.Open(path, FileMode.Create, FileAccess.Write);

            BinaryWriter bw = new BinaryWriter(s, Encoding.ASCII);
            bw.Write(((MyResult)array[selected_row]).result);

            bw.Flush();
            bw.Close();
			
			_Process.input_result(gVars.tmpOutFile, 
					gVars.tmpSpeedFile, gVars.tmpTotalFile, pr);
			
			ResultDataGrid rdg = new ResultDataGrid(gVars.tmpSpeedFile,
													gVars.tmpTotalFile);
			rdg.ShowDialog();
        }
    }

    public class MyResult{
        public int id { get; set; }
        public string fio { get; set; }
        public byte[] result { get; set; }
        public DateTime date { get; set; }
    }
}
