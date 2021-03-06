﻿using System.ComponentModel;
using System.Threading.Tasks;
using TfsWitAdminTools.Cmn;
using TfsWitAdminTools.Core;
using TfsWitAdminTools.Model;

namespace TfsWitAdminTools.ViewModel
{
    public class WIDExportVM : ToolsChildVM
    {
        #region Ctor

        public WIDExportVM(ToolsVM tools, IDialogProvider dialogProvider)
            : base(tools)
        {
            this._dialogProvider = dialogProvider;

            #region Commands

            BrowseCommand = new DelegateCommand(() =>
            {
                string path = _dialogProvider.OpenBrowseDialog();
                Path = path;
            });

            ExportCommand = new DelegateCommand(async () =>
            {
                await Export();
            },
            () => (
                Tools.CurrentProjectCollection != null &&
                (IsAllTeamProjects == true || Tools.CurrentTeamProject != null) &&
                (IsAllWorkItemTypes == true || Tools.CurrentWorkItemType != null) &&
                !string.IsNullOrEmpty(Path)
                )
            );

            #endregion
        }

        #endregion

        #region Props

        private string _path;

        public string Path
        {
            get { return _path; }
            set
            {
                if (Set(ref _path, value))
                    ExportCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _isAllTeamProjects;

        public bool IsAllTeamProjects
        {
            get { return _isAllTeamProjects; }
            set
            {
                if (Set(ref _isAllTeamProjects, value))
                    ExportCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _isAllWorkItemTypes;

        public bool IsAllWorkItemTypes
        {
            get { return _isAllWorkItemTypes; }
            set
            {
                if (Set(ref _isAllWorkItemTypes, value))
                    ExportCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Fields

        private IDialogProvider _dialogProvider;

        #endregion

        #region Methods

        private async Task Export()
        {
            ProjectCollectionInfo projectCollection = Tools.CurrentProjectCollection;

            TeamProjectInfo[] teamProjects;

            if (IsAllTeamProjects)
            {
                teamProjects = projectCollection.TeamProjectInfos;

                foreach (TeamProjectInfo teamProject in teamProjects)
                {
                    if (teamProject.WorkItemTypeInfos == null)
                    {
                        await Tools.GetAllTeamProjectsWITypes();
                        break;
                    }
                }
            }
            else
                teamProjects = new TeamProjectInfo[] { Tools.CurrentTeamProject };

            foreach (TeamProjectInfo teamProject in teamProjects)
            {
                WorkItemTypeInfo[] workItemTypeInfos;
                string path = Path;

                if (IsAllTeamProjects)
                {
                    path = System.IO.Path.Combine(path, teamProject.Name);
                    System.IO.Directory.CreateDirectory(path);
                }

                workItemTypeInfos = (IsAllWorkItemTypes)
                    ? teamProject.WorkItemTypeInfos
                    : workItemTypeInfos = new WorkItemTypeInfo[] { Tools.CurrentWorkItemType };

                Tools.Progress.BeginWorking(workItemTypeInfos.Length);
                try
                {
                    foreach (WorkItemTypeInfo workItemTypeInfo in workItemTypeInfos)
                    {
                        try
                        {
                            await Export(projectCollection, teamProject, workItemTypeInfo, path);
                            Tools.Progress.NextStep();
                        }
                        catch (WitAdminException)
                        {
                            Tools.Progress.FailStep();
                        }
                    }
                }
                finally
                {
                    Tools.Progress.EndWorking();
                }
            }
        }

        private async Task Export(ProjectCollectionInfo projectCollection, TeamProjectInfo teamProject, WorkItemTypeInfo workItemType, string path)
        {
            string projectCollectionName = projectCollection.Name;
            string teamProjectName = teamProject.Name;
            string workItemTypeName = workItemType.Name;

            string fileName = string.Format("{0}.xml", workItemTypeName);
            string fullPath = System.IO.Path.Combine(path, fileName);

            await Tools.WitAdminService.ExportWorkItemDefenition(TFManager, projectCollectionName, teamProjectName,
                workItemTypeName, fullPath);
        }

        #endregion

        #region Commands

        public DelegateCommand BrowseCommand { get; set; }

        public DelegateCommand ExportCommand { get; set; }

        #endregion
    }
}
