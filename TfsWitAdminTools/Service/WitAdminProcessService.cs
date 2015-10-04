using System.Diagnostics;
using TfsWitAdminTools.Core;

namespace TfsWitAdminTools.Service
{
    public class WitAdminProcessService : IWitAdminProcessService
    {
        private readonly Process _process;
        private readonly string[] _confirmations;
        public static readonly string WitAdminExecFileName = "witadmin.exe";

        public WitAdminProcessService(string argument, string[] confirmations, IConfigProvider configProvider)
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

            bool hasConfirmation = _confirmations.Length > 0;
            if(hasConfirmation)
            {
                _process.StartInfo.FileName = null;
                _process.StartInfo.RedirectStandardInput = true;
                foreach(string confirmation in _confirmations)
                {
                    _process.StandardInput.WriteLine(confirmation);
                }
                _process.StandardInput.Close();
            }
        }

        public void WaitForExit()
        {
            _process.WaitForExit();
        }
    }
}
