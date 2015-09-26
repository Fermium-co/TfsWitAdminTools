using TfsWitAdminTools.Cmn;

namespace TfsWitAdminTools.Model
{
    public class WorkItemTypeInfo : PropertyNotifyObject
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                SetValue(ref _name, value);
            }
        }

        private string _defenition;
        public string Defenition
        {
            get { return _defenition; }
            set
            {
                SetValue(ref _defenition, value);
            }
        }
    }
}
