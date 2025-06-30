using System.IO;

namespace WebTests.Utils
{
    public static class FileHelper
    {
        public static void SaveText(string filePath, string content)
        {
            File.WriteAllText(filePath, content);
        }
    }
}