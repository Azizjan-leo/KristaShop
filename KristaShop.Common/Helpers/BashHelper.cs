using System.Diagnostics;

namespace KristaShop.Common.Helpers {
    public static class BashHelper {
        public static void ExecuteCommand(string command) {
            var escapedArgs = command.Replace("\"", "\\\"");

            using var process = new Process {
                StartInfo = new ProcessStartInfo {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\""
                }
            };

            process.Start();
            process.WaitForExit();
        }
    }
}
