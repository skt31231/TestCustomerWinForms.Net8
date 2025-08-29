using Microsoft.Extensions.Configuration;
using System.IO;

namespace TestCustomerWinForms.Net8
{
    public static class AppConfig
    {
        private static IConfigurationRoot? _config;

        public static IConfigurationRoot Configuration
        {
            get
            {
                if (_config != null) return _config;
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                _config = builder.Build();
                return _config;
            }
        }

        public static string ConnString =>
            Configuration.GetConnectionString("TestGPT") ?? "";
    }
}