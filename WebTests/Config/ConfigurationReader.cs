using Microsoft.Extensions.Configuration;
using System.IO;

namespace WebTests.Config
{
    public static class ConfigurationReader
    {
        private static IConfigurationRoot configuration;

        static ConfigurationReader()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Config/appsettings.json", optional: false, reloadOnChange: true);

            configuration = builder.Build();
        }

        public static string BaseUrl => configuration["BaseUrl"];
        public static string ExcelPath => configuration["ExcelPath"];
        public static string OutputPath => configuration["OutputPath"];
        public static string LogPath => configuration["LogPath"];
        public static string Browser => configuration["Browser"];
    }
}