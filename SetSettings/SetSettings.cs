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

	public static class SetSettings {
		
		public static void setFont (Form f) {
			f.Font = new Font(gVars.fontName, gVars.fontSize);
		} 
		
		public static void setAllSettings(Form f) {
			setFont(f);
		} 
		
	}

}