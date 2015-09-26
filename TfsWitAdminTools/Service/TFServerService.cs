using System.Configuration;
using TfsWitAdminTools.Core;

namespace TfsWitAdminTools.Service
{
    public class TFServerService : ITfsServerService
    {
        private readonly IConfigProvider _configProvider;

        public TFServerService(IConfigProvider configProvider)
        {
            this._configProvider = configProvider;
        }

        public string GetFirstServerUrl()
        {
            string mainServerUrl = _configProvider.GetConfig("TfsUrl");
            return mainServerUrl;
        }
    }
}
