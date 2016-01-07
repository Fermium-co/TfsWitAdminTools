using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
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
            WitAdminService = wiAdminService;

            TargetTemplateName = "Agile";

            #region Commands

            SetAddressCommand = new DelegateCommand(() =>
            {
                TFManager = DiManager.Current.Resolve<ITFManager>(new { serverAddress = Address });
                InitChildViewModelsCommand.Execute(this);
                GetProjectCollectionInfosCommand.Execute(this);
            },
            () => !string.IsNullOrEmpty(Address)
            );

            ResetTFManagerCommand = new DelegateCommand(() =>
            {
                TFManager = null;
                ProjectCollectionInfos = null;
            });

            GetProjectCollectionInfosCommand = new DelegateCommand(async () =>
            {
                ProjectCollectionInfos = await GetProjectCollectionInfos(TFManager);
            },
            () => TFManager != null
            );

            GetAllTeamProjectsWITypesCommand = new DelegateCommand(async () =>
            {
                await GetAllTeamProjectsWITypes();
            }, () => CurrentProjectCollection != null);

            GetWITypesCommand = new DelegateCommand(async () =>
            {
                TeamProjectInfo currentTeamProject = CurrentTeamProject;
                await GetWITypes(currentTeamProject);
            },
            () => (CurrentProjectCollection != null && CurrentTeamProject != null));

            InitChildViewModelsCommand = new DelegateCommand(() =>
            {
                Progress = DiManager.Current.Resolve<ProgressVM>(new { tools = this });

                WIDViewer = DiManager.Current.Resolve<WIDViewerVM>(new { tools = this });
                WIDExport = DiManager.Current.Resolve<WIDExportVM>(new { tools = this });
                WIDImport = DiManager.Current.Resolve<WIDImportVM>(new { tools = this });
                WIDRename = DiManager.Current.Resolve<WIDRenameVM>(new { tools = this });
                WIDDestroy = DiManager.Current.Resolve<WIDDestroyVM>(new { tools = this });

                CategoriesViewer = DiManager.Current.Resolve<CategoriesViewerVM>(new { tools = this });
                CategoriesExport = DiManager.Current.Resolve<CategoriesExportVM>(new { tools = this });
                CategoriesImport = DiManager.Current.Resolve<CategoriesImportVM>(new { tools = this });

                ProcessConfigViewer = DiManager.Current.Resolve<ProcessConfigViewerVM>(new { tools = this });
                ProcessConfigExport = DiManager.Current.Resolve<ProcessConfigExportVM>(new { tools = this });
                ProcessConfigImport = DiManager.Current.Resolve<ProcessConfigImportVM>(new { tools = this });
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

        #region Firalds and Props

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
                if (Set(ref _currentTeamProject, value))
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
                if (Set(ref _currentWorkItemType, value))
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

        #endregion

        #region View Models

        private ProgressVM _progress;

        public ProgressVM Progress
        {
            get { return _progress; }
            set
            {
                if (Set(ref _progress, value))
                {
                    RaiseCommandsCanExecute();
                }
            }
        }

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

        private WIDDestroyVM _wiDDestroy;

        public WIDDestroyVM WIDDestroy
        {
            get { return _wiDDestroy; }
            set
            {
                if (Set(ref _wiDDestroy, value))
                {
                    RaiseCommandsCanExecute();
                }
            }
        }

        private CategoriesViewerVM _categoriesViewer;

        public CategoriesViewerVM CategoriesViewer
        {
            get { return _categoriesViewer; }
            set
            {
                if (Set(ref _categoriesViewer, value))
                {
                    RaiseCommandsCanExecute();
                }
            }
        }

        private CategoriesExportVM _categoriesExport;

        public CategoriesExportVM CategoriesExport
        {
            get { return _categoriesExport; }
            set
            {
                if (Set(ref _categoriesExport, value))
                {
                    RaiseCommandsCanExecute();
                }
            }
        }

        private CategoriesImportVM _categoriesImport;

        public CategoriesImportVM CategoriesImport
        {
            get { return _categoriesImport; }
            set
            {
                if (Set(ref _categoriesImport, value))
                {
                    RaiseCommandsCanExecute();
                }
            }
        }

        private ProcessConfigViewerVM _processConfigViewer;

        public ProcessConfigViewerVM ProcessConfigViewer
        {
            get { return _processConfigViewer; }
            set
            {
                if (Set(ref _processConfigViewer, value))
                {
                    RaiseCommandsCanExecute();
                }
            }
        }

        private ProcessConfigExportVM _processConfigExport;

        public ProcessConfigExportVM ProcessConfigExport
        {
            get { return _processConfigExport; }
            set
            {
                if (Set(ref _processConfigExport, value))
                {
                    RaiseCommandsCanExecute();
                }
            }
        }

        private ProcessConfigImportVM _processConfigImport;

        public ProcessConfigImportVM ProcessConfigImport
        {
            get { return _processConfigImport; }
            set
            {
                if (Set(ref _processConfigImport, value))
                {
                    RaiseCommandsCanExecute();
                }
            }
        }

        #endregion

        #region Service Props

        public IWitAdminService WitAdminService { get; private set; }

        public ITFManager TFManager { get; private set; }

        #endregion

        #region Methods

        private async Task<List<ProjectCollectionInfo>> GetProjectCollectionInfos(ITFManager tfManager)
        {
            var projectCollectionInfos = new List<ProjectCollectionInfo>();
            Progress.BeginWorking();
            try
            {
                await Task.Run(() =>
                {
                    var projectCollections = tfManager.ProjectCollections;
                    var teamProjects = tfManager.TeamProjects;

                    foreach (var projectCollection in projectCollections)
                    {
                        var teamProjectInfos = teamProjects[projectCollection.Key]
                            .Select(teamProjectItem =>
                                new TeamProjectInfo() { Name = teamProjectItem.Name, WorkItemTypeInfos = null, Categories = null, ProcessConfig = null }
                                ).ToArray();
                        var projColInfo = new ProjectCollectionInfo() { Name = projectCollection.Key, TeamProjectInfos = teamProjectInfos };
                        projectCollectionInfos.Add(projColInfo);
                    }
                });

                Progress.NextStep();
            }
            finally
            {
                Progress.EndWorking();
            }

            return projectCollectionInfos.ToList();
        }

        public async Task GetAllTeamProjectsWITypes()
        {
            try
            {
                TeamProjectInfo[] teamProjects = CurrentProjectCollection.TeamProjectInfos;
                Progress.BeginWorking(teamProjects.Length);
                foreach (var teamProject in teamProjects)
                {
                    try
                    {
                        await GetWITypes(teamProject);
                        Progress.NextStep();
                    }
                    catch (WitAdminException)
                    {
                        Progress.FailStep();
                    }
                }
            }

            finally
            {
                Progress.EndWorking();
            }
        }

        private async Task GetWITypes(TeamProjectInfo teamProject)
        {
            string projectCollectionName = CurrentProjectCollection.Name;
            string teamProjectName = teamProject.Name;
            WorkItemTypeInfo[] workItemTypeInfos = null;

            Progress.BeginWorking();
            try
            {
                try
                {
                    workItemTypeInfos =
                    (await WitAdminService.ExportWorkItemTypes(TFManager, projectCollectionName, teamProjectName))
                        .Select(workItemTypeString => new WorkItemTypeInfo() { Name = workItemTypeString, Defenition = null })
                        .ToArray();
                    Progress.NextStep();
                }
                catch (WitAdminException)
                {
                    Progress.FailStep();
                }

                teamProject.WorkItemTypeInfos = workItemTypeInfos;
            }
            finally
            {
                Progress.EndWorking();
            }
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
            if (WIDDestroy != null)
                WIDDestroy.DestroyCommand.RaiseCanExecuteChanged();

            if (CategoriesViewer != null)
                CategoriesViewer.ShowCommand.RaiseCanExecuteChanged();
            if (CategoriesExport != null)
                CategoriesExport.ExportCommand.RaiseCanExecuteChanged();
            if (CategoriesImport != null)
                CategoriesImport.ImportCommand.RaiseCanExecuteChanged();

            if (ProcessConfigViewer != null)
                ProcessConfigViewer.ShowCommand.RaiseCanExecuteChanged();
            if (ProcessConfigExport != null)
                ProcessConfigExport.ExportCommand.RaiseCanExecuteChanged();
            if (ProcessConfigImport != null)
                ProcessConfigImport.ImportCommand.RaiseCanExecuteChanged();
        }

        public DelegateCommand ClearOutputCommand { get; set; }

        #endregion

        #region Events

        void wiAdminService_CommandInvoked(object sender, CommandInvokedEventArgs e)
        {
            Output += string.Format("Command:\n{0}\nResult:\n{1}\n", e.Argument, e.Output);
        }

        #endregion
    }
}
