
namespace TfsWitAdminTools.ViewModel
{
    public class CategoriesViewerVM : ToolsChildVM
    {
        #region Ctor

        public CategoriesViewerVM(ToolsVM tools)
            : base(tools)
        {
            ShowCommand = new DelegateCommand(() =>
            {
                Show();
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

        private void Show()
        {
            string projectCollectionName = Tools.CurrentProjectCollection.Name;
            string teamProjectName = Tools.CurrentTeamProject.Name;
            string categories = Tools.WIAdminService
                .ExportCategories(TFManager, projectCollectionName, teamProjectName);

            Tools.CurrentTeamProject.Categories = categories;
        }

        #endregion

        #region Commands

        public DelegateCommand ShowCommand { get; set; }

        #endregion
    }
}
