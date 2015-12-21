using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TfsWitAdminTools.Cmn;
using TfsWitAdminTools.Model;

namespace TfsWitAdminTools.ViewModel
{
    public class ProcessConfigImportVM : ToolsChildVM
    {
        #region Ctor

        public ProcessConfigImportVM(ToolsVM tools, IDialogProvider dialogProvider)
            : base(tools)
        {
            this._dialogProvider = dialogProvider;

            BrowseCommand = new DelegateCommand(() =>
            {
                string fileName = _dialogProvider.OpenFileDialog();
                FileName = fileName;
            });

            ImportCommand = new DelegateCommand(async () =>
            {
                try
                {
                    Tools.BeginWorking();

                    await Import();
                }
                finally
                {
                    Tools.EndWorking();
                }
            },
            () => (
                Tools.CurrentProjectCollection != null &&
                (IsAllTeamProjects || Tools.CurrentTeamProject != null) &&
                !string.IsNullOrEmpty(FileName)
                )
            );
        }

        #endregion

        #region Props

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (Set(ref _fileName, value))
                    ImportCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _isAllTeamProjects;

        public bool IsAllTeamProjects
        {
            get { return _isAllTeamProjects; }
            set
            {
                if (Set(ref _isAllTeamProjects, value))
                    ImportCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Fields

        private IDialogProvider _dialogProvider;

        #endregion

        #region Methods

        private async Task Import()
        {
            ProjectCollectionInfo projectCollection = Tools.CurrentProjectCollection;
            TeamProjectInfo[] teamProjects;

            teamProjects = (IsAllTeamProjects) 
                ? projectCollection.TeamProjectInfos
                : new TeamProjectInfo[] { Tools.CurrentTeamProject };

            foreach (TeamProjectInfo teamProject in teamProjects)
                Import(projectCollection, teamProject, FileName);
        }

        private void Import(ProjectCollectionInfo projectCollection, TeamProjectInfo teamProject, string fileName)
        {
            string projectCollectionName = projectCollection.Name;
            string teamProjectName = teamProject.Name;

            Tools.WIAdminService.ImportProcessConfig(TFManager, projectCollectionName, teamProjectName, fileName);
        }

        #endregion

        #region Commands

        public DelegateCommand BrowseCommand { get; set; }

        public DelegateCommand ImportCommand { get; set; }

        #endregion
    }
}
