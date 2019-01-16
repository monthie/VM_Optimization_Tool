using System;
using System.ServiceProcess;

namespace VM_Optimization_Tool
{
    static class SetWinServices
    {
        public static bool DisableWinUpdates()
        {
            ServiceController sc = new ServiceController("wuauserv");
            
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
        public static bool DisableDefrag()
        {
            ServiceController sc = new ServiceController("defragsvc");

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
    }
}
