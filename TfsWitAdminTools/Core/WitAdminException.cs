using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsWitAdminTools.Core
{
    public class WitAdminException : Exception
    {
        public WitAdminException()
        {
        }

        public WitAdminException(string message)
        : base(message)
        {
        }

        public WitAdminException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
