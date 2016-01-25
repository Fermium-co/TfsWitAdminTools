
using System.Threading.Tasks;
using TfsWitAdminTools.Core;

namespace TfsWitAdminTools.ViewModel
{
    public class CategoriesViewerVM : ToolsChildVM
    {
        #region Ctor

        public CategoriesViewerVM(ToolsVM tools)
            : base(tools)
        {
            ShowCommand = new DelegateCommand(async () =>
            {
                await Show();
            },
            () =>
                (
                Tools.CurrentProjectCollection != null &&
                Tools.CurrentTeamProject != null
                )
            );
        }

        #endregion

        #region Props

        #endregion

        #region Methods

        private async Task Show()
        {
            string projectCollectionName = Tools.CurrentProjectCollection.Name;
            string teamProjectName = Tools.CurrentTeamProject.Name;
            string categories = null;

            Tools.Progress.BeginWorking();
            try
            {
                try
                {
                    categories = await Tools.WitAdminService
                        .ExportCategories(TFManager, projectCollectionName, teamProjectName);
                    Tools.Progress.NextStep();
                }
                catch (WitAdminException)
                {
                    Tools.Progress.FailStep();
                }
                Tools.CurrentTeamProject.Categories = categories;
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
