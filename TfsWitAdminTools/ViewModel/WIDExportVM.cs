using System.ComponentModel;
using System.Threading.Tasks;
using TfsWitAdminTools.Cmn;
using TfsWitAdminTools.Core;
using TfsWitAdminTools.Model;

namespace TfsWitAdminTools.ViewModel
{
    public class WIDExportVM : ToolsChildVM
    {
        #region Ctor

        public WIDExportVM(ToolsVM tools, IDialogProvider dialogProvider)
            : base(tools)
        {
            this._dialogProvider = dialogProvider;

            #region Commands

            BrowseCommand = new DelegateCommand(() =>
            {
                string path = _dialogProvider.OpenBrowseDialog();
                Path = path;
            });

            ExportCommand = new DelegateCommand(async () =>
            //ExportCommand = new DelegateCommand(() =>
            {
                //BackgroundWorker bw = new BackgroundWorker();
                //bw.DoWork += (sender, e) =>
                //    Export();

                //bw.RunWorkerCompleted += (sender, e) =>
                //    Tools.IsWorrking = false;

                //Tools.IsWorrking = true;
                //bw.RunWorkerAsync();

                //ToDo
                try
                {
                    Tools.IsWorrking = true;
                    await Export();
                }
                finally
                {
                    if (Tools.IsWorrking)
                        Tools.IsWorrking = false;
                }


                //try
                //{
                //    Tools.IsWorrking = true;

                //    //ToDo: using wait
                //    //await Task.Factory.StartNew(() =>
                //    Task.Factory.StartNew(() =>
                //    {
                //        Export();
                //    }).Wait();

                //    //await Export();

                //}
                //finally
                //{
                //    Tools.IsWorrking = false;
                //}
            },
            () => (
                Tools.CurrentProjectCollection != null &&
                (IsAllTeamProjects == true || Tools.CurrentTeamProject != null) &&
                (IsAllWorkItemTypes == true || Tools.CurrentWorkItemType != null) &&
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

        private async Task Export()
        {
            ProjectCollectionInfo projectCollection = Tools.CurrentProjectCollection;

            TeamProjectInfo[] teamProjects;

            if (IsAllTeamProjects)
            {
                //Tools.GetAllTeamProjectsWITypesCommand.Execute(this);
                teamProjects = projectCollection.TeamProjectInfos;

                foreach (TeamProjectInfo teamProject in teamProjects)
                {
                    if (teamProject.WorkItemTypeInfos == null)
                    {
                        //Tools.GetAllTeamProjectsWITypesCommand.Execute(this);
                        await Tools.GetAllTeamProjectsWITypes();
                        break;
                    }
                }
            }
            else
                teamProjects = new TeamProjectInfo[] { Tools.CurrentTeamProject };

            foreach (TeamProjectInfo teamProject in teamProjects)
            {
                WorkItemTypeInfo[] workItemTypeInfos;
                string path = Path;
                if (IsAllWorkItemTypes)
                {
                    //if (teamProject.WorkItemTypeInfos == null)
                    //    Tools.GetWITypesCommand.Execute(this);
                    workItemTypeInfos = teamProject.WorkItemTypeInfos;
                    path = System.IO.Path.Combine(path, teamProject.Name);
                    System.IO.Directory.CreateDirectory(path);
                }
                else
                    workItemTypeInfos = new WorkItemTypeInfo[] { Tools.CurrentWorkItemType };

                foreach (WorkItemTypeInfo workItemTypeInfo in workItemTypeInfos)
                    Export(projectCollection, teamProject, workItemTypeInfo, path);
                //System.Threading.Tasks.Task.Factory.StartNew(async () =>
                //    {
                //        await Export(projectCollection, teamProject, workItemTypeInfo, path);
                //    });
            }
        }

        private async Task Export(ProjectCollectionInfo projectCollection, TeamProjectInfo teamProject, WorkItemTypeInfo workItemType, string path)
        {
            string projectCollectionName = projectCollection.Name;
            string teamProjectName = teamProject.Name;
            string workItemTypeName = workItemType.Name;

            string fileName = string.Format("{0}.xml", workItemTypeName);
            string fullPath = System.IO.Path.Combine(path, fileName);

            Tools.WIAdminService.ExportWorkItemDefenition(TFManager, projectCollectionName, teamProjectName,
                workItemTypeName, fullPath);
        }

        #endregion

        #region Commands

        public DelegateCommand BrowseCommand { get; set; }

        public DelegateCommand ExportCommand { get; set; }

        #endregion
    }
}
