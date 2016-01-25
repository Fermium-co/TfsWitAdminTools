using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsWitAdminTools.Core
{
    public interface IWitAdminProcessService
    {
        string Output { get; }
        List<string> SplitedOutput { get; }
        string Error { get; }
        Task Start();
        void WaitForExit();
    }
}
