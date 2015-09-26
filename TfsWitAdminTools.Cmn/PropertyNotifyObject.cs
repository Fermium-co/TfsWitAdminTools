using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TfsWitAdminTools.Cmn
{
    public class PropertyNotifyObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public PropertyNotifyObject()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual bool SetValue<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new InvalidOperationException("Property name may not be null or empty.");

            if (EqualityComparer<T>.Default.Equals(property, value))
                return false;

            PropertyChangingEventHandler propertyChangingEventHandler = PropertyChanging;
            if (propertyChangingEventHandler != null)
                propertyChangingEventHandler(this, new PropertyChangingEventArgs(propertyName));

            property = value;

            PropertyChangedEventHandler propertyChangedEventHandler = PropertyChanged;
            if (propertyChangedEventHandler != null)
                propertyChangedEventHandler(this, new PropertyChangedEventArgs(propertyName));

            return true;
        }
    }
}
