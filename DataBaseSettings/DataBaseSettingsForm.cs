using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Start_mpr
{
    public partial class DataBaseSettingsForm : Form
    {
        public DataBaseSettingsForm(String connection_string){
            InitializeComponent();
            textBox1.Text = connection_string;		
			textBox1.Enabled = gVars.developer;
			this.button1.Enabled = gVars.developer;
        }

        public string getConnectionString(){
            return textBox1.Text;
        }
    }
}
