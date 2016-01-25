using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsWitAdminTools.Cmn
{
    public static class ExceptionExt
    {
        public static string GetErrorMessage(this Exception e)
        {
            var errorMessage = string.Format("Error : \n{0}\n\n Stack : \n{1}\n",
                e.Message, 
                e.StackTrace == null ? string.Empty : e.StackTrace.ToString());

            return errorMessage;
        }
    }
}
