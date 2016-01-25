using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsWitAdminTools.Core
{
    class DataContextInitException : Exception
    {
        public DataContextInitException()
        {
        }

        public DataContextInitException(string message)
        : base(message)
        {
        }

        public DataContextInitException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
