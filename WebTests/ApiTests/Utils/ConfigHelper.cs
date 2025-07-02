using Microsoft.Extensions.Configuration;
using System.IO;

namespace ApiTests.Utils
{
    /// <summary>
    /// Reads API configuration values from api.settings.json
    /// </summary>
    public static class ConfigHelper
    {
        private static IConfigurationRoot config;

        static ConfigHelper()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Config/api.settings.json", optional: false, reloadOnChange: true);

            config = builder.Build();
        }

        /// <summary>
        /// Trello API base URL
        /// </summary>
        public static string BaseUrl => config["BaseUrl"];

        /// <summary>
        /// Trello API key
        /// </summary>
        public static string ApiKey => config["ApiKey"];

        /// <summary>
        /// Trello token
        /// </summary>
        public static string Token => config["Token"];
    }
}