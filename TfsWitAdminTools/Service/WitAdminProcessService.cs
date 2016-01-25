using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TfsWitAdminTools.Core;

namespace TfsWitAdminTools.Service
{
    public class WitAdminProcessService : IWitAdminProcessService
    {
        #region Fields

        private readonly Process _process;
        public static readonly string WitAdminExecFileName = "witadmin.exe";

        private StringBuilder _output = new StringBuilder();
        private List<string> _splitedOutput = new List<string>();
        private StringBuilder _error = new StringBuilder();

        #endregion

        #region Props

        public string Output
        {
            get
            {
                return _output.ToString();
            }
        }

        public List<string> SplitedOutput
        {
            get
            {
                return _splitedOutput;
            }
        }

        public string Error
        {
            get
            {
                return _error.ToString();
            }
        }

        #endregion

        #region Ctor

        public WitAdminProcessService(string argument, IConfigProvider configProvider)
        {
            var workingDirectory = configProvider.GetConfig("witAdminExecutableAddress");
            var witAdminPath = string.Format("{0}\\{1}", workingDirectory, WitAdminExecFileName);
            ProcessStartInfo startInfo = new ProcessStartInfo(witAdminPath)
            {
                Arguments = argument,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                Verb = "runas",
                CreateNoWindow = true
            };

            Process process = new Process()
            {
                StartInfo = startInfo
            };

            process.OutputDataReceived += ((sender, e) =>
            {
                if (e.Data != null)
                {
                    _output.AppendLine(e.Data);
                    _splitedOutput.Add(e.Data);
                }
            });

            process.ErrorDataReceived += ((sender, e) =>
            {
                if (e.Data != null)
                    _error.AppendLine(e.Data);
            });

            _process = process;
        }

        #endregion

        #region Methods

        public async Task Start()
        {
            await Task.Run(() => _process.Start());

            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();
        }

        public void WaitForExit()
        {
            _process.WaitForExit();
        }

        #endregion
    }
}
