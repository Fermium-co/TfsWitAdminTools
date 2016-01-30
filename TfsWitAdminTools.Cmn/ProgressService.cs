using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace TfsWitAdminTools.Cmn
{
    public class ProgressService : IProgressService
    {
        #region Fields and Props 

        private Stack<IWork> _worksStack = new Stack<IWork>();

        #region IsWorrking

        private bool _isWorrking;

        public bool IsWorrking
        {
            get { return _isWorrking; }
            private set {
                _isWorrking = value;

                Mouse.OverrideCursor = (_isWorrking == true)
                    ? Cursors.Wait : null;
            }
        }

        private IWork CurrentWork
        {
            get
            {
                IWork currentProgressService = null;
                if (_worksStack.Any())
                    currentProgressService = _worksStack.Peek();

                return currentProgressService;
            }
        }

        private IWork _lastWork = null;

        #endregion

        #endregion

        #region Ctor

        public ProgressService()
        {

        }

        #endregion

        #region Events

        public event EventHandler<ProgressFinishedEventArgs> WorkFinished;

        protected virtual void OnWorkFinished(ProgressFinishedEventArgs e)
        {
            if (WorkFinished != null)
                System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(WorkFinished, this, e);
        }

        private void ProgressService_OnWorkFinished(object sender, ProgressFinishedEventArgs e)
        {
            OnWorkFinished(e);
            _lastWork.WorkFinished -= ProgressService_OnWorkFinished;
        }

        #endregion

        #region Methods

        public void BeginWorking(int? stepsCount = null)
        {
            if (IsWorrking == false)
                IsWorrking = true;

            var work = new Work();
            work.InitWork(stepsCount ?? 1);
            work.WorkFinished += ProgressService_OnWorkFinished;

            _worksStack.Push(work);
        }

        public void NextStep()
        {
            var currentProgressService = CurrentWork;
            currentProgressService.NextStep();
        }

        public void FailStep()
        {
            var currentProgressService = CurrentWork;
            currentProgressService.FailStep();
        }

        public void EndWorking()
        {
            var currentProgressService = CurrentWork;
            currentProgressService.EndWork();

            _lastWork = _worksStack.Pop();

            if (!_worksStack.Any())
                IsWorrking = false;
        }

        #endregion
    }
}
