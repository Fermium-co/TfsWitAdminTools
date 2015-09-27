using System.Diagnostics;
using TfsWitAdminTools.Core;

namespace TfsWitAdminTools.Service
{
    public class DefaultProcessService : IProcessService
    {
        private readonly Process _process;

        public DefaultProcessService(string argument, IConfigProvider configProvider)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = configProvider.GetConfig("witAdminExecutableAddress"),
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
