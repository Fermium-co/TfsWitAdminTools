﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsWitAdminTools.Cmn
{
    public interface IDialogProvider
    {
        string OpenBrowseDialog();

        string OpenFileDialog();

        string OpenFileDialog(bool IsFolderPicker);
    }
}
