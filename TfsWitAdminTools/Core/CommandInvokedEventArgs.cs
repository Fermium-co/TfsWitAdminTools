using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsWitAdminTools.Core
{
    public class CommandInvokedEventArgs : EventArgs
    {
        public string Argument { get; set; }

        public string Output { get; set; }
    }
}
