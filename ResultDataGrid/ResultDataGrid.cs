using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace Start_mpr {
    public partial class ResultDataGrid : Form {	
        public ResultDataGrid(string speedFile, string totalFile) {
            InitializeComponent();			
			
            this.StartPosition = FormStartPosition.CenterScreen;

            splitContainer1.Orientation = 
                  System.Windows.Forms.Orientation.Horizontal;
            splitContainer1.SplitterDistance = 125;
            splitContainer1.SplitterWidth = 6;

            addDataGridSpeedColumns();
            addDgwErrorsColumns();
            addDgwTimesColumns();

            setCommonDataGridSettings(dataGridView1);
            setCommonDataGridSettings(dgwErrors);
            setCommonDataGridSettings(dgwTimes);

            dataGridView1.Paint += dataGridView1_Paint;
            dgwErrors.Paint += dgwErrors_Paint;
            dgwTimes.Paint += dgwTimes_Paint;

            CsvToDataGrid.speedResults(dataGridView1, speedFile);
            CsvToDataGrid.errorResults(dgwErrors, totalFile);
            CsvToDataGrid.timeResults(dgwTimes, totalFile);

            __resizeDataGrid(dataGridView1);
            __resizeDataGrid(dgwErrors);
            __resizeDataGrid(dgwTimes);
        }

        void setCommonDataGridSettings(DataGridView dgw) {
			dgw.AllowUserToAddRows = false;
			
            dgw.ColumnHeadersHeightSizeMode =
                 DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgw.ColumnHeadersHeight =
                        dgw.ColumnHeadersHeight * 2;
            dgw.ColumnHeadersDefaultCellStyle.Alignment =
                 DataGridViewContentAlignment.BottomCenter;

            dgw.CellPainting += dataGridView1_CellPainting;
        }

        void addDataGridSpeedColumns() {
            this.dataGridView1.Columns.Add("figureCount", "скорость");
            this.dataGridView1.Columns.Add("knsRight", "пасcив.");
            this.dataGridView1.Columns.Add("knsActive", "активн.");
            this.dataGridView1.Columns.Add("knsLost", "спутано");
            this.dataGridView1.Columns.Add("latentRight", "верно");
            this.dataGridView1.Columns.Add("latentActive", "активн.");
            this.dataGridView1.Columns.Add("latentLost", "спутано ");
            this.dataGridView1.Columns.Add("motornRight", " верно");
            this.dataGridView1.Columns.Add("motornActive", "активн.");
            this.dataGridView1.Columns.Add("motornLost", "спутано");
            this.dataGridView1.Columns.Add("motornLost", "клики");
        }

        void addDgwErrorsColumns() {
            this.dgwErrors.Columns.Add("", "Пр");
            this.dgwErrors.Columns.Add("", "Лв");
            this.dgwErrors.Columns.Add("", "Пр-Лв");
            this.dgwErrors.Columns.Add("", "Лв");
            this.dgwErrors.Columns.Add("", "Пр");
            this.dgwErrors.Columns.Add("", "Лв-Пр");
        }

        void addDgwTimesColumns() {
            this.dgwTimes.Columns.Add("", "верно");
            this.dgwTimes.Columns.Add("", "активн.");
            this.dgwTimes.Columns.Add("", "спутано ");
            this.dgwTimes.Columns.Add("", "верно");
            this.dgwTimes.Columns.Add("", "активн.");
            this.dgwTimes.Columns.Add("", "спутано ");
        }

        void dataGridView1_Paint(object sender, PaintEventArgs e) {
            string[] headers = { "КНС", "Латентное время", "Mоторное время" };

            __dataGridView_Paint(dataGridView1, 1, 10, 3, e, headers);
        }

        void dgwErrors_Paint(object sender, PaintEventArgs e) {
            string[] headers = { "Сумма ошибок", "Сумма КНС" };

            __dataGridView_Paint(dgwErrors, 0, 6, 3, e, headers);
        }

        void dgwTimes_Paint(object sender, PaintEventArgs e) {
            string[] headers = { "Разность средних (Латентное)", 
                                 "Разность средних (Моторное)" };

            __dataGridView_Paint(dgwTimes, 0, 6, 3, e, headers);
        }

        void __dataGridView_Paint(DataGridView dgw, int start, 
                                int end, int step, PaintEventArgs e, string[] headers) {

            for (int j = start; j < end; ) {
                Rectangle r1 = dgw.GetCellDisplayRectangle(j, -1, true);

                r1.X += 1;
                r1.Y += 1;
                r1.Width = r1.Width * step - step;
                r1.Height = r1.Height / 2 - 2;
                e.Graphics.FillRectangle(new
                   SolidBrush(dgw.ColumnHeadersDefaultCellStyle.BackColor), r1);
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(headers[j / step],
                    dgw.ColumnHeadersDefaultCellStyle.Font,
                    new SolidBrush(dgw.ColumnHeadersDefaultCellStyle.ForeColor),
                    r1,
                    format);
                j += step;
            }
        }

        void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e) {
            if (e.RowIndex == -1 && e.ColumnIndex > -1) {
                e.PaintBackground(e.CellBounds, false);

                Rectangle r2 = e.CellBounds;
                r2.Y += e.CellBounds.Height / 2;
                r2.Height = e.CellBounds.Height / 2;
                e.PaintContent(r2);
                e.Handled = true;
            }
        }
        void __resizeDataGrid(DataGridView dgw) {
            int len = dgw.Columns.Count;

            if (len > 0) {
                int width = (dgw.Width - 45) / len;

                for (int j = 0; j < len; j++)
                    dgw.Columns[j].Width = width;
            }

        }

        private void Form1_ResizeEnd(object sender, EventArgs e) {
            dataGridView1.Width = this.Width - 35;
            dataGridView1.Height = this.Height - 67;
            tabControl1.Width = this.Width - 17;
            tabControl1.Height = this.Height - 41;
            splitContainer1.Width = tabPage2.Width - 9;
            splitContainer1.Height = tabControl1.Height - 53;         
        }

        private void dataGridView1_Resize(object sender, EventArgs e) {
            __resizeDataGrid(dataGridView1);
        }

        private void splitContainer1_Panel1_Resize(object sender, EventArgs e) {
            dgwErrors.Width = splitContainer1.Width;
            dgwErrors.Height = splitContainer1.Panel1.Height - 22;
            dgwTimes.Width = splitContainer1.Width;
            dgwTimes.Height = splitContainer1.Height - dgwErrors.Height - 50;
        }

        private void dgwErrors_Resize(object sender, EventArgs e) {
            __resizeDataGrid(dgwErrors);
            __resizeDataGrid(dgwTimes);
        }

    }

    static class CsvToDataGrid {
        public static void speedResults(DataGridView dr, string file){
            var sr = new StreamReader(File.OpenRead(file), Encoding.Default);
            while (!sr.EndOfStream) 
                __addRow(sr.ReadLine().Split(';'), dr);   
		    sr.Close();
            
        }

        public static void errorResults(DataGridView dr, string file) {
            var sr = new StreamReader(File.OpenRead(file), Encoding.Default);
            __totalResults(sr, dr, "errors");
			sr.Close();
        }

        public static void timeResults(DataGridView dr, string file) {
            var sr = new StreamReader(File.OpenRead(file), Encoding.Default);
            __totalResults(sr, dr, "delay");
			sr.Close();
        }

        private static void __totalResults(StreamReader sr, DataGridView dr, string tableName) {
            string line;
			string[] lines;
                    
            while ((line = sr.ReadLine()) != null) {                    // Переделать !!!!!
                if (line.Contains(tableName)) {
                    
                    while ((line = sr.ReadLine()) != null){
						 lines = line.Split(';');
						 if (lines.Length < 2)
							 return;
						
                        __addRow(lines, dr); 
					}
                }
            }
        }

        private static void __addRow(string[] lines, DataGridView dr) {
            DataGridViewRow row = (DataGridViewRow)dr.RowTemplate.Clone();
			row.CreateCells(dr);

            for (int i = 0; i < lines.Length; i++){
				try{
					row.Cells[i].Value = lines[i];
				}
				catch {}
			}

            dr.Rows.Add(row);
        }
    }
}
