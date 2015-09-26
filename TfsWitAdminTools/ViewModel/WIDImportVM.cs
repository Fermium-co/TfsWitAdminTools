using TfsWitAdminTools.Cmn;

namespace TfsWitAdminTools.ViewModel
{
    public class WIDImportVM : ToolsChildVM
    {
        #region Ctor

        public WIDImportVM(ToolsVM server)
            : base(server)
        {
            BrowseCommand = new DelegateCommand(() =>
            {
                var fileName = DialogTools.OpenFileDialog();
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

        #region Methods

        private void Import()
        {
            var projectCollectionName = Server.CurrentProjectCollection.Name;
            var teamProjectName = Server.CurrentTeamProject.Name;

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
