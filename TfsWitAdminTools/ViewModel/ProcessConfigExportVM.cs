using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TfsWitAdminTools.Cmn;
using TfsWitAdminTools.Core;
using TfsWitAdminTools.Model;

namespace TfsWitAdminTools.ViewModel
{
    public class ProcessConfigExportVM : ToolsChildVM
    {
        #region Ctor

        public ProcessConfigExportVM(ToolsVM tools, IDialogProvider dialogProvider)
            : base(tools)
        {
            this._dialogProvider = dialogProvider;

            #region Commands

            BrowseCommand = new DelegateCommand(() =>
            {
                string path = dialogProvider.OpenBrowseDialog();
                Path = path;
            });

            ExportCommand = new DelegateCommand(async () =>
            {
                await Export();
            },
            () => (
                Tools.CurrentProjectCollection != null &&
                (IsAllTeamProjects || Tools.CurrentTeamProject != null) &&
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

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set { Set(ref _fileName, value); }
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

        #endregion

        #region Fields

        private IDialogProvider _dialogProvider;

        #endregion

        #region Methods

        private async Task Export()
        {
            ProjectCollectionInfo projectCollection = Tools.CurrentProjectCollection;
            TeamProjectInfo[] teamProjects;

            teamProjects = (IsAllTeamProjects)
                ? projectCollection.TeamProjectInfos
                : new TeamProjectInfo[] { Tools.CurrentTeamProject };

            Tools.Progress.BeginWorking(teamProjects.Length);
            try
            {
                foreach (TeamProjectInfo teamProject in teamProjects)
                {
                    try
                    {
                        await Export(projectCollection, teamProject, Path);
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

        private async Task Export(ProjectCollectionInfo projectCollection, TeamProjectInfo teamProject, string path)
        {
            string projectCollectionName = projectCollection.Name;
            string teamProjectName = teamProject.Name;

            string fileName = string.Format("{0}.xml", teamProjectName);
            string fullPath = System.IO.Path.Combine(path, fileName);

            await Tools.WitAdminService.ExportProcessConfig(TFManager, projectCollectionName, teamProjectName, fullPath);
        }

        #endregion

        #region Commands

        public DelegateCommand BrowseCommand { get; set; }

        public DelegateCommand ExportCommand { get; set; }

        #endregion
    }
}
