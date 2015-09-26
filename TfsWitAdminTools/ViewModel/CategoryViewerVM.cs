
namespace TfsWitAdminTools.ViewModel
{
    public class CategoryViewerVM : ToolsChildVM
    {
        #region Ctor

        public CategoryViewerVM(ToolsVM server)
            : base(server)
        {
            ShowCommand = new DelegateCommand(() =>
            {
                var projectCollectionName = Server.CurrentProjectCollection.Name;
                var teamProjectName = Server.CurrentTeamProject.Name;
                var categories = Server.WIAdminService
                    .ExportCategories(TFManager, projectCollectionName, teamProjectName);

                Server.CurrentTeamProject.Categories = categories;
            },
            () =>
                (Server.CurrentProjectCollection != null && Server.CurrentTeamProject != null)
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
