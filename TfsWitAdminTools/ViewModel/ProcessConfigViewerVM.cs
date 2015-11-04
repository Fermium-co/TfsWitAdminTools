using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsWitAdminTools.ViewModel
{
    public class ProcessConfigViewerVM : ToolsChildVM
    {
        #region Ctor

        public ProcessConfigViewerVM(ToolsVM tools)
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
            string processConfigs = Tools.WIAdminService
                .ExportProcessConfig(TFManager, projectCollectionName, teamProjectName);

            Tools.CurrentTeamProject.ProcessConfig = processConfigs;
        }

        #endregion

        #region Commands

        public DelegateCommand ShowCommand { get; set; }

        #endregion
    }
}
