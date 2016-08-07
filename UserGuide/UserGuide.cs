using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Start_mpr {
    public partial class UserGuide : Form {

        string info;
        Graphics g;
        Bitmap bm;

        public UserGuide() {
            InitializeComponent();

            info = "ВЫПОЛНЕНИЕ ТЕСТА\nИспытуемому предьявляются в произвольном порядке различные\nфигуры темные на светлом фоне, размер приблизительно 4 на 4 см.\nc возрастающей  скоростью (от 30 до 160 фигур в минуту). Приращение\nтемпа происходит каждые 30 сек.";     
            this.label1.Text = info;
            DrawRectangle();
            DrawEllipse();
            DrawTriangle();
			
			SetSettings.setAllSettings(this);
        }

        private void DrawRectangle() {
            bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bm);
            g.FillRectangle(new SolidBrush(Color.Black), 50, 10, 20, 20);
            pictureBox1.Image = bm;
        }

        private void DrawEllipse() {
            bm = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            g = Graphics.FromImage(bm);
            g.FillEllipse(new SolidBrush(Color.Black), 50, 10, 20, 20); 
            pictureBox2.Image = bm;
        }

        private void DrawTriangle() {
            bm = new Bitmap(pictureBox3.Width, pictureBox3.Height);
            g = Graphics.FromImage(bm);
            g.FillPolygon(new SolidBrush(Color.Black), 
                new Point[] { new Point(60, 10), new Point(75, 30), new Point(45, 30) });
            pictureBox3.Image = bm;
        }
    }
}
