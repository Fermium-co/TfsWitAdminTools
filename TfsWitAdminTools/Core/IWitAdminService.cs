using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TfsWitAdminTools.Cmn;
using TfsWitAdminTools.Service;

namespace TfsWitAdminTools.Core
{
    public interface IWitAdminService
    {
        Task<string[]> ExportWorkItemTypes(ITFManager tfManager, string projectCollectionName, string teamProjectName);

        string ExportWorkItemDefenition(ITFManager tfManager, string projectCollectionName, string teamProjectName, string workItemTypeName);

        void ExportWorkItemDefenition(ITFManager tfManager, string projectCollectionName, string teamProjectName, string workItemTypeName,
            string fileName);

        void ImportWorkItemDefenition(ITFManager tfManager, string projectCollectionName, string teamProjectName, string fileName);

        string RenameWorkItem(ITFManager tfManager, string projectCollectionName, string teamProjectName, string workItemTypeName, string newName);

        string ExportCategories(ITFManager tfManager, string projectCollectionName, string teamProjectName);

        void ExportCategories(ITFManager tfManager, string projectCollectionName, string teamProjectName, string fileName);

        void ImportCategories(ITFManager tfManager, string projectCollectionName, string teamProjectName, string fileName);

        string InvokeCommand(string argument, bool isConfirmRequired = false);

        Task<string[]> InvokeCommandWithSplitedResult(string argument, bool isConfirmRequired = false);

        IWitAdminProcessService CreateProcess(string argument, string[] confirmations);

        event EventHandler<CommandInvokedEventArgs> CommandInvoked;
    }
}
