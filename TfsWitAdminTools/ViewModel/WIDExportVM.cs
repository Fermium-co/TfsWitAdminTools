using System.Threading.Tasks;
using TfsWitAdminTools.Cmn;
using TfsWitAdminTools.Core;
using TfsWitAdminTools.Model;

namespace TfsWitAdminTools.ViewModel
{
    public class WIDExportVM : ToolsChildVM
    {
        #region Ctor

        public WIDExportVM(ToolsVM server, IDialogProvider dialogProvider)
            : base(server)
        {
            this._dialogProvider = dialogProvider;

            #region Commands

            BrowseCommand = new DelegateCommand(() =>
            {
                string path = _dialogProvider.OpenBrowseDialog();
                Path = path;
            });

            //ExportCommand = new DelegateCommand(async () =>
            ExportCommand = new DelegateCommand(() =>
            {
                Server.IsWorrking = true;

                try
                {
                    Server.IsWorrking = true;

                //    await Task.Factory.StartNew(() =>
                //    {
                        Export();
                //    });
                }
                finally
                {
                    Server.IsWorrking = false;
                }
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

        #region Fields

        private IDialogProvider _dialogProvider;

        #endregion

        #region Methods

        private void Export()
        {
            ProjectCollectionInfo projectCollection = Server.CurrentProjectCollection;

            TeamProjectInfo[] teamProjects;

            if (IsAllTeamProjects)
            {
                Server.GetAllTeamProjectsWITypesCommand.Execute(this);
                teamProjects = projectCollection.TeamProjectInfos;
            }
            else
                teamProjects = new TeamProjectInfo[] { Server.CurrentTeamProject };

            foreach (TeamProjectInfo teamProject in teamProjects)
            {
                WorkItemTypeInfo[] workItemTypeInfos;
                string path = Path;
                if (IsAllWorkItemTypes)
                {
                    if (teamProject.WorkItemTypeInfos == null)
                        Server.GetWITypesCommand.Execute(this);
                    workItemTypeInfos = teamProject.WorkItemTypeInfos;
                    path = System.IO.Path.Combine(path, teamProject.Name);
                    System.IO.Directory.CreateDirectory(path);
                }
                else
                    workItemTypeInfos = new WorkItemTypeInfo[] { Server.CurrentWorkItemType };

                foreach (WorkItemTypeInfo workItemTypeInfo in workItemTypeInfos)
                    Export(projectCollection, teamProject, workItemTypeInfo, path);
            }
        }

        private void Export(ProjectCollectionInfo projectCollection, TeamProjectInfo teamProject, WorkItemTypeInfo workItemType, string path)
        {
            string projectCollectionName = projectCollection.Name;
            string teamProjectName = teamProject.Name;
            string workItemTypeName = workItemType.Name;

            string fileName = string.Format("{0}.xml", workItemTypeName);
            string fullPath = path = System.IO.Path.Combine(path, fileName);

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
