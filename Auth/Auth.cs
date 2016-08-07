using System;
using System.Windows.Forms;

using System.IO;

namespace Start_mpr
{
    public partial class Auth : Form
    {
        public static string FirstName;
        public static string LastName;

        public Auth()
        {
            InitializeComponent();
			
            GetUsersList();
            comboBox1.Text = comboBox1.Items[0].ToString();
        }

        private void GetUsersList()
        {
            FileStream file = File.Open("./list.profile", FileMode.Open);
            using (StreamReader sr = new StreamReader(file))
            {
                string buf = "";
                comboBox1.Items.Clear();
                while ((buf = sr.ReadLine()) != null)
                {
                    string[] user = buf.Split(':');
                    comboBox1.Items.Add(String.Format("{0} {1}", user[0], user[1]));
                }
            }
            file.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FirstName = textBox1.Text;
            LastName = textBox2.Text;

            if (FirstName == "" || LastName == "")
            {
                toolStripStatusLabel1.Text = "Заполните все поля!";
                return;
            }

            FileStream file = File.Open("./list.profile", FileMode.Append);
            using (StreamWriter sw = new StreamWriter(file))
            {
                sw.WriteLine(String.Format("{0}:{1}", FirstName, LastName));
            }
            file.Close();
            this.Visible = false;
            Form1 frm1 = new Form1();

            var result = frm1.ShowDialog();

            
            if (Form1.get_DialogResult(Form1.dlgRslt) == System.Windows.Forms.DialogResult.OK)
            {
                this.Visible = true;
                toolStripStatusLabel1.Text = "Выбирете свое имя из списка или создайте новое.";
            }
            else 
            {
                Application.Exit();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                this.Height += 90;
                panel1.Enabled = false;
                
                button1.Enabled = true;
            }
            else
            {
                this.Height -= 90;
                panel1.Enabled = true;
                button1.Enabled = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
			this.DialogResult = DialogResult.OK;
			
            string buf = comboBox1.SelectedItem.ToString();
            string[] user = buf.Split(' ');
            FirstName = user[0];
            LastName = user[1];
            this.Close();
			
			
			
            /*this.Visible = false;
            Form1 frm1 = new Form1();
            var result = frm1.ShowDialog();

            if (Form1.get_DialogResult(Form1.dlgRslt) == System.Windows.Forms.DialogResult.OK)
            {
                this.Visible = true;
                toolStripStatusLabel1.Text = "Выбирете свое имя из списка или создайте новое.";
            }
            else
            {
                Application.Exit();
            }*/
        }

        
    }
}
