using System.ComponentModel;
using System.Diagnostics;

namespace VM_Optimization_Tool
{
    static class ApplyChanges
    {      
        public static void Changes(XmlParser[] xmlParsers, BackgroundWorker bgWorker)
        {
            int count = 0;
            int highestPercentageReached = 0;
            foreach (XmlParser task in xmlParsers)
            {
                string command = "";

                if (task.Type == "Registry" && task.Command == "ADD")
                {
                    if (task.Params.Count == 1)
                    {
                        command = "/C reg " + task.Command + " \"" + task.Params[0] + "\" /f";
                    } else
                    {
                        command = "/C reg " + task.Command + " \"" + task.Params[0] + "\" /v \"" + task.Params[1] + "\" /t \"" + task.Params[2] + "\" /d " + task.Params[3] + " /f";
                    }
                }
                else if (task.Type == "Registry" && task.Command == "LOAD")
                {
                    command = "/C reg " + task.Command + " \"" + task.Params[0] + "\" \"" + task.Params[1] + "\"";
                }
                else if (task.Type == "ShellExecute")
                {
                    command = "/C " + task.Command;
                }
                else if (task.Type == "Service")
                {
                    command = "/C powershell.exe Set-Service '" + task.Params[0] + "' -startuptype \"" + task.Params[1] + "\"";
                }
                else if (task.Type == "SchTasks")
                {
                    if (task.Params[1] == "DISABLED")
                    {
                        command = "/C schtasks /change /tn \"" + task.Params[0]+"\" /disable";
                    }                   
                }
                if (command.Length != 0) {
                    var proc = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            Arguments = command,
                            WindowStyle = ProcessWindowStyle.Hidden,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            FileName = "cmd.exe"
                        }
                    };
                    LogWriter.LogWrite(command);
                    proc.OutputDataReceived += (s, e) => LogWriter.LogWrite(e.Data);
                    proc.ErrorDataReceived += (s, e) => LogWriter.LogWrite(e.Data);
                    proc.Start();
                    proc.BeginOutputReadLine();
                    proc.BeginErrorReadLine();
                    proc.WaitForExit();
                    count += 1;
                    int percentComplete = (int)((float)count / (float)xmlParsers.Length * 100);
                    // Report progress as a percentage of the total task.
                    
                    if (percentComplete > highestPercentageReached)
                    {
                        highestPercentageReached = percentComplete;
                        bgWorker.ReportProgress(percentComplete);
                    }
                    if (bgWorker.CancellationPending) break;
                }  
            }
        }
    }
}
