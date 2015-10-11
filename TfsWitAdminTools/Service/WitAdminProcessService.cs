using System.Diagnostics;
using System.IO;
using TfsWitAdminTools.Core;

namespace TfsWitAdminTools.Service
{
    public class WitAdminProcessService : IWitAdminProcessService
    {
        private readonly Process _process;
        private readonly string _confirmation = "Yes";
        public static readonly string WitAdminExecFileName = "witadmin.exe";

        public WitAdminProcessService(string argument, bool isConfirmationRequired, IConfigProvider configProvider)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = WitAdminExecFileName,
                WorkingDirectory = configProvider.GetConfig("witAdminExecutableAddress"),
                Arguments = argument,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process process = new Process()
            {
                StartInfo = startInfo
            };

            this._process = process;
        }

        public bool IsEndOfStream()
        {
            return _process.StandardOutput.EndOfStream;
        }

        public string ReadLine()
        {
            return _process.StandardOutput.ReadLine();
        }

        public string ReadToEnd()
        {
            return _process.StandardOutput.ReadToEnd();
        }

        public void Start()
        {
            _process.Start();
        }

        public void WaitForExit()
        {
            _process.WaitForExit();
        }
    }
}
