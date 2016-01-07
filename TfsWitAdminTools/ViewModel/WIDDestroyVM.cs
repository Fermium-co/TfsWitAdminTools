using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TfsWitAdminTools.Core;
using TfsWitAdminTools.Model;

namespace TfsWitAdminTools.ViewModel
{
    public class WIDDestroyVM : ToolsChildVM
    {
        #region Ctor

        public WIDDestroyVM(ToolsVM tools)
            : base(tools)
        {
            DestroyCommand = new DelegateCommand(async () =>
            {
                await Destroy();
            },
            () => (
                Tools.CurrentProjectCollection != null &&
                (IsAllTeamProjects == true || Tools.CurrentTeamProject != null) &&
                Tools.CurrentWorkItemType != null
                )
            );
        }

        #endregion

        #region Props

        private bool _isAllTeamProjects;
        public bool IsAllTeamProjects
        {
            get { return _isAllTeamProjects; }
            set
            {
                if (Set(ref _isAllTeamProjects, value))
                    DestroyCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Methods

        private async Task Destroy()
        {
            TeamProjectInfo[] teamProjects = null;
            if (IsAllTeamProjects)
                teamProjects = Tools.CurrentProjectCollection.TeamProjectInfos;
            else
                teamProjects = new TeamProjectInfo[] { Tools.CurrentTeamProject };

            Tools.Progress.BeginWorking(teamProjects.Length);
            try
            {
                foreach (TeamProjectInfo teamProject in teamProjects)
                {
                    string projectCollectionName = Tools.CurrentProjectCollection.Name;
                    string teamProjectName = teamProject.Name;
                    string workItemTypeName = Tools.CurrentWorkItemType.Name;

                    try
                    {
                        await Tools.WitAdminService.DestroyWorkItem(TFManager, projectCollectionName, teamProjectName, workItemTypeName);
                        Tools.Progress.NextStep();
                    }
                    catch (WitAdminException)
                    {
                        Tools.Progress.FailStep();
                    }
                }
            }
            finally
            {
                Tools.Progress.EndWorking();
            }
        }

        #endregion

        #region Commands

        public DelegateCommand DestroyCommand { get; set; }


        #endregion
    }
}
