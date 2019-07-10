using Microsoft.Win32;
using System;
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
                var key = Registry.LocalMachine.OpenSubKey
                (@"SYSTEM\CurrentControlSet\Services\" + ServiceName, true);
                if (key != null) key.SetValue("Start", 2);
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
                var key = Registry.LocalMachine.OpenSubKey
                (@"SYSTEM\CurrentControlSet\Services\" + ServiceName, true);
                if (key != null) key.SetValue("Start", 4);
            }
            catch (Exception e)
            {
                throw new Exception("Could not disable the service, error: " + e.Message);
            }
        }
    }
}
