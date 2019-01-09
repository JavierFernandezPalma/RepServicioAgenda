using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROBOT
{
    public class LogErrores
    {
        public void EscribaLog(string modulo, string log)
        {
            string path = ConfigurationManager.AppSettings["LogErrores"];

            using (StreamWriter sw = File.AppendText(path + "LOG_" + modulo.ToUpper() + "_" + System.DateTime.Now.ToString("dd-MM-yyyy")))
            {
                sw.WriteLine("");
                sw.WriteLine("Se ha generado el siguiente LOG : \n" + log);
                sw.WriteLine("\n");
                sw.WriteLine("Registrado el : " + System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
                sw.WriteLine("");
                sw.WriteLine("=================================================================================================");
            }
        }
    }
}
