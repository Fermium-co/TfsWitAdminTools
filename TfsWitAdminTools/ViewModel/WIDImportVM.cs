using System.ComponentModel;
using System.Threading.Tasks;
using TfsWitAdminTools.Cmn;
using TfsWitAdminTools.Core;
using TfsWitAdminTools.Model;

namespace TfsWitAdminTools.ViewModel
{
    public class WIDImportVM : ToolsChildVM
    {
        #region Ctor

        public WIDImportVM(ToolsVM tools, IDialogProvider dialogProvider)
            : base(tools)
        {
            this._dialogProvider = dialogProvider;

            #region Commands

            BrowseCommand = new DelegateCommand(() =>
            {
                string fileName = _dialogProvider.OpenFileDialog();
                FileName = fileName;
            });

            ImportCommand = new DelegateCommand(async () =>
            {
                await Import();
            },
            () => (
                Tools.CurrentProjectCollection != null &&
                (IsAllTeamProjects == true || Tools.CurrentTeamProject != null) &&
                !string.IsNullOrEmpty(FileName)
                )
            );

            #endregion
        }

        #endregion

        #region Props

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

        #endregion

        #region Fields

        private IDialogProvider _dialogProvider;

        #endregion

        #region Methods

        private async Task Import()
        {

            TeamProjectInfo[] teamProjects = null;
            if (IsAllTeamProjects)
                teamProjects = Tools.CurrentProjectCollection.TeamProjectInfos;
            else
                teamProjects = new TeamProjectInfo[] { Tools.CurrentTeamProject };

            Tools.Progress.BeginWorking(teamProjects.Length);
            try
            {
                foreach (TeamProjectInfo teamProject in teamProjects)
                {
                    string projectCollectionName = Tools.CurrentProjectCollection.Name;
                    string teamProjectName = teamProject.Name;

                    try
                    {
                        await Tools.WitAdminService.ImportWorkItemDefenition(TFManager, projectCollectionName, teamProjectName, FileName);
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

        #endregion

        #region Commands

        public DelegateCommand BrowseCommand { get; set; }

        public DelegateCommand ImportCommand { get; set; }

        public DelegateCommand ImportAllCommand { get; set; }

        #endregion
    }
}
