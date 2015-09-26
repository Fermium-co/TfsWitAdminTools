using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TfsWitAdminTools.Cmn;
using TfsWitAdminTools.Core;
using TfsWitAdminTools.Model;
using TfsWitAdminTools.Service;

namespace TfsWitAdminTools.ViewModel
{
    public class ToolsVM : ViewModelBase
    {
        #region Ctor

        public ToolsVM(IWitAdminService wiAdminService)
        {
            Init(wiAdminService);
        }

        private void Init(IWitAdminService wiAdminService)
        {
            this.WIAdminService = wiAdminService;

            TargetTemplateName = "Agile";

            #region Commands

            SetAddressCommand = new DelegateCommand(() =>
            {
                TFManager = DiManager.Current.Resolve<TFManager>(new { serverAddress = Address });
                GetProjectCollectionInfosCommand.Execute(this);
                InitChildViewModelsCommand.Execute(this);
            },
            () => !string.IsNullOrEmpty(Address)
            );

            GetProjectCollectionInfosCommand = new DelegateCommand(() =>
            {
                ProjectCollectionInfos = GetProjectCollectionInfos(TFManager);
            },
            () => TFManager != null
            );

            GetAllTeamProjectsWITypesCommand = new DelegateCommand(() =>
            {
                var teamProjects = CurrentProjectCollection.TeamProjectInfos;
                foreach (var teamProject in teamProjects)
                {
                    GetWITypes(teamProject);
                }
            }, () => CurrentProjectCollection != null);

            GetWITypesCommand = new DelegateCommand(() =>
            {
                var currentTeamProject = CurrentTeamProject;
                GetWITypes(currentTeamProject);
            },
            () => (CurrentProjectCollection != null && CurrentTeamProject != null));

            InitChildViewModelsCommand = new DelegateCommand(() =>
            {
                WIDViewer = DiManager.Current.Resolve<WIDViewerVM>(new { server = this });
                WIDExport = DiManager.Current.Resolve<WIDExportVM>(new { server = this });
                WIDImport = DiManager.Current.Resolve<WIDImportVM>(new { server = this });
                WIDRename = DiManager.Current.Resolve<WIDRenameVM>(new { server = this });
                CategoryViewer = DiManager.Current.Resolve<CategoryViewerVM>(new { server = this });
                CategoryExport = DiManager.Current.Resolve<CategoryExportVM>(new { server = this });
                CategoryImport = DiManager.Current.Resolve<CategoryImportVM>(new { server = this });
            },
            () => TFManager != null);

            ClearOutputCommand = new DelegateCommand(() =>
            {
                Output = string.Empty;
            },
            () => !string.IsNullOrEmpty(Output));

            #endregion

            #region Events

            wiAdminService.CommandInvoked += wiAdminService_CommandInvoked;

            #endregion
        }

        #endregion

        #region Props

        #region Address

        private string _address;

        public string Address
        {
            get { return _address; }
            set
            {
                if (Set(ref _address, value))
                {
                    SetAddressCommand.Execute(this);
                    RaiseCommandsCanExecute();
                }
            }
        }

        #endregion

        #region TargetTemplateName

        private string _TargetTemplateName;

        public string TargetTemplateName
        {
            get { return _TargetTemplateName; }
            set { Set(ref _TargetTemplateName, value); }
        }

        #endregion

        #region ProjectCollectionInfo

        private List<ProjectCollectionInfo> _projectCollectionInfos;

        public List<ProjectCollectionInfo> ProjectCollectionInfos
        {
            get { return _projectCollectionInfos; }
            set { Set(ref _projectCollectionInfos, value); }
        }


        private ProjectCollectionInfo _currentProjectCollection;

        public ProjectCollectionInfo CurrentProjectCollection
        {
            get { return _currentProjectCollection; }
            set
            {
                if (Set(ref _currentProjectCollection, value))
                    RaiseCommandsCanExecute();
            }
        }

        #endregion

        #region TeamProjectInfo

        private TeamProjectInfo _currentTeamProject;

        public TeamProjectInfo CurrentTeamProject
        {
            get { return _currentTeamProject; }
            set
            {
                if (Set(ref  _currentTeamProject, value))
                    RaiseCommandsCanExecute();
            }
        }

        #endregion WorkItemTypeInfos

        #region WorkItemType

        private List<WorkItemTypeInfo> _workItemTypeInfos;

        public List<WorkItemTypeInfo> WorkItemTypeInfos
        {
            get { return _workItemTypeInfos; }
            set { Set(ref _workItemTypeInfos, value); }
        }

        private WorkItemTypeInfo _currentWorkItemType;

        public WorkItemTypeInfo CurrentWorkItemType
        {
            get { return _currentWorkItemType; }
            set
            {
                if (Set(ref  _currentWorkItemType, value))
                    RaiseCommandsCanExecute();
            }
        }

        #endregion

        #region Output

        private string _output;

        public string Output
        {
            get { return _output; }
            set { Set(ref _output, value); }
        }

        #endregion

        #endregion

        #region View Models

        public WIDViewerVM WIDViewer { get; set; }

        public WIDExportVM WIDExport { get; set; }

        public WIDImportVM WIDImport { get; set; }

        public WIDRenameVM WIDRename { get; set; }

        public CategoryViewerVM CategoryViewer { get; set; }

        public CategoryExportVM CategoryExport { get; set; }

        public CategoryImportVM CategoryImport { get; set; }

        #endregion

        #region Service Props

        public IWitAdminService WIAdminService { get; set; }

        public TFManager TFManager { get; set; }

        #endregion

        #region Methods

        private List<ProjectCollectionInfo> GetProjectCollectionInfos(TFManager tfManager)
        {
            var projectCollections = tfManager.projectCollections;
            var teamProjects = tfManager.teamProjects;
            var projectCollectionInfos = new List<ProjectCollectionInfo>();
            foreach (var projectCollection in projectCollections)
            {
                var teamProjectInfos = teamProjects[projectCollection.Key]
                    .Select(teamProjectItem =>
                        new TeamProjectInfo() { Name = teamProjectItem.Name, WorkItemTypeInfos = null, Categories = null }
                        ).ToArray();
                var projColInfo = new ProjectCollectionInfo() { Name = projectCollection.Key, TeamProjectInfos = teamProjectInfos };
                projectCollectionInfos.Add(projColInfo);
            }

            return projectCollectionInfos.ToList();
        }

        private async Task GetWITypes(TeamProjectInfo teamProject)
        {
            var projectCollectionName = CurrentProjectCollection.Name;
            var teamProjectName = teamProject.Name;
            var workItemTypeInfos =
                (await WIAdminService.ExportWorkItemTypes(TFManager, projectCollectionName, teamProjectName))
                .Select(workItemTypeString => new WorkItemTypeInfo() { Name = workItemTypeString, Defenition = null })
                .ToArray();

            teamProject.WorkItemTypeInfos = workItemTypeInfos;
        }

        #endregion

        #region Commands

        public DelegateCommand SetAddressCommand { get; set; }

        public DelegateCommand GetProjectCollectionInfosCommand { get; set; }

        public DelegateCommand GetAllTeamProjectsWITypesCommand { get; set; }

        public DelegateCommand GetWITypesCommand { get; set; }

        public DelegateCommand InitChildViewModelsCommand { get; set; }

        private void RaiseCommandsCanExecute()
        {
            GetAllTeamProjectsWITypesCommand.RaiseCanExecuteChanged();
            GetWITypesCommand.RaiseCanExecuteChanged();

            WIDViewer.ShowCommand.RaiseCanExecuteChanged();
            WIDExport.ExportCommand.RaiseCanExecuteChanged();
            WIDImport.ImportCommand.RaiseCanExecuteChanged();
            WIDRename.RenameCommand.RaiseCanExecuteChanged();

            CategoryViewer.ShowCommand.RaiseCanExecuteChanged();
            CategoryExport.ExportCommand.RaiseCanExecuteChanged();
            CategoryImport.ImportCommand.RaiseCanExecuteChanged();
        }

        public DelegateCommand ClearOutputCommand { get; set; }

        #endregion

        #region Events

        void wiAdminService_CommandInvoked(object sender, CommandInvokedEventArgs e)
        {
            Output += string.Format("Command:\n{0}\nResult:\n{1}\n\n", e.Argument, e.Output);
        }

        #endregion
    }
}
