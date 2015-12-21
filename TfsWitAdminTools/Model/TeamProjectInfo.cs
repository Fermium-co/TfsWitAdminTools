using TfsWitAdminTools.Cmn;

namespace TfsWitAdminTools.Model
{
    public class TeamProjectInfo : PropertyNotifyObject
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

        private WorkItemTypeInfo[] _workItemTypeInfos;
        public WorkItemTypeInfo[] WorkItemTypeInfos
        {
            get { return _workItemTypeInfos; }
            set
            {
                SetValue(ref _workItemTypeInfos, value);
            }
        }

        private string _categories;
        public string Categories
        {
            get { return _categories; }
            set
            {
                SetValue(ref _categories, value);
            }
        }

        private string _processConfig;
        public string ProcessConfig
        {
            get { return _processConfig; }
            set
            {
                SetValue(ref _processConfig, value);
            }
        }
    }
}
