using System.Threading.Tasks;
using TfsWitAdminTools.Cmn;
using TfsWitAdminTools.Core;
using TfsWitAdminTools.Model;

namespace TfsWitAdminTools.ViewModel
{
    public class CategoriesExportVM : ToolsChildVM
    {
        #region Ctor

        public CategoriesExportVM(ToolsVM tools, IDialogProvider dialogProvider)
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
                try
                {
                    Tools.BeginWorking();

                    await Export();
                }
                finally
                {
                    Tools.EndWorking();
                }
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

            foreach (TeamProjectInfo teamProject in teamProjects)
                Export(projectCollection, teamProject, Path);
        }

        private void Export(ProjectCollectionInfo projectCollection, TeamProjectInfo teamProject, string path)
        {
            string projectCollectionName = projectCollection.Name;
            string teamProjectName = teamProject.Name;

            string fileName = string.Format("{0}.xml", teamProjectName);
            string fullPath = System.IO.Path.Combine(path, fileName);

            Tools.WIAdminService.ExportCategories(TFManager, projectCollectionName, teamProjectName, fullPath);
        }

        #endregion

        #region Commands

        public DelegateCommand BrowseCommand { get; set; }

        public DelegateCommand ExportCommand { get; set; }

        #endregion
    }
}
