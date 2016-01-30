using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TfsWitAdminTools.Cmn
{
    public class Work : IWork
    {
        #region Fields

        int _steps;
        int _currentStep;
        int _failedSteps;

        #endregion

        #region Props

        public bool IsFinished
        {
            get
            {
                return (_currentStep == _steps - 1);
            }
        }

        #endregion

        #region Ctor 

        public Work()
        {

        }

        #endregion

        #region Methods

        public void InitWork(int stepsCount)
        {
            _steps = stepsCount;
            _currentStep = -1;
            _failedSteps = 0;
        }

        public void NextStep()
        {
            var lastStep = _steps - 1;
            if (_currentStep < lastStep)
                _currentStep++;
            else
                throw new Exception("Progress is finished!!");

            if (IsFinished)
            {
                var eventArgs = new ProgressFinishedEventArgs()
                {
                    Steps = _steps,
                    CurrentStep = _currentStep,
                    FailedSteps = _failedSteps
                };
                OnWorkFinished(eventArgs);
            }
        }

        public void FailStep()
        {
            _failedSteps++;

            NextStep();
        }

        public void EndWork()
        {
            var eventArgs = new ProgressFinishedEventArgs()
            {
                Steps = _steps,
                CurrentStep = _currentStep,
                FailedSteps = _failedSteps
            };
            if (!IsFinished)
                OnWorkFinished(eventArgs);
        }

        public void Reset()
        {
            _currentStep = -1;
        }

        #endregion

        #region Events 

        public event EventHandler<ProgressFinishedEventArgs> WorkFinished;

        protected virtual void OnWorkFinished(ProgressFinishedEventArgs e)
        {
            if (WorkFinished != null)
                System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(WorkFinished, this, e);
        }

        #endregion
    }
}
