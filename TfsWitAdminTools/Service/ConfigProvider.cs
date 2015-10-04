using System.Configuration;
using TfsWitAdminTools.Core;

namespace TfsWitAdminTools.Service
{
    public class ConfigProvider : IConfigProvider
    {
        public string GetConfig(string configKey)
        {
            return ConfigurationManager.AppSettings.Get(configKey);
        }
    }
}
