using log4net;
using System.Reflection;

namespace WebTests.Utils
{
    public static class Logger
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static Logger()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public static void Info(string message) => log.Info(message);
        public static void Error(string message) => log.Error(message);
        public static void Warn(string message) => log.Warn(message);
    }
}