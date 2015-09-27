using TfsWitAdminTools.Core;

namespace TfsWitAdminTools.ViewModel
{
    public class MainVM : ViewModelBase
    {
        #region Ctor

        public MainVM(ITfsServerService tfServerService, IWitAdminService wiAdminService, ToolsVM firstServer, ToolsVM secondServer)
        {
            this.FirstServer = firstServer;
            string firstServerUrl = tfServerService.GetFirstServerUrl();
            this.FirstServer.Address = firstServerUrl;
            this.SecondServer = secondServer;
        }

        #endregion

        #region Props

        private ToolsVM _firstServer;

        public ToolsVM FirstServer
        {
            get { return _firstServer; }
            private set
            {
                Set(ref _firstServer, value);
            }
        }

        private ToolsVM _secondServer;

        public ToolsVM SecondServer
        {
            get { return _secondServer; }
            private set
            {
                Set(ref _secondServer, value);
            }
        }

        #endregion
    }
}
