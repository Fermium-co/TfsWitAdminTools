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
                TFManager = DiManager.Current.Resolve<ITFManager>(new { serverAddress = Address });
                GetProjectCollectionInfosCommand.Execute(this);
                InitChildViewModelsCommand.Execute(this);
            },
            () => !string.IsNullOrEmpty(Address)
            );

            ResetTFManagerCommand = new DelegateCommand(() =>
            {
                TFManager = null;
                ProjectCollectionInfos = null;
            });

            GetProjectCollectionInfosCommand = new DelegateCommand(() =>
            {
                ProjectCollectionInfos = GetProjectCollectionInfos(TFManager);
            },
            () => TFManager != null
            );

            GetAllTeamProjectsWITypesCommand = new DelegateCommand(() =>
            {
                TeamProjectInfo[] teamProjects = CurrentProjectCollection.TeamProjectInfos;
                foreach (var teamProject in teamProjects)
                {
                    GetWITypes(teamProject);
                }
            }, () => CurrentProjectCollection != null);

            GetWITypesCommand = new DelegateCommand(() =>
            {
                TeamProjectInfo currentTeamProject = CurrentTeamProject;
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
                    ResetTFManagerCommand.Execute(this);
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
            set
            {
                if (Set(ref _output, value))
                    ClearOutputCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region IsWorrking

        private bool _isWorrking;

        public bool IsWorrking
        {
            get { return _isWorrking; }
            set
            {
                if (Set(ref _isWorrking, value))
                    ClearOutputCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #endregion

        #region View Models

        private WIDViewerVM _wiDViewer;

        public WIDViewerVM WIDViewer
        {
            get { return _wiDViewer; }
            set
            {
                if (Set(ref _wiDViewer, value))
                {
                    RaiseCommandsCanExecute();
                }
            }
        }

        private WIDExportVM _wiDExport;

        public WIDExportVM WIDExport
        {
            get { return _wiDExport; }
            set
            {
                if (Set(ref _wiDExport, value))
                {
                    RaiseCommandsCanExecute();
                }
            }
        }

        private WIDImportVM _wiDImport;

        public WIDImportVM WIDImport
        {
            get { return _wiDImport; }
            set
            {
                if (Set(ref _wiDImport, value))
                {
                    RaiseCommandsCanExecute();
                }
            }
        }

        private WIDRenameVM _wiDRename;

        public WIDRenameVM WIDRename
        {
            get { return _wiDRename; }
            set
            {
                if (Set(ref _wiDRename, value))
                {
                    RaiseCommandsCanExecute();
                }
            }
        }

        private CategoryViewerVM _categoryViewer;

        public CategoryViewerVM CategoryViewer
        {
            get { return _categoryViewer; }
            set
            {
                if (Set(ref _categoryViewer, value))
                {
                    RaiseCommandsCanExecute();
                }
            }
        }

        private CategoryExportVM _categoryExport;

        public CategoryExportVM CategoryExport
        {
            get { return _categoryExport; }
            set
            {
                if (Set(ref _categoryExport, value))
                {
                    RaiseCommandsCanExecute();
                }
            }
        }

        private CategoryImportVM _categoryImport;

        public CategoryImportVM CategoryImport
        {
            get { return _categoryImport; }
            set
            {
                if (Set(ref _categoryImport, value))
                {
                    RaiseCommandsCanExecute();
                }
            }
        }

        #endregion

        #region Service Props

        public IWitAdminService WIAdminService { get; set; }

        public ITFManager TFManager { get; set; }

        #endregion

        #region Methods

        private List<ProjectCollectionInfo> GetProjectCollectionInfos(ITFManager tfManager)
        {
            var projectCollections = tfManager.ProjectCollections;
            var teamProjects = tfManager.TeamProjects;
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
            string projectCollectionName = CurrentProjectCollection.Name;
            string teamProjectName = teamProject.Name;
            var workItemTypeInfos =
                (await WIAdminService.ExportWorkItemTypes(TFManager, projectCollectionName, teamProjectName))
                .Select(workItemTypeString => new WorkItemTypeInfo() { Name = workItemTypeString, Defenition = null })
                .ToArray();

            teamProject.WorkItemTypeInfos = workItemTypeInfos;
        }

        #endregion

        #region Commands

        public DelegateCommand SetAddressCommand { get; set; }

        public DelegateCommand ResetTFManagerCommand { get; set; }

        public DelegateCommand GetProjectCollectionInfosCommand { get; set; }

        public DelegateCommand GetAllTeamProjectsWITypesCommand { get; set; }

        public DelegateCommand GetWITypesCommand { get; set; }

        public DelegateCommand InitChildViewModelsCommand { get; set; }

        private void RaiseCommandsCanExecute()
        {
            GetAllTeamProjectsWITypesCommand.RaiseCanExecuteChanged();
            GetWITypesCommand.RaiseCanExecuteChanged();
            SetAddressCommand.RaiseCanExecuteChanged();
            ClearOutputCommand.RaiseCanExecuteChanged();

            if (WIDViewer != null)
                WIDViewer.ShowCommand.RaiseCanExecuteChanged();
            if (WIDExport != null)
                WIDExport.ExportCommand.RaiseCanExecuteChanged();
            if (WIDImport != null)
                WIDImport.ImportCommand.RaiseCanExecuteChanged();
            if (WIDRename != null)
                WIDRename.RenameCommand.RaiseCanExecuteChanged();

            if (CategoryViewer != null)
                CategoryViewer.ShowCommand.RaiseCanExecuteChanged();
            if (CategoryExport != null)
                CategoryExport.ExportCommand.RaiseCanExecuteChanged();
            if (CategoryImport != null)
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
