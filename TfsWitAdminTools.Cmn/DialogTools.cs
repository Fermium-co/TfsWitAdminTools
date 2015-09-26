using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsWitAdminTools.Cmn
{
    public class DialogTools
    {
        public static string OpenBrowseDialog()
        {
            return OpenFileDialog(true);
        }

        public static string OpenFileDialog()
        {
            return OpenFileDialog(false);
        }

        public static string OpenFileDialog(bool IsFolderPicker)
        {
            var fileName = string.Empty;
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = IsFolderPicker;
            var dialogResult = dialog.ShowDialog();
            if (dialogResult == CommonFileDialogResult.Ok)
                fileName = dialog.FileName;

            return fileName;
        }
    }
}
