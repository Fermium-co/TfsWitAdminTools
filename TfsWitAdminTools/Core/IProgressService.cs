using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsWitAdminTools.Core
{
    public interface IProgressService
    {
        event EventHandler<ProgressFinishedEventArgs> ProgressFinished;

        bool IsFinished { get; }

        void InitWork(int stepsCount);

        void NextStep();

        void FailStep();

        void EndWork();

        void Reset();
    }
}
