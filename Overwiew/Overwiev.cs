using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Start_mpr {
    public partial class Overwiew : Form {
        public Overwiew() {
            InitializeComponent();

            fillInfo();
			richTextBox1.ReadOnly = true;
			this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void fillInfo() {
            string path = "./data/info";

            if (File.Exists(path)) {
                richTextBox1.Text = File.ReadAllText(path, Encoding.Default);
            }
        }
    }
}
