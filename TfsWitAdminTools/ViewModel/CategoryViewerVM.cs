
namespace TfsWitAdminTools.ViewModel
{
    public class CategoryViewerVM : ToolsChildVM
    {
        #region Ctor

        public CategoryViewerVM(ToolsVM tools)
            : base(tools)
        {
            ShowCommand = new DelegateCommand(() =>
            {
                string projectCollectionName = Tools.CurrentProjectCollection.Name;
                string teamProjectName = Tools.CurrentTeamProject.Name;
                string categories = Tools.WIAdminService
                    .ExportCategories(TFManager, projectCollectionName, teamProjectName);

                Tools.CurrentTeamProject.Categories = categories;
            },
            () =>
                (Tools.CurrentProjectCollection != null && Tools.CurrentTeamProject != null)
            );
        }

        #endregion

        #region Props

        #endregion

        #region Commands

        public DelegateCommand ShowCommand { get; set; }

        #endregion
    }
}
