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
            string argument = string.Format("renamewitd /collection:{0}/{1} /p:{2} /n:\"{3}\" /new:\"{4}\" /noprompt", tfManager.TfsAddress, projectCollectionName, teamProjectName, workItemTypeName, newName);

            string result = InvokeCommand(argument, true);

            return result;
        }

        public string DestroyWorkItem(ITFManager tfManager, string projectCollectionName, string teamProjectName, string workItemTypeName)
        {
            string argument = string.Format("destroywitd /collection:{0}/{1} /p:{2} /n:\"{3}\" /noprompt", tfManager.TfsAddress, projectCollectionName, teamProjectName, workItemTypeName);

            string result = InvokeCommand(argument, true);

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

        public virtual string InvokeCommand(string argument, bool isConfirmRequired = false)
        {
            IWitAdminProcessService process = CreateProcess(argument);

            process.Start();
            //process.WaitForExit();

            string result = process.ReadToEnd();

            CommandInvokedEventArgs eventArg = new CommandInvokedEventArgs();
            eventArg.Argument = argument;
            eventArg.Output = result;
            OnCommandInvoked(eventArg);

            return result;
        }

        public async Task<string[]> InvokeCommandWithSplitedResult(string argument, bool isConfirmRequired = false)
        {
            CommandInvokedEventArgs eventArg = null;

            string[] results = await Task.Factory.StartNew<string[]>(() =>
            {
                IWitAdminProcessService process = CreateProcess(argument, isConfirmRequired);
                process.Start();
                process.WaitForExit();

                List<String> result = new List<string>();
                StringBuilder resultText = new StringBuilder();
                while (!process.IsEndOfStream())
                {
                    var resultItem = process.ReadLine();

                    result.Add(resultItem);

                    resultText.AppendLine(resultItem);
                }

                eventArg = new CommandInvokedEventArgs();
                eventArg.Argument = argument;
                eventArg.Output = resultText.ToString();

                return result.ToArray();
            });

            OnCommandInvoked(eventArg);

            return results;
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
                //CommandInvoked(this, e);
        }

        #endregion
    }

    public class CommandInvokedEventArgs : EventArgs
    {
        public string Argument { get; set; }

        public string Output { get; set; }
    }
}
