using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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

        public async Task<string> ExportWorkItemDefenition(ITFManager tfManager, string projectCollectionName, string teamProjectName, string workItemTypeName)
        {
            string argument = string.Format("exportwitd /collection:{0}/{1} /p:{2} /n:\"{3}\"", tfManager.TfsAddress, projectCollectionName, teamProjectName, workItemTypeName);

            string workItemDefenition = await InvokeCommand(argument);

            return workItemDefenition;
        }

        public async Task ExportWorkItemDefenition(ITFManager tfManager, string projectCollectionName, string teamProjectName, string workItemTypeName, string fileName)
        {
            string argument = string.Format("exportwitd /collection:{0}/{1} /p:{2} /n:\"{3}\" /f:\"{4}\"", tfManager.TfsAddress, projectCollectionName, teamProjectName, workItemTypeName, fileName);
            await InvokeCommand(argument);
        }

        public async Task ImportWorkItemDefenition(ITFManager tfManager, string projectCollectionName, string teamProjectName, string fileName)
        {
            string argument = string.Format("importwitd /collection:{0}/{1} /p:{2} /f:\"{3}\"", tfManager.TfsAddress, projectCollectionName, teamProjectName, fileName);
            await InvokeCommand(argument);
        }

        public async Task<string> RenameWorkItem(ITFManager tfManager, string projectCollectionName, string teamProjectName, string workItemTypeName, string newName)
        {
            string argument = string.Format("renamewitd /collection:{0}/{1} /p:{2} /n:\"{3}\" /new:\"{4}\" /noprompt", tfManager.TfsAddress, projectCollectionName, teamProjectName, workItemTypeName, newName);

            string result = await InvokeCommand(argument, true);

            return result;
        }

        public async Task<string> DestroyWorkItem(ITFManager tfManager, string projectCollectionName, string teamProjectName, string workItemTypeName)
        {
            string argument = string.Format("destroywitd /collection:{0}/{1} /p:{2} /n:\"{3}\" /noprompt", tfManager.TfsAddress, projectCollectionName, teamProjectName, workItemTypeName);

            string result = await InvokeCommand(argument, true);

            return result;
        }

        public async Task<string> ExportCategories(ITFManager tfManager, string projectCollectionName, string teamProjectName)
        {
            string argument = string.Format("exportcategories /collection:{0}/{1} /p:{2}", tfManager.TfsAddress, projectCollectionName, teamProjectName);
            string result = await InvokeCommand(argument);

            return result;
        }

        public async Task ExportCategories(ITFManager tfManager, string projectCollectionName, string teamProjectName, string fileName)
        {
            string argument = string.Format("exportcategories /collection:{0}/{1} /p:{2} /f:\"{3}\"", tfManager.TfsAddress, projectCollectionName, teamProjectName, fileName);
            await InvokeCommand(argument);
        }

        public async Task ImportCategories(ITFManager tfManager, string projectCollectionName, string teamProjectName, string fileName)
        {
            string argument = string.Format("importcategories /collection:{0}/{1} /p:{2} /f:\"{3}\"", tfManager.TfsAddress, projectCollectionName, teamProjectName, fileName);
            await InvokeCommand(argument);
        }

        public async Task<string> ExportProcessConfig(ITFManager tfManager, string projectCollectionName, string teamProjectName)
        {
            string argument = string.Format("exportprocessconfig /collection:{0}/{1} /p:{2}", tfManager.TfsAddress, projectCollectionName, teamProjectName);
            string result = await InvokeCommand(argument);

            return result;
        }

        public async Task ExportProcessConfig(ITFManager tfManager, string projectCollectionName, string teamProjectName, string fileName)
        {
            string argument = string.Format("exportprocessconfig /collection:{0}/{1} /p:{2} /f:\"{3}\"", tfManager.TfsAddress, projectCollectionName, teamProjectName, fileName);
            await InvokeCommand(argument);
        }

        public async Task ImportProcessConfig(ITFManager tfManager, string projectCollectionName, string teamProjectName, string fileName)
        {
            string argument = string.Format("importprocessconfig /collection:{0}/{1} /p:{2} /f:\"{3}\"", tfManager.TfsAddress, projectCollectionName, teamProjectName, fileName);
            await InvokeCommand(argument);
        }

        #endregion

        #region CoreMethods

        public virtual async Task<string> InvokeCommand(string argument, bool isConfirmRequired = false)
        {
            string result = await RunProcess(argument);

            return result;
        }

        private async Task<string> RunProcess(string argument)
        {
            IWitAdminProcessService process = CreateProcess(argument);

            await process.Start();
            //process.WaitForExit();

            string result = null;
            var errorMessage = process.ReadError();
            if (string.IsNullOrEmpty(errorMessage))
                result = process.ReadToEnd();
            else
                result = string.Format("Error : \n{0}", errorMessage);

            CommandInvokedEventArgs eventArg = new CommandInvokedEventArgs();
            eventArg.Argument = argument;
            eventArg.Output = result;
            OnCommandInvoked(eventArg);

            return result;
        }

        public async Task<string[]> InvokeCommandWithSplitedResult(string argument, bool isConfirmRequired = false)
        {
            string[] results = await RunProcessWithSplitedResult(argument, isConfirmRequired);

            return results;
        }

        private async Task<string[]> RunProcessWithSplitedResult(string argument, bool isConfirmRequired)
        {
            CommandInvokedEventArgs eventArg = null;

            IWitAdminProcessService process = CreateProcess(argument, isConfirmRequired);
            await process.Start();
            process.WaitForExit();

            List<String> result = new List<string>();
            var errorMessage = process.ReadError();
            StringBuilder resultText = new StringBuilder();
            if (string.IsNullOrEmpty(errorMessage))
            {
                while (!process.IsEndOfStream())
                {
                    var resultItem = process.ReadLine();

                    result.Add(resultItem);

                    resultText.AppendLine(resultItem);
                }
            }
            else
                resultText.AppendLine(string.Format("Error : \n{0}", errorMessage));

            eventArg = new CommandInvokedEventArgs();
            eventArg.Argument = argument;
            eventArg.Output = resultText.ToString();
            OnCommandInvoked(eventArg);

            return result.ToArray();
        }

        public virtual IWitAdminProcessService CreateProcess(string argument, bool isConfirmationRequired = false)
        {
            var process = DiManager.Current.Resolve<IWitAdminProcessService>(new { argument = argument, isConfirmationRequired = isConfirmationRequired });
            return process;
        }

        #endregion

        #region Events

        public event EventHandler<CommandInvokedEventArgs> CommandInvoked;

        private readonly IConfigProvider _configProvider;

        protected virtual void OnCommandInvoked(CommandInvokedEventArgs e)
        {
            if (CommandInvoked != null)
                System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(CommandInvoked, this, e);
        }

        #endregion
    }

    public class CommandInvokedEventArgs : EventArgs
    {
        public string Argument { get; set; }

        public string Output { get; set; }
    }
}
