using System;
using System.IO;
using System.Reflection;

namespace VM_Optimization_Tool
{

    /// <summary>
    /// Log Class to log errors etc.
    /// </summary>
    public static class LogWriter
    {
        private static string m_exePath = string.Empty;

        /// <summary>
        /// Log output
        /// </summary>
        /// <param name="logMessage">Message to log</param>
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
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Helper function to log
        /// </summary>
        /// <param name="logMessage"></param>
        /// <param name="txtWriter"></param>
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
            catch (Exception)
            {
            }
        }
    }
}
