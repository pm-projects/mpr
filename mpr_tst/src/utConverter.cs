#pragma warning disable 219

//#define DEBUG

using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Security.Cryptography;
using System.Globalization;

namespace mpr_tst.src
{
    public class convert

 //! в  классе собраны различные функции для конвертирования всего во все.
    {
        const string _nm = "convert";

        static public string Encode(string s)
        {
            if (s == null) return null;
            byte[] b = System.Text.Encoding.UTF8.GetBytes(s);
            return Convert.ToBase64String(b);
        }
        static public string Decode(string s)
        {
            if (s == null) return null;
            try
            {
                byte[] b = Convert.FromBase64String(s);
                return System.Text.Encoding.UTF8.GetString(b);
            }
            catch (Exception ex)
            {
                gVars.lfl.wrEx(_nm, ex);
                return ">>>" + s + "<<<";
            }
        }



        static public
        int btoi(string par)
        {
            return (String.Compare(par, "True", true) == 0) ? 1 : 0;
        }

        static public
        string btoa(string par)
        {
            return btoi(par).ToString();
        }



        public static object atonull(object x)
        {
            string a = x.ToString();

            if (a == CNST.NULL)
                return null;
            else
                return x;
        }


        static public string word(string par)   ///< to get first word in the string
        {
            string rc = "";
            int i = 0, l = par.Length;
            for (i = 0; i < l && par[i] != ' '; i++)
            {
                rc += par[i].ToString();
            }
            return rc;
        }
        static public int idx(string par)      ///< to convert  second word in the string  to int
        {
            int rc = -1;
            int i = 0, l = par.Length;
            for (i = 0; i < l && par[i] != ' '; i++)
                ;
            if (++i < l)
                rc = Int32.Parse(par.Substring(i));
            return rc;
        }


        static public
        bool atob(string par)
        {
            if (par.ToLower() == "false")
                return false;
            else if (par.ToLower() == "no")
                return false;
            else if (atoi(par) == 0)
                return false;
            return true;
        }


        static public
        int[] atois(string str)
        {
            int[] dNs = null;

            if (str != null && str.Length > 0)
            {
                string[] vs = str.Split(new char[] { ' ' });
                dNs = new int[vs.Length];
                for (int x = 0; x < vs.Length; x++)
                {
                    dNs[x] = convert.atoi(vs[x]);
                }
            }
            return dNs;
        }

        static public
        int atoi(string str)
        {
            return (int)atoNmbr(str, true, new CultureInfo(gVars.cI, false).NumberFormat);
        }
        static public
        uint atoui(string str)
        {
            return (uint)atoNmbr(str, true, new CultureInfo(gVars.cI, false).NumberFormat);
        }

        static public
        double atod(string str)
        {
            return (double)atoNmbr(str, false, new CultureInfo(gVars.cI, false).NumberFormat);
        }

        static public
        double atod(string str, string sep)
        {
            NumberFormatInfo f = new CultureInfo(gVars.cI, false).NumberFormat;
            f.NumberDecimalSeparator = sep;
            return (double)atoNmbr(str, false, f);
        }

        static public
        double atoNmbr(string str, bool onlyInt, NumberFormatInfo f)
        {
            double r;
            for (int i = str.Length; i > 0; i--)
            {
                if (double.TryParse(str.Substring(0, i),
                        onlyInt ? (
                        NumberStyles.AllowLeadingSign |
                        NumberStyles.AllowLeadingWhite |
                        NumberStyles.AllowTrailingWhite
                        )
                        :
                       (NumberStyles.AllowCurrencySymbol |
                        NumberStyles.AllowExponent |
                        NumberStyles.AllowThousands |
                        NumberStyles.AllowDecimalPoint |
                        NumberStyles.AllowLeadingSign |
                        NumberStyles.AllowLeadingWhite |
                        NumberStyles.AllowTrailingWhite
                       ),
                        f, out r))
                {
                    return r;
                }
            }
            return 0.0;
        }
    }
}
