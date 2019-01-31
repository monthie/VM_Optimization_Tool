using System.Diagnostics;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections.Generic;

namespace VM_Optimization_Tool
{
    class Optimization
    {
        private readonly static int screenWidth = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
        private readonly static int screenHeight = (int)System.Windows.SystemParameters.PrimaryScreenHeight;
        private readonly static int windowSizeWidth = 800;
        private readonly static int windowSizeHeight = 500;

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
        }
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
        public static void StartSDelete()
        {
            string command = "/C sdelete.exe -z C:";
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
