using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mpr_tst.src {
    public partial class CountDown : Form {

        Timer timer;
        public CountDown() {
            InitializeComponent();

            MaximizeBox = false;
            MinimizeBox = false;
            this.ControlBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;

            timer = new Timer();

            this.Opacity = .1;
            timer.Interval = 90;
            timer.Tick += ChangeOpacity;
            timer.Start();
        }

        void ChangeOpacity(object sender, EventArgs e) {
            this.Opacity += .08;
            if (this.Opacity >= 1) {
                timer.Stop();
                this.Close();
            }
        }
    }
}
