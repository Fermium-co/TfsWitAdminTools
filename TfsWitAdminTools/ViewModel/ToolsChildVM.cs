using TfsWitAdminTools.Cmn;

namespace TfsWitAdminTools.ViewModel
{
    public class ToolsChildVM : ViewModelBase
    {
        #region Ctor

        public ToolsChildVM(ToolsVM server)
        {
            this.Server = server;
        }

        #endregion

        #region Props

        public ToolsVM _server;

        public ToolsVM Server
        {
            get { return _server; }
            set
            {
                Set(ref _server, value);
            }
        }

        public ITFManager TFManager
        {
            get
            {
                return Server.TFManager;
            }
        }

        #endregion
    }
}