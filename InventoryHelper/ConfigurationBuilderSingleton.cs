using Microsoft.Extensions.Configuration;

namespace InventoryHelper
{
    public sealed class ConfigurationBuilderSingleton
    {
        private static ConfigurationBuilderSingleton _instance = null;
        private static readonly object _instanceLock = new();

        private static IConfigurationRoot _configuration;

        private ConfigurationBuilderSingleton()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("secrets.json", optional: true, reloadOnChange: true);

            _configuration = builder.Build();
        }

        public static ConfigurationBuilderSingleton Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        return _instance = new ConfigurationBuilderSingleton();
                    }
                }
                return _instance;
            }
        }

        public static IConfigurationRoot ConfigurationRoot
        {
            get
            {
                if (_configuration == null)
                {
                    _ = Instance;
                }
                return _configuration;
            }
        }
    }
}
