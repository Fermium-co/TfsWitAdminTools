
using System.Threading.Tasks;
using TfsWitAdminTools.Core;

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
                await Show();
            },
            () => (
                Tools.CurrentProjectCollection != null && Tools.CurrentTeamProject != null &&
                Tools.CurrentWorkItemType != null
                )
            );
        }

        #endregion

        #region Methods

        private async Task Show()
        {
            string projectCollectionName = Tools.CurrentProjectCollection.Name;
            string teamProjectName = Tools.CurrentTeamProject.Name;
            string workItemTypeName = Tools.CurrentWorkItemType.Name;
            Tools.Progress.BeginWorking();
            try
            {
                string workItemTypeDefenition = null;
                try
                {
                    workItemTypeDefenition = await Tools.WitAdminService
                        .ExportWorkItemDefenition(TFManager, projectCollectionName, teamProjectName, workItemTypeName);
                    Tools.Progress.NextStep();
                }
                catch (WitAdminException)
                {
                    Tools.Progress.FailStep();
                }
                Tools.CurrentWorkItemType.Defenition = workItemTypeDefenition;
            }
            finally
            {
                Tools.Progress.EndWorking();
            }
        }

        #endregion

        #region Commands

        public DelegateCommand ShowCommand { get; set; }

        #endregion
    }
}
