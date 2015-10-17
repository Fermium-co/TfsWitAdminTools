using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

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

            PropertyChangingEventHandler tempPropertyChanging = PropertyChanging;

            if (tempPropertyChanging != null)
            {
                if (Dispatcher.CurrentDispatcher != null)
                {
                    Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
                    {
                        tempPropertyChanging(this, new PropertyChangingEventArgs(propName));
                    }));
                }
            }
            //if (PropertyChanging != null)
            //    PropertyChanging(this, new PropertyChangingEventArgs(propName));

            prop = value;

            PropertyChangedEventHandler tempPropertyChanged = PropertyChanged;

            if (tempPropertyChanged != null)
            {
                if (Dispatcher.CurrentDispatcher != null)
                {
                    Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
                    {
                        tempPropertyChanged(this, new PropertyChangedEventArgs(propName));
                    }));
                }
            }
            //if (PropertyChanged != null)
            //    PropertyChanged(this, new PropertyChangedEventArgs(propName));

            return true;
        }
    }
}
