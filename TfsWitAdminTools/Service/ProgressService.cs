using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TfsWitAdminTools.Core;

namespace TfsWitAdminTools.Service
{
    public class ProgressService : IProgressService
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
                return (_currentStep == _steps);
            }
        }

        #endregion

        #region Ctor 

        public ProgressService()
        {

        }

        #endregion

        #region Methods

        public void InitWork(int stepsCount)
        {
            _steps = stepsCount;
            _currentStep = 0;
            _failedSteps = 0;
        }

        public void NextStep()
        {
            var lastStep = _steps - 1;
            if (_currentStep <= lastStep)
                _currentStep++;
            else
                throw new Exception("Progress is finished!!");

            if (_currentStep > lastStep)
            {
                var eventArgs = new ProgressFinishedEventArgs()
                {
                    Steps = _steps,
                    CurrentStep = _currentStep,
                    FailedSteps = _failedSteps
                };
                OnProgressFinished(eventArgs);
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
                OnProgressFinished(eventArgs);
        }

        public void Reset()
        {
            _currentStep = -1;
        }

        #endregion

        #region Events 

        public event EventHandler<ProgressFinishedEventArgs> ProgressFinished;

        protected virtual void OnProgressFinished(ProgressFinishedEventArgs e)
        {
            if (ProgressFinished != null)
                System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(ProgressFinished, this, e);
        }

        #endregion
    }
}
