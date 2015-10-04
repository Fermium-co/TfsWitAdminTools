
using TfsWitAdminTools.Model;
namespace TfsWitAdminTools.ViewModel
{
    public class WIDRenameVM : ToolsChildVM
    {
        #region Ctor

        public WIDRenameVM(ToolsVM server)
            : base(server)
        {
            RenameCommand = new DelegateCommand(() =>
            {
                Rename();
            },
            () => (
                Server.CurrentProjectCollection != null &&
                (IsAllTeamProjects == true || Server.CurrentTeamProject != null) &&
                Server.CurrentWorkItemType != null &&
                !string.IsNullOrEmpty(NewName)
                )
            );
        }

        private void Rename()
        {
            TeamProjectInfo[] teamProjects = null;
            if (IsAllTeamProjects)
                teamProjects = Server.CurrentProjectCollection.TeamProjectInfos;
            else
                teamProjects = new TeamProjectInfo[] { Server.CurrentTeamProject };

            foreach (TeamProjectInfo teamProject in teamProjects)
            {
                string projectCollectionName = Server.CurrentProjectCollection.Name;
                string teamProjectName = teamProject.Name;
                string workItemTypeName = Server.CurrentWorkItemType.Name;

                Server.WIAdminService.RenameWorkItem(TFManager, projectCollectionName, teamProjectName, workItemTypeName,
                    NewName);
            }
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

        #region Commands

        public DelegateCommand RenameCommand { get; set; }


        #endregion
    }
}
