﻿using System;
using System.Diagnostics;
using System.ServiceProcess;

namespace VM_Optimization_Tool
{
    /// <summary>
    /// static class to enable or disable win services
    /// </summary>
    static class SetWinServices
    {
        public static bool DisableWinService(string service)
        {
            ServiceController sc = new ServiceController(service);

            try
            {
                if (sc != null && sc.Status == ServiceControllerStatus.Running)
                {
                    sc.Stop();
                }
                sc.WaitForStatus(ServiceControllerStatus.Stopped);
                sc.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public static bool EnableWinService(string service)
        {
            ServiceController sc = new ServiceController(service);

            try
            {
                if (sc != null && sc.Status == ServiceControllerStatus.Stopped)
                {
                    sc.Start();
                }
                sc.WaitForStatus(ServiceControllerStatus.Running);
                sc.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Can be called to enable the Windows Service
        /// </summary>
        public static void Enable(string ServiceName)
        {
            try
            {
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        Arguments = "/C powershell.exe Set-Service '" + ServiceName + "' -startuptype \"AUTOMATIC\"",
                        WindowStyle = ProcessWindowStyle.Hidden,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        FileName = "cmd.exe"
                    }
                };
                proc.OutputDataReceived += (s, e) => LogWriter.LogWrite(e.Data);
                proc.ErrorDataReceived += (s, e) => LogWriter.LogWrite(e.Data);
                proc.Start();
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();
                proc.WaitForExit();
            }
            catch (Exception e)
            {
                throw new Exception("Could not enable the service, error: " + e.Message);
            }
        }

        /// <summary>
        /// Disables the Windows service
        /// </summary>
        public static void Disable(string ServiceName)
        {
            try
            {
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        Arguments = "/C powershell.exe Set-Service '" + ServiceName + "' -startuptype \"DISABLED\"",
                        WindowStyle = ProcessWindowStyle.Hidden,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        FileName = "cmd.exe"
                    }
                };
                proc.OutputDataReceived += (s, e) => LogWriter.LogWrite(e.Data);
                proc.ErrorDataReceived += (s, e) => LogWriter.LogWrite(e.Data);
                proc.Start();
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();
                proc.WaitForExit();
            }
            catch (Exception e)
            {
                throw new Exception("Could not disable the service, error: " + e.Message);
            }
        }
    }
}
