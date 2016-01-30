using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsWitAdminTools.Cmn
{
    public class ProgressFinishedEventArgs : EventArgs
    {
        public int Steps { get; set; }

        public int CurrentStep { get; set; }

        public int FailedSteps { get; set; }
    }
}
