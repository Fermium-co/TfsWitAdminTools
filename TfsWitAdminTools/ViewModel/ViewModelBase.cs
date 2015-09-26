using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TfsWitAdminTools.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        protected bool Set<T>(ref T prop, T value, [CallerMemberName]string propName = null)
        {
            if (EqualityComparer<T>.Default.Equals(prop, value))
                return false;

            if (PropertyChanging != null)
                PropertyChanging(this, new PropertyChangingEventArgs(propName));

            prop = value;

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));

            return true;
        }
    }
}
