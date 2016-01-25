using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TfsWitAdminTools.Cmn
{
    public class MessageBoxTools
    {
        public static MessageBoxResult ShowException(Exception exception)
        {
            return MessageBox.Show(exception.GetErrorMessage(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
