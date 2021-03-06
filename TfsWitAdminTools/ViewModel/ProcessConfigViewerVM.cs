﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TfsWitAdminTools.Core;

namespace TfsWitAdminTools.ViewModel
{
    public class ProcessConfigViewerVM : ToolsChildVM
    {
        #region Ctor

        public ProcessConfigViewerVM(ToolsVM tools)
            : base(tools)
        {
            ShowCommand = new DelegateCommand(async () =>
            {
                await Show();
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

        private async Task Show()
        {
            string projectCollectionName = Tools.CurrentProjectCollection.Name;
            string teamProjectName = Tools.CurrentTeamProject.Name;
            string processConfigs = null;

            Tools.Progress.BeginWorking();
            try
            {
                try
                {
                    processConfigs = await Tools.WitAdminService
                        .ExportProcessConfig(TFManager, projectCollectionName, teamProjectName);
                    Tools.Progress.NextStep();
                }
                catch (WitAdminException)
                {
                    Tools.Progress.FailStep();
                }

                Tools.CurrentTeamProject.ProcessConfig = processConfigs;
            }
            finally
            {
                Tools.Progress.EndWorking();
            }
        }

        #endregion

        #region Commands

        public DelegateCommand ShowCommand { get; set; }

        #endregion
    }
}
