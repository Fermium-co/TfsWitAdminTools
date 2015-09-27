using TfsWitAdminTools.Cmn;
using TfsWitAdminTools.Core;

namespace TfsWitAdminTools.ViewModel
{
    public class WIDImportVM : ToolsChildVM
    {
        #region Ctor

        public WIDImportVM(ToolsVM server, IDialogProvider dialogProvider)
            : base(server)
        {
            this._dialogProvider = dialogProvider;

            #region Commands

            BrowseCommand = new DelegateCommand(() =>
            {
                string fileName = _dialogProvider.OpenFileDialog();
                FileName = fileName;
            });

            ImportCommand = new DelegateCommand(() =>
            {
                Import();
            },
            () => (
                Server.CurrentProjectCollection != null && Server.CurrentTeamProject != null &&
                !string.IsNullOrEmpty(FileName)
                )
            );

            ImportAllCommand = new DelegateCommand(() =>
            {
                Import();
            },
            () => (
                Server.CurrentProjectCollection != null && Server.CurrentTeamProject != null &&
                !string.IsNullOrEmpty(FileName)
                )
            );

            #endregion
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

        #region Methods

        private void Import()
        {
            string projectCollectionName = Server.CurrentProjectCollection.Name;
            string teamProjectName = Server.CurrentTeamProject.Name;

            Server.WIAdminService.ImportWorkItemDefenition(TFManager, projectCollectionName,
                teamProjectName, FileName);
        }

        #endregion

        #region Commands

        public DelegateCommand BrowseCommand { get; set; }

        public DelegateCommand ImportCommand { get; set; }

        public DelegateCommand ImportAllCommand { get; set; }

        #endregion
    }
}
