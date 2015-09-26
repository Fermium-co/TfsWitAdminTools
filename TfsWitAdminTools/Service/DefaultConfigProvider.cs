using System.Configuration;
using TfsWitAdminTools.Core;

namespace TfsWitAdminTools.Service
{
    public class DefaultConfigProvider : IConfigProvider
    {
        public string GetConfig(string configKey)
        {
            return ConfigurationManager.AppSettings.Get(configKey);
        }
    }
}
