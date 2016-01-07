using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TfsWitAdminTools.Core;

namespace TfsWitAdminTools.ViewModel
{
    public class ProgressVM : ToolsChildVM
    {
        #region Fields and Props 

        private Stack<IProgressService> _worksStack = new Stack<IProgressService>();

        #region IsWorrking

        private bool _isWorrking;

        private bool IsWorrking
        {
            get { return _isWorrking; }
            set
            {
                if (Set(ref _isWorrking, value))
                    Tools.ClearOutputCommand.RaiseCanExecuteChanged();

                Mouse.OverrideCursor = (_isWorrking == true)
                    ? Cursors.Wait : null;
            }
        }

        public IProgressService CurrentProgressService
        {
            get
            {
                IProgressService currentProgressService = null;
                if (_worksStack.Any())
                    currentProgressService = _worksStack.Peek();

                return currentProgressService;
            }
        }

        #endregion

        #endregion

        #region Ctor

        public ProgressVM(ToolsVM tools)
            : base(tools)
        {

        }

        #endregion

        #region Events

        private void ProgressService_OnProgressFinished(object sender, ProgressFinishedEventArgs e)
        {
            var succeedCount = e.Steps - e.FailedSteps;
            Tools.Output += string.Format(
                "Operation finished : {0} Total, {1} Succeed, {2} Failed.\n\n",
                e.Steps, succeedCount, e.FailedSteps);
        }

        #endregion

        #region Methods

        public void BeginWorking(int? stepsCount = null)
        {
            if (IsWorrking == false)
                IsWorrking = true;

            var progressService = DiManager.Current.Resolve<IProgressService>();
            progressService.InitWork(stepsCount ?? 1);
            progressService.ProgressFinished += ProgressService_OnProgressFinished;

            _worksStack.Push(progressService);
        }

        public void NextStep()
        {
            var currentProgressService = CurrentProgressService;
            currentProgressService.NextStep();
        }

        public void FailStep()
        {
            var currentProgressService = CurrentProgressService;
            currentProgressService.FailStep();
        }

        public void EndWorking()
        {
            var currentProgressService = CurrentProgressService;
            currentProgressService.EndWork();

            _worksStack.Pop();

            if (!_worksStack.Any())
                IsWorrking = false;
        }

        #endregion
    }
}
