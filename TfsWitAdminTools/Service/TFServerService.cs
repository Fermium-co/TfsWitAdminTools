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
            string firstServerUrl = _configProvider.GetConfig(ConfigKeys.FirstTfsUrl);
            return firstServerUrl;
        }

        public string GetSecondServerUrl()
        {
            string secondServerUrl = _configProvider.GetConfig(ConfigKeys.SecondTfsUrl);
            return secondServerUrl;
        }

        public string GetConfigValue(string key)
        {
            string configValue = _configProvider.GetConfig(key);
            return configValue;
        }

        public class ConfigKeys
        {
            public static readonly string FirstTfsUrl = "FirstTfsUrl";
            public static readonly string SecondTfsUrl = "SecondTfsUrl";
        }
    }
}
