using TfsWitAdminTools.Cmn;

namespace TfsWitAdminTools.ViewModel
{
    public class CategoryImportVM : ToolsChildVM
    {
        #region Ctor

        public CategoryImportVM(ToolsVM server)
            : base(server)
        {
            BrowseCommand = new DelegateCommand(() =>
            {
                var fileName = DialogTools.OpenFileDialog();
                FileName = fileName;
            });

            ImportCommand = new DelegateCommand(() =>
            {
                var projectCollectionName = Server.CurrentProjectCollection.Name;
                var teamProjectName = Server.CurrentTeamProject.Name;

                Server.WIAdminService.ImportCategories(TFManager, projectCollectionName, teamProjectName, FileName);
            },
            () => (
                Server.CurrentProjectCollection != null && Server.CurrentTeamProject != null &&
                !string.IsNullOrEmpty(FileName)
                )
            );
        }

        #endregion

        #region Props

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (Set(ref _fileName, value))
                    ImportCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Commands

        public DelegateCommand BrowseCommand { get; set; }

        public DelegateCommand ImportCommand { get; set; }

        #endregion
    }
}
