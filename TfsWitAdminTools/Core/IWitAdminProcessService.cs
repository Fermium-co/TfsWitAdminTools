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
        void Start();
        void WaitForExit();
        string ReadToEnd();
        string ReadLine();
        bool IsEndOfStream();
    }
}
