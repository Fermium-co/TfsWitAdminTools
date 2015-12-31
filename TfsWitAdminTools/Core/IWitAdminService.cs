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

        Task<string> ExportWorkItemDefenition(ITFManager tfManager, string projectCollectionName, string teamProjectName, string workItemTypeName);

        Task ExportWorkItemDefenition(ITFManager tfManager, string projectCollectionName, string teamProjectName, string workItemTypeName,
            string fileName);

        Task ImportWorkItemDefenition(ITFManager tfManager, string projectCollectionName, string teamProjectName, string fileName);

        Task<string> RenameWorkItem(ITFManager tfManager, string projectCollectionName, string teamProjectName, string workItemTypeName, string newName);

        Task<string> DestroyWorkItem(ITFManager tfManager, string projectCollectionName, string teamProjectName, string workItemTypeName);

        Task<string> ExportCategories(ITFManager tfManager, string projectCollectionName, string teamProjectName);

        Task ExportCategories(ITFManager tfManager, string projectCollectionName, string teamProjectName, string fileName);

        Task ImportCategories(ITFManager tfManager, string projectCollectionName, string teamProjectName, string fileName);

        Task<string> ExportProcessConfig(ITFManager tfManager, string projectCollectionName, string teamProjectName);


        Task ExportProcessConfig(ITFManager tfManager, string projectCollectionName, string teamProjectName, string fileName);


        Task ImportProcessConfig(ITFManager tfManager, string projectCollectionName, string teamProjectName, string fileName);
        
        Task<string> InvokeCommand(string argument, bool isConfirmRequired = false);

        Task<string[]> InvokeCommandWithSplitedResult(string argument, bool isConfirmRequired = false);

        IWitAdminProcessService CreateProcess(string argument, bool isConfirmationRequired = false);

        event EventHandler<CommandInvokedEventArgs> CommandInvoked;
    }
}
