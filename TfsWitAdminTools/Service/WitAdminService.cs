using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TfsWitAdminTools.Cmn;
using TfsWitAdminTools.Core;

namespace TfsWitAdminTools.Service
{
    public class WitAdminService : IWitAdminService
    {
        public WitAdminService(IConfigProvider configProvider)
        {
            this._configProvider = configProvider;
        }

        #region Methods

        public async Task<string[]> ExportWorkItemTypes(ITFManager tfManager, string projectCollectionName, string teamProjectName)
        {
            string argument = string.Format(@"listwitd /collection:{0}/{1} /p:{2}", tfManager.TfsAddress, projectCollectionName, teamProjectName);

            string[] workItemTypes = await InvokeCommandWithSplitedResult(argument);

            return workItemTypes;
        }

        public string ExportWorkItemDefenition(ITFManager tfManager, string projectCollectionName, string teamProjectName, string workItemTypeName)
        {
            string argument = string.Format("exportwitd /collection:{0}/{1} /p:{2} /n:\"{3}\"", tfManager.TfsAddress, projectCollectionName, teamProjectName, workItemTypeName);

            string workItemDefenition = InvokeCommand(argument);

            return workItemDefenition;
        }

        public void ExportWorkItemDefenition(ITFManager tfManager, string projectCollectionName, string teamProjectName, string workItemTypeName, string fileName)
        {
            string argument = string.Format("exportwitd /collection:{0}/{1} /p:{2} /n:\"{3}\" /f:\"{4}\"", tfManager.TfsAddress, projectCollectionName, teamProjectName, workItemTypeName, fileName);
            InvokeCommand(argument);
        }

        public void ImportWorkItemDefenition(ITFManager tfManager, string projectCollectionName, string teamProjectName, string fileName)
        {
            string argument = string.Format("importwitd /collection:{0}/{1} /p:{2} /f:\"{3}\"", tfManager.TfsAddress, projectCollectionName, teamProjectName, fileName);
            InvokeCommand(argument);
        }

        public string RenameWorkItem(ITFManager tfManager, string projectCollectionName, string teamProjectName, string workItemTypeName, string newName)
        {
            string argument = string.Format("renamewitd /collection:{0}/{1} /p:{2} /n:\"{3}\" /new:\"{4}\"", tfManager.TfsAddress, projectCollectionName, teamProjectName, workItemTypeName, newName);

            string result = InvokeCommand(argument);

            return result;
        }

        public string ExportCategories(ITFManager tfManager, string projectCollectionName, string teamProjectName)
        {
            string argument = string.Format("exportcategories /collection:{0}/{1} /p:{2}", tfManager.TfsAddress, projectCollectionName, teamProjectName);
            string result = InvokeCommand(argument);

            return result;
        }

        public void ExportCategories(ITFManager tfManager, string projectCollectionName, string teamProjectName, string fileName)
        {
            string argument = string.Format("exportcategories /collection:{0}/{1} /p:{2} /f:\"{3}\"", tfManager.TfsAddress, projectCollectionName, teamProjectName, fileName);
            InvokeCommand(argument);
        }

        public void ImportCategories(ITFManager tfManager, string projectCollectionName, string teamProjectName, string fileName)
        {
            string argument = string.Format("importcategories /collection:{0}/{1} /p:{2} /f:\"{3}\"", tfManager.TfsAddress, projectCollectionName, teamProjectName, fileName);
            InvokeCommand(argument);
        }

        #endregion

        #region CoreMethods

        public string InvokeCommand(string argument)
        {
            Process process = CreateProcess(argument);

            process.Start();
            //process.WaitForExit();

            string result = process.StandardOutput.ReadToEnd();

            CommandInvokedEventArgs eventArg = new CommandInvokedEventArgs();
            eventArg.Argument = argument;
            eventArg.Output = result;
            OnCommandInvoked(eventArg);

            return result;
        }

        public Task<string[]> InvokeCommandWithSplitedResult(string argument)
        {
            return Task.Factory.StartNew<string[]>(() =>
            {
                Process process = CreateProcess(argument);
                process.Start();
                process.WaitForExit();

                List<String> result = new List<string>();
                while (!process.StandardOutput.EndOfStream)
                {
                    result.Add(process.StandardOutput.ReadLine());
                }

                return result.ToArray();
            });
        }

        public Process CreateProcess(string argument)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = _configProvider.GetConfig("witAdminExecutableAddress"),
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

            return process;
        }

        #endregion

        #region Events

        public event EventHandler<CommandInvokedEventArgs> CommandInvoked;

        private readonly IConfigProvider _configProvider;

        protected virtual void OnCommandInvoked(CommandInvokedEventArgs e)
        {
            if (CommandInvoked != null)
                CommandInvoked(this, e);
        }

        #endregion
    }

    public class CommandInvokedEventArgs : EventArgs
    {
        public string Argument { get; set; }

        public string Output { get; set; }
    }
}
