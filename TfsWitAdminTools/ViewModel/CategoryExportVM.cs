using TfsWitAdminTools.Cmn;
using TfsWitAdminTools.Core;

namespace TfsWitAdminTools.ViewModel
{
    public class CategoryExportVM : ToolsChildVM
    {
        #region Ctor

        public CategoryExportVM(ToolsVM server)
            : base(server)
        {
            #region Commands

            BrowseCommand = new DelegateCommand(() =>
            {
                var path = DiManager.Current.Resolve<IDialogProvider>().OpenBrowseDialog();
                Path = path;
            });

            ExportCommand = new DelegateCommand(() =>
            {
                var projectCollectionName = Server.CurrentProjectCollection.Name;
                var teamProjectName = Server.CurrentTeamProject.Name;

                var fileName = FileName;
                if (string.IsNullOrEmpty(fileName))
                    fileName = teamProjectName;
                fileName = string.Format("{0}.xml", fileName);
                var path = Path;
                var fullPath = path = System.IO.Path.Combine(path, fileName);

                Server.WIAdminService.ExportCategories(TFManager, projectCollectionName, teamProjectName, fullPath);
            },
            () => (
                Server.CurrentProjectCollection != null && Server.CurrentTeamProject != null && 
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

        #region Commands

        public DelegateCommand BrowseCommand { get; set; }

        public DelegateCommand ExportCommand { get; set; }

        #endregion
    }
}
