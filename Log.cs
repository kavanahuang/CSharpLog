using System;
using System.Diagnostics;
using System.IO;

namespace CSharpLog
{
    class Log
    {
        private static readonly bool debug = false;  

        // Write to log file.
        public static bool WriteLogs(string dirName, string type, string content)
        {
            if (!Logs.debug)
            {
                return false;
            }

            string path = AppDomain.CurrentDomain.BaseDirectory;
            if (!string.IsNullOrEmpty(path))
            {
                path = AppDomain.CurrentDomain.BaseDirectory + dirName;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                separator = Path.DirectorySeparatorChar;
                path = path + separator + DateTime.Now.ToString("yyyyMMdd") + ".log";
                if (!File.Exists(path))
                {
                    FileStream fs = File.Create(path);
                    fs.Close();
                }

                if (File.Exists(path))
                {
                    string fileInfo = "[" + type + "]" + Logs.GetFileInfo();
                    StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default);

                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + fileInfo + content);
                    sw.Close();
                }
            }

            return true;
        }

        // Get called file information.
        public static string GetFileInfo()
        {
            StackTrace st   = new StackTrace(true);
            StackFrame sf   = st.GetFrame(4);
            string filename = sf.GetFileName();
            int line        = sf.GetFileLineNumber();

            return "[" + filename + ":" + line + "]";
        }

        private static bool Log(string type, string content)
        {
            return Logs.WriteLogs("logs", type, content);
        }

        public static bool Debug(string content)
        {
            return Logs.Log("Debug", content);
        }

        public static bool Info(string content)
        {
            return Logs.Log("Info", content);
        }

        public static bool Warn(string content)
        {
            return Logs.Log("Warn", content);
        }

        public static bool Error(string content)
        {
            return Logs.Log("Error", content);
        }

        public static bool Fatal(string content)
        {
            return Logs.Log("Fatal", content);
        }
    }
}
