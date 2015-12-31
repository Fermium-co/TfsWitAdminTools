
using System.Threading.Tasks;
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
                try
                {
                    Tools.BeginWorking();

                    await Rename();
                }
                finally
                {
                    Tools.EndWorking();
                }
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

            foreach (TeamProjectInfo teamProject in teamProjects)
            {
                string projectCollectionName = Tools.CurrentProjectCollection.Name;
                string teamProjectName = teamProject.Name;
                string workItemTypeName = Tools.CurrentWorkItemType.Name;

                await Tools.WIAdminService.RenameWorkItem(TFManager, projectCollectionName, teamProjectName, workItemTypeName,
                    NewName);
            }
        }

        #endregion

        #region Commands

        public DelegateCommand RenameCommand { get; set; }


        #endregion
    }
}
