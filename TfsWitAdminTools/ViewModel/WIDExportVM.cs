using TfsWitAdminTools.Cmn;
using TfsWitAdminTools.Model;

namespace TfsWitAdminTools.ViewModel
{
    public class WIDExportVM : ToolsChildVM
    {
        #region Ctor

        public WIDExportVM(ToolsVM server)
            : base(server)
        {
            #region Commands

            BrowseCommand = new DelegateCommand(() =>
            {
                var path = DialogTools.OpenBrowseDialog();
                Path = path;
            });

            ExportCommand = new DelegateCommand(() =>
            {
                Export();
            },
            () => (
                Server.CurrentProjectCollection != null &&
                (IsAllTeamProjects == true || Server.CurrentTeamProject != null) &&
                (IsAllWorkItemTypes == true || Server.CurrentWorkItemType != null) &&
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

        #region Methods

        private void Export()
        {
            var projectCollection = Server.CurrentProjectCollection;

            TeamProjectInfo[] teamProjects;

            if (IsAllTeamProjects)
            {
                Server.GetAllTeamProjectsWITypesCommand.Execute(this);
                teamProjects = projectCollection.TeamProjectInfos;
            }
            else
                teamProjects = new TeamProjectInfo[] { Server.CurrentTeamProject };

            foreach (var teamProject in teamProjects)
            {
                WorkItemTypeInfo[] workItemTypeInfos;
                string path = Path;
                if (IsAllWorkItemTypes)
                {
                    workItemTypeInfos = teamProject.WorkItemTypeInfos;
                    path = System.IO.Path.Combine(path, teamProject.Name);
                    System.IO.Directory.CreateDirectory(path);
                }
                else
                    workItemTypeInfos = new WorkItemTypeInfo[] { Server.CurrentWorkItemType };

                foreach (var workItemTypeInfo in workItemTypeInfos)
                    Export(projectCollection, teamProject, workItemTypeInfo, path);
            }
        }

        private void Export(ProjectCollectionInfo projectCollection, TeamProjectInfo teamProject, WorkItemTypeInfo workItemType, string path)
        {
            var projectCollectionName = projectCollection.Name;
            var teamProjectName = teamProject.Name;
            var workItemTypeName = workItemType.Name;

            var fileName = string.Format("{0}.xml", workItemTypeName);
            var fullPath = path = System.IO.Path.Combine(path, fileName);

            Server.WIAdminService.ExportWorkItemDefenition(TFManager, projectCollectionName, teamProjectName,
                workItemTypeName, fullPath);
        }

        #endregion

        #region Commands

        public DelegateCommand BrowseCommand { get; set; }

        public DelegateCommand ExportCommand { get; set; }

        #endregion
    }
}
