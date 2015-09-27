using TfsWitAdminTools.Cmn;
using TfsWitAdminTools.Core;

namespace TfsWitAdminTools.ViewModel
{
    public class CategoryImportVM : ToolsChildVM
    {
        #region Ctor

        public CategoryImportVM(ToolsVM server, IDialogProvider dialogProvider)
            : base(server)
        {
            this._dialogProvider = dialogProvider;

            BrowseCommand = new DelegateCommand(() =>
            {
                string fileName = _dialogProvider.OpenFileDialog();
                FileName = fileName;
            });

            ImportCommand = new DelegateCommand(() =>
            {
                string projectCollectionName = Server.CurrentProjectCollection.Name;
                string teamProjectName = Server.CurrentTeamProject.Name;

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

        #region Fields

        private IDialogProvider _dialogProvider;

        #endregion


        #region Commands

        public DelegateCommand BrowseCommand { get; set; }

        public DelegateCommand ImportCommand { get; set; }

        #endregion
    }
}
