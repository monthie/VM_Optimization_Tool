using System;
using System.IO;
using System.Reflection;

namespace VM_Optimization_Tool
{
    public static class LogWriter
    {
        private static string m_exePath = string.Empty;
        /*public LogWriter(string logMessage)
        {
            LogWrite(logMessage);
        }*/
        public static void LogWrite(string logMessage)
        {
            m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + "log.txt"))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private static void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                if (logMessage.Length != 0)
                {
                    txtWriter.Write("{0} {1}: ", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
                    txtWriter.WriteLine("{0}", logMessage);
                }   
            }
            catch (Exception ex)
            {
            }
        }
    }
}
