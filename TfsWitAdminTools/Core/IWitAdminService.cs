using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TfsWitAdminTools.Cmn;
using TfsWitAdminTools.Service;

namespace TfsWitAdminTools.Core
{
    public interface IWitAdminService
    {
        Task<string[]> ExportWorkItemTypes(TFManager tfManager, string projectCollectionName, string teamProjectName);

        string ExportWorkItemDefenition(TFManager tfManager, string projectCollectionName, string teamProjectName, string workItemTypeName);

        void ExportWorkItemDefenition(TFManager tfManager, string projectCollectionName, string teamProjectName, string workItemTypeName,
            string fileName);

        void ImportWorkItemDefenition(TFManager tfManager, string projectCollectionName, string teamProjectName, string fileName);

        string RenameWorkItem(TFManager tfManager, string projectCollectionName, string teamProjectName, string workItemTypeName, string newName);

        string ExportCategories(TFManager tfManager, string projectCollectionName, string teamProjectName);

        void ExportCategories(TFManager tfManager, string projectCollectionName, string teamProjectName, string fileName);

        void ImportCategories(TFManager tfManager, string projectCollectionName, string teamProjectName, string fileName);

        string InvokeCommand(string argument);

        Task<string[]> InvokeCommandWithSplitedResult(string argument);

        Process CreateProcess(string argument);

        event EventHandler<CommandInvokedEventArgs> CommandInvoked;
    }
}
