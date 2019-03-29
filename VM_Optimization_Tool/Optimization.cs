using System.Diagnostics;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace VM_Optimization_Tool
{
    /// <summary>
    /// Optimization class with various windows tasks
    /// </summary>
    class Optimization
    {

        // get info about screen to move commando line position
        private readonly static int screenWidth = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
        private readonly static int screenHeight = (int)System.Windows.SystemParameters.PrimaryScreenHeight;
        private readonly static int windowSizeWidth = 800;
        private readonly static int windowSizeHeight = 500;

        /// <summary>
        /// Start cleanmgr.exe with parameter /lowdisk to auto select all options to clean
        /// </summary>
        public static void StartCleanMgr() {
            string command = "/C cleanmgr.exe /lowdisk";
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    Arguments = command,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    FileName = "cmd.exe"
                }
            };
            proc.Start();
            LogWriter.LogWrite("started Cleanmgr.exe");
            proc.WaitForExit();
        }

        /// <summary>
        /// Start dism.exe if win 8 or higher. clean win updates etc.
        /// </summary>
        public static void StartDism() {
            string command = "/C Dism.exe /Online /Cleanup-Image /AnalyzeComponentStore";
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    Arguments = command,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    FileName = "cmd.exe"
                }
            };            
            proc.Start();
            while (string.IsNullOrEmpty(proc.MainWindowTitle))
            {
                Thread.Sleep(100);
                proc.Refresh();
            }
            MoveWindow(proc.MainWindowHandle, (screenWidth / 2), (screenHeight / 2) - windowSizeHeight, windowSizeWidth, windowSizeHeight, true);
            proc.WaitForExit();
            command = "/C Dism.exe /Online /Cleanup-Image /StartComponentCleanup /ResetBase";
            proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    Arguments = command,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    FileName = "cmd.exe"
                }
            };
            proc.Start();
            while (string.IsNullOrEmpty(proc.MainWindowTitle))
            {
                Thread.Sleep(100);
                proc.Refresh();
            }
            MoveWindow(proc.MainWindowHandle, (screenWidth / 2), (screenHeight / 2), windowSizeWidth, windowSizeHeight, true);
            proc.WaitForExit();
        }

        /// <summary>
        /// Start defrag process
        /// </summary>
        public static void StartDefrag()
        {
            string command = "/C sc config defragsvc start= demand";
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    Arguments = command,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    FileName = "cmd.exe"
                }
            };
            proc.Start();
            proc.WaitForExit();
            command = "/C defrag C:\\ /H /U /V";
            proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    Arguments = command,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    FileName = "cmd.exe"
                }
            };
            proc.Start();
            while (string.IsNullOrEmpty(proc.MainWindowTitle))
            {
                Thread.Sleep(100);
                proc.Refresh();
            }
            MoveWindow(proc.MainWindowHandle, (screenWidth / 2) - windowSizeWidth, (screenHeight / 2), windowSizeWidth, windowSizeHeight, true);
            proc.WaitForExit();
            command = "/C sc config defragsvc start= disabled";
            proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    Arguments = command,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    FileName = "cmd.exe"
                }
            };
            proc.Start();
            proc.WaitForExit();
        }

        /// <summary>
        /// start sdelete to null empty storage
        /// </summary>
        public static void StartSDelete()
        {
            string command = "/C \"C:\\Program Files\\VM Optimization Tool\\VM Optimization Tool\\sdelete.exe\" -z C:";
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    Arguments = command,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    FileName = "cmd.exe"
                }
            };
            proc.Start();
            proc.WaitForExit();
        }

        [DllImport("User32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int cx, int cy, bool repaint);
    }
}
