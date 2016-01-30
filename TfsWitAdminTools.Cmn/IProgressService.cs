using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsWitAdminTools.Cmn
{
    public interface IProgressService
    {
        event EventHandler<ProgressFinishedEventArgs> WorkFinished;

        bool IsWorrking { get; }

        void BeginWorking(int? stepsCount = null);

        void NextStep();

        void FailStep();

        void EndWorking();
    }
}
