
namespace TfsWitAdminTools.ViewModel
{
    public class WIDViewerVM : ToolsChildVM
    {
        #region Ctor

        public WIDViewerVM(ToolsVM server)
            : base(server)
        {
            ShowCommand = new DelegateCommand(() =>
            {
                string projectCollectionName = Server.CurrentProjectCollection.Name;
                string teamProjectName = Server.CurrentTeamProject.Name;
                string workItemTypeName = Server.CurrentWorkItemType.Name;
                string workItemTypeDefenition = Server.WIAdminService
                    .ExportWorkItemDefenition(TFManager, projectCollectionName, teamProjectName, workItemTypeName);

                Server.CurrentWorkItemType.Defenition = workItemTypeDefenition;
            },
            () => (
                Server.CurrentProjectCollection != null && Server.CurrentTeamProject != null &&
                Server.CurrentWorkItemType != null
                )
            );
        }

        #endregion

        #region Commands

        public DelegateCommand ShowCommand { get; set; }

        #endregion
    }
}
