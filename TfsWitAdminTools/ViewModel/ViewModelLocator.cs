using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TfsWitAdminTools.Core;

namespace TfsWitAdminTools.ViewModel
{
    public class ViewModelLocator
    {
        public MainVM MainVM
        {
            get
            {
                return DiManager.Current.Resolve<MainVM>();
            }
        }
    }
}
