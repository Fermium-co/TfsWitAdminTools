using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsWitAdminTools.Cmn
{
    public interface IWork
    {
        event EventHandler<ProgressFinishedEventArgs> WorkFinished;

        bool IsFinished { get; }

        void InitWork(int stepsCount);

        void NextStep();

        void FailStep();

        void EndWork();

        void Reset();
    }
}
