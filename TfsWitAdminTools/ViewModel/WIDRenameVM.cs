
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
                string projectCollectionName = Server.CurrentProjectCollection.Name;
                string teamProjectName = Server.CurrentTeamProject.Name;
                string workItemTypeName = Server.CurrentWorkItemType.Name;

                Server.WIAdminService.RenameWorkItem(TFManager, projectCollectionName, teamProjectName, workItemTypeName,
                    NewName);
            },
            () => (
                Server.CurrentProjectCollection != null && Server.CurrentTeamProject != null &&
                Server.CurrentWorkItemType != null && !string.IsNullOrEmpty(NewName)
                )
            );
        }

        #endregion

        #region Props

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
