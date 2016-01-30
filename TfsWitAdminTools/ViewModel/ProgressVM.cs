using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TfsWitAdminTools.Cmn;
using TfsWitAdminTools.Core;

namespace TfsWitAdminTools.ViewModel
{
    public class ProgressVM : ToolsChildVM
    {
        #region Fields and Props

        private IProgressService _progressService;

        private bool _isWorrking;

        private bool IsWorrking
        {
            get { return _isWorrking; }
            set
            {
                if (Set(ref _isWorrking, value))
                    Tools.ClearOutputCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Ctor

        public ProgressVM(ToolsVM tools)
            : base(tools)
        {
            _progressService = DiManager.Current.Resolve<IProgressService>();
        }

        #endregion

        #region Events

        private void ProgressService_OnProgressFinished(object sender, ProgressFinishedEventArgs e)
        {
            var succeedCount = e.Steps - (e.Steps - (e.CurrentStep + 1)) - e.FailedSteps;
            Tools.Output += string.Format(
                "Operation finished : {0} Total, {1} Succeed, {2} Failed.\n\n",
                e.Steps, succeedCount, e.FailedSteps);

            _progressService.WorkFinished -= ProgressService_OnProgressFinished;
        }

        #endregion

        #region Methods

        public void BeginWorking(int? stepsCount = null)
        {
            if (IsWorrking == false)
                IsWorrking = true;

            _progressService.BeginWorking(stepsCount ?? 1);
            _progressService.WorkFinished += ProgressService_OnProgressFinished;
        }

        public void NextStep()
        {
            var progressService = DiManager.Current.Resolve<IProgressService>();
            _progressService.NextStep();
        }

        public void FailStep()
        {
            _progressService.FailStep();
        }

        public void EndWorking()
        {
            _progressService.EndWorking();
            if (!_progressService.IsWorrking)
                IsWorrking = false;
        }

        #endregion
    }
}
