
using System.Threading.Tasks;
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
            string categories = await Tools.WIAdminService
                .ExportCategories(TFManager, projectCollectionName, teamProjectName);

            Tools.CurrentTeamProject.Categories = categories;
        }

        #endregion

        #region Commands

        public DelegateCommand ShowCommand { get; set; }

        #endregion
    }
}
