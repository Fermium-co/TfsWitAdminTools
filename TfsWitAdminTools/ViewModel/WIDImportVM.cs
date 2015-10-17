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

            ImportCommand = new DelegateCommand(() =>
            {
                //ToDo
                try
                {
                    Tools.IsWorrking = true;
                    Import();
                }
                finally
                {
                    if (Tools.IsWorrking)
                        Tools.IsWorrking = false;
                }

                //BackgroundWorker bw = new BackgroundWorker();
                //bw.DoWork += (sender, e) =>
                //    Import();

                //bw.RunWorkerCompleted += (sender, e) =>
                //    Tools.IsWorrking = false;

                //Tools.IsWorrking = true;
                //bw.RunWorkerAsync();
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

        private void Import()
        {
            TeamProjectInfo[] teamProjects = null;
            if(IsAllTeamProjects)
                teamProjects = Tools.CurrentProjectCollection.TeamProjectInfos;
            else
                teamProjects = new TeamProjectInfo[] { Tools.CurrentTeamProject };

            foreach (TeamProjectInfo teamProject in teamProjects)
            {
                string projectCollectionName = Tools.CurrentProjectCollection.Name;
                string teamProjectName = teamProject.Name;

                Tools.WIAdminService.ImportWorkItemDefenition(TFManager, projectCollectionName,teamProjectName, 
                    FileName);
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
