
using System.Threading.Tasks;
namespace TfsWitAdminTools.ViewModel
{
    public class WIDViewerVM : ToolsChildVM
    {
        #region Ctor

        public WIDViewerVM(ToolsVM tools)
            : base(tools)
        {
            ShowCommand = new DelegateCommand(async () =>
            {
                string projectCollectionName = Tools.CurrentProjectCollection.Name;
                string teamProjectName = Tools.CurrentTeamProject.Name;
                string workItemTypeName = Tools.CurrentWorkItemType.Name;
                string workItemTypeDefenition = Tools.WIAdminService
                    .ExportWorkItemDefenition(TFManager, projectCollectionName, teamProjectName, workItemTypeName);

                Tools.CurrentWorkItemType.Defenition = workItemTypeDefenition;
            },
            () => (
                Tools.CurrentProjectCollection != null && Tools.CurrentTeamProject != null &&
                Tools.CurrentWorkItemType != null
                )
            );
        }

        #endregion

        #region Commands

        public DelegateCommand ShowCommand { get; set; }

        #endregion
    }
}
