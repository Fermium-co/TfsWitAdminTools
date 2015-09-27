using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsWitAdminTools.Cmn
{
     public class DialogProvider : IDialogProvider
     {
        public string OpenBrowseDialog()
        {
            return OpenFileDialog(true);
        }

        public string OpenFileDialog()
        {
            return OpenFileDialog(false);
        }

        public string OpenFileDialog(bool IsFolderPicker)
        {
            string fileName = string.Empty;
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = IsFolderPicker;
            CommonFileDialogResult dialogResult = dialog.ShowDialog();
            if (dialogResult == CommonFileDialogResult.Ok)
                fileName = dialog.FileName;

            return fileName;
        }
    }
}
