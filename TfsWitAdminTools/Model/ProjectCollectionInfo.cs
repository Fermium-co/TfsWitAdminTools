using TfsWitAdminTools.Cmn;

namespace TfsWitAdminTools.Model
{
    public class ProjectCollectionInfo : PropertyNotifyObject
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

        public TeamProjectInfo[] _teamProjectInfos;
        public TeamProjectInfo[] TeamProjectInfos
        {
            get { return _teamProjectInfos; }
            set
            {
                SetValue(ref _teamProjectInfos, value);
            }
        }
    }
}
