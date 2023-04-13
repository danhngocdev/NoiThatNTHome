using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DVG.WIS.Services.Monitor
{
    public class CreateLog
    {
        private static CreateLog _instance;
        private static object lockObject = new object();
        public static CreateLog Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (lockObject)
                    {
                        if (null == _instance)
                        {
                            _instance = new CreateLog();
                        }
                    }
                }
                return _instance;
            }
        }

        public static void WriteLog(string sErrMsg, string absolutePath)
        {
            try
            {
                absolutePath = LogPath(absolutePath);
                if (File.Exists(absolutePath))
                {
                    //File.WriteAllText(absolutePath, String.Empty);
                    File.Create(absolutePath).Close();
                }

                //string sLogFormat = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " ==> ";
                using (StreamWriter sw = new StreamWriter(absolutePath, true))
                {
                    sw.WriteLine(sErrMsg);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string LogPath(string absolutePath, string fileName = "Warning")
        {
            try
            {
                if (!Directory.Exists(absolutePath))
                {
                    Directory.CreateDirectory(absolutePath);
                }
            }
            catch (Exception)
            {
            }
            return string.Concat(absolutePath, "\\", fileName, ".txt");
        }
    }
}
