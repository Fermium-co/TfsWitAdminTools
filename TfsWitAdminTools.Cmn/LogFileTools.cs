using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsWitAdminTools.Cmn
{
    public class LogFileTools
    {
        public static void LogException(Exception exception)
        {
            var innerExceptionItem = exception.InnerException;
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("------------#Exception - {0} -----------------------------------------------------------------------------------",
                DateTime.Now.ToString()));
            sb.AppendLine(exception.GetErrorMessage()).AppendLine();
            var innerExceptionCount = 0;
            while (innerExceptionItem != null)
            {
                innerExceptionCount++;
                sb
                    .AppendLine(string.Format("Inner Exception #{0}: \n{1}\n\n Stack : \n{2}\n",
                    innerExceptionCount, innerExceptionItem.Message, innerExceptionItem.StackTrace.ToString()));

                innerExceptionItem = innerExceptionItem.InnerException;
            }
            sb.AppendLine();
            File.AppendAllText("LogFile.txt", sb.ToString(), Encoding.UTF8);
        }
    }
}
