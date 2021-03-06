﻿using TfsWitAdminTools.Core;

namespace TfsWitAdminTools.ViewModel
{
    public class MainVM : ViewModelBase
    {
        #region Ctor

        public MainVM(ITfsServerService tfServerService, ToolsVM firstTools, ToolsVM secondTools)
        {
            this.FirstTools = firstTools;
            string firstToolsUrl = tfServerService.GetFirstServerUrl();
            this.FirstTools.Address = firstToolsUrl;
            this.FirstTools.SetAddressCommand.Execute(this);

            this.SecondTools = secondTools;
            string secondToolsUrl = tfServerService.GetSecondServerUrl();
            this.SecondTools.Address = secondToolsUrl;
            this.SecondTools.SetAddressCommand.Execute(this);
        }

        #endregion

        #region Props

        private ToolsVM _firstTools;

        public ToolsVM FirstTools
        {
            get { return _firstTools; }
            private set
            {
                Set(ref _firstTools, value);
            }
        }

        private ToolsVM _secondTools;

        public ToolsVM SecondTools
        {
            get { return _secondTools; }
            private set
            {
                Set(ref _secondTools, value);
            }
        }

        #endregion
    }
}
