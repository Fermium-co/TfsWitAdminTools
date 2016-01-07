using TfsWitAdminTools.Cmn;

namespace TfsWitAdminTools.ViewModel
{
    public class ToolsChildVM : ViewModelBase
    {
        #region Ctor

        public ToolsChildVM(ToolsVM tools)
        {
            this.Tools = tools;
        }

        #endregion

        #region Props

        private ToolsVM _tools;

        public ToolsVM Tools
        {
            get { return _tools; }
            private set
            {
                Set(ref _tools, value);
            }
        }

        public ITFManager TFManager
        {
            get
            {
                return Tools.TFManager;
            }
        }

        #endregion
    }
}