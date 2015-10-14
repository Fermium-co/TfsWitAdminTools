using TfsWitAdminTools.Cmn;
using TfsWitAdminTools.Core;

namespace TfsWitAdminTools.ViewModel
{
    public class CategoryExportVM : ToolsChildVM
    {
        #region Ctor

        public CategoryExportVM(ToolsVM tools, IDialogProvider dialogProvider)
            : base(tools)
        {
            this._dialogProvider = dialogProvider;

            #region Commands

            BrowseCommand = new DelegateCommand(() =>
            {
                string path = dialogProvider.OpenBrowseDialog();
                Path = path;
            });

            ExportCommand = new DelegateCommand(() =>
            {
                string projectCollectionName = Tools.CurrentProjectCollection.Name;
                string teamProjectName = Tools.CurrentTeamProject.Name;

                string fileName = FileName;
                if (string.IsNullOrEmpty(fileName))
                    fileName = teamProjectName;
                fileName = string.Format("{0}.xml", fileName);
                string path = Path;
                string fullPath = path = System.IO.Path.Combine(path, fileName);

                Tools.WIAdminService.ExportCategories(TFManager, projectCollectionName, teamProjectName, fullPath);
            },
            () => (
                Tools.CurrentProjectCollection != null && Tools.CurrentTeamProject != null && 
                !string.IsNullOrEmpty(Path)
                )
            );

            #endregion
        }

        #endregion

        #region Props

        private string _path;
        public string Path
        {
            get { return _path; }
            set
            {
                if (Set(ref _path, value))
                    ExportCommand.RaiseCanExecuteChanged();
            }
        }

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set { Set(ref _fileName, value); }
        }

        #endregion

        #region Fields

        private IDialogProvider _dialogProvider;

        #endregion

        #region Commands

        public DelegateCommand BrowseCommand { get; set; }

        public DelegateCommand ExportCommand { get; set; }

        #endregion
    }
}
