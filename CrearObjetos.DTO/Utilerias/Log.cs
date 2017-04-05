using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrearObjetos.DTO.Utilerias
{
    public class Log
    {
        private static readonly object sync = new object();
        private static string pathLog = ConfigurationSettings.AppSettings["FileLog"].ToString();

        public static void LogFile(string ErrorText, string Method, string FileName, string UserName)
        {
            try
            {
                lock (sync)
                {
                    string path = pathLog;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    else
                    {
                        path = pathLog;
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                    }
                    StreamWriter swLog;
                    string sLogFileName = FileName;
                    string sFilename = string.Format("{0}\\{1}_{2}.txt", path, sLogFileName, DateTime.Now.ToString("yyyyMMdd"));

                    if (!File.Exists(sFilename))
                    {
                        swLog = new StreamWriter(sFilename);
                    }
                    else
                    {
                        swLog = File.AppendText(sFilename);
                    }
                    swLog.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    swLog.WriteLine(Method + " " + UserName);
                    swLog.WriteLine(DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " " + ErrorText);
                    swLog.Close();
                }
            }
            catch (Exception ex)
            {
                LogFile(ex.Message + " " + ex.StackTrace, Method, FileName, UserName);
            }
        }
    }
}