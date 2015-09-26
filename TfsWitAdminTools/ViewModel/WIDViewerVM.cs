
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
                var projectCollectionName = Server.CurrentProjectCollection.Name;
                var teamProjectName = Server.CurrentTeamProject.Name;
                var workItemTypeName = Server.CurrentWorkItemType.Name;
                var workItemTypeDefenition = Server.WIAdminService
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
