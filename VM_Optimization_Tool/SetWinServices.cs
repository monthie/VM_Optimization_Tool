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
    }
}
