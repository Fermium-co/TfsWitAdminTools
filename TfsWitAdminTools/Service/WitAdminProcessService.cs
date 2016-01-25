﻿using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using TfsWitAdminTools.Core;

namespace TfsWitAdminTools.Service
{
    public class WitAdminProcessService : IWitAdminProcessService
    {
        private readonly Process _process;
        public static readonly string WitAdminExecFileName = "witadmin.exe";

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

            this._process = process;
        }

        public string ReadError()
        {
            string errorMessage = null;
            errorMessage = _process.StandardError.ReadToEnd();
            return errorMessage;
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

        public async Task Start()
        {
            await Task.Run(() => _process.Start());
        }

        public void WaitForExit()
        {
            _process.WaitForExit();
        }
    }
}
