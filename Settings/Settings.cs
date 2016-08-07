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
    public partial class Settings : Form {

        private Dictionary<string, TableLayoutPanel> layouts;

        public Settings() {
            
            InitializeComponent();
            //
            //Events
            //
            comboFontFont.SelectedIndexChanged += ChangeFontLabel;
            numFontSize.ValueChanged += ChangeFontLabel;
            tree.NodeMouseClick += tree_NodeMouseClick;
            addApplyEventHandlers(this);
            //
            //
            //
            FontOperations.fillComboBox(comboFontFont);
            //
            //
            //
            layouts = new Dictionary<string, TableLayoutPanel>();

            setLayoutVisability();
            fillLayoutDictionary();

			SetSettings.setAllSettings(this);
            setDefaultValues();

            this.DialogResult = DialogResult.Cancel;

        }

        void tree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {

            if (layouts.ContainsKey(e.Node.Name)) {
                layouts[e.Node.Name].Visible = true;
            }

        }

        private void ChangeFontLabel(object sender, EventArgs e) {
            ABC.Font = new Font(comboFontFont.Text, float.Parse(numFontSize.Value.ToString()));
        }

        private void fillLayoutDictionary() {
             layouts.Add("cNodeFont", layoutFont);
        }

        private void setLayoutVisability() {
            layoutFont.Visible = false;
            canApplyChanges(false);
        }

        private void canApplyChanges(bool can) {
            btApply.Enabled = can;
            btOk.Enabled = can;
        }

        public void addApplyEventHandlers(Control where) {

            foreach (Control c in where.Controls) {

                if (c is ComboBox) {
                    (c as ComboBox).SelectedIndexChanged += _applyEventHandler;
                }

                else if (c is NumericUpDown) {
                    (c as NumericUpDown).ValueChanged += _applyEventHandler;
                }

                else if (c.Controls.Count > 0) {
                    addApplyEventHandlers(c);
                }
            }
        }

        private void _applyEventHandler(object sender, EventArgs e) {
            canApplyChanges(true);
        }

        private void setDefaultValues() {
            this.numFontSize.Value = Convert.ToInt32(gVars.fontSize.ToString());
            this.comboFontFont.SelectedIndex = this.comboFontFont.FindStringExact(gVars.fontName);
        }


        private void btOk_Click(object sender, EventArgs e) {
            btApply_Click(sender, e);
            this.Close();
        }

        private void btCancel_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void btApply_Click(object sender, EventArgs e) {
			
			gVars.fontName = comboFontFont.Text;
			gVars.fontSize = float.Parse(numFontSize.Value.ToString());
		
			SetSettings.setAllSettings(this);
            canApplyChanges(false);
        }
    }
}
