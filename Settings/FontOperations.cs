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
    static class FontOperations {

        public static void fillComboBox(ComboBox comboFont) {
            fillFontComboBox(comboFont);
        }

        public static void fillFontComboBox(ComboBox cb) {
            foreach (FontFamily font in FontFamily.Families) {
                cb.Items.Add(font.Name);
            }
        }
    }
}
