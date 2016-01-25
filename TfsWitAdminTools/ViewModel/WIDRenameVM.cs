
using System.Threading.Tasks;
using TfsWitAdminTools.Core;
using TfsWitAdminTools.Model;
namespace TfsWitAdminTools.ViewModel
{
    public class WIDRenameVM : ToolsChildVM
    {
        #region Ctor

        public WIDRenameVM(ToolsVM tools)
            : base(tools)
        {
            RenameCommand = new DelegateCommand(async () =>
            {
                await Rename();
            },
            () => (
                Tools.CurrentProjectCollection != null &&
                (IsAllTeamProjects == true || Tools.CurrentTeamProject != null) &&
                Tools.CurrentWorkItemType != null &&
                !string.IsNullOrEmpty(NewName)
                )
            );
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
                    RenameCommand.RaiseCanExecuteChanged();
            }
        }

        private string _newName;
        public string NewName
        {
            get { return _newName; }
            set
            {
                if (Set(ref _newName, value))
                    RenameCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Methods

        private async Task Rename()
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
                    string workItemTypeName = Tools.CurrentWorkItemType.Name;

                    try
                    {
                        await Tools.WitAdminService.RenameWorkItem(TFManager, projectCollectionName, teamProjectName, workItemTypeName,
                            NewName);
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

        public DelegateCommand RenameCommand { get; set; }


        #endregion
    }
}
