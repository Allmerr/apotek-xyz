using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apotek_xyz
{
    public class connection
    {
        public static string getKoneksi()
        {
            string result = "";
            result = ConfigurationManager.AppSettings["connString"].ToString();
            return result;
        }
    }
}
