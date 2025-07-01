using System.IO;
using NUnit.Framework;

public static class FileHelper
{
    public static void SaveText(string outputPath, string content)
    {
        var directory = Path.GetDirectoryName(outputPath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
            TestContext.WriteLine($"[INFO] Directory created: {directory}");
        }

        File.WriteAllText(outputPath, content);
        TestContext.WriteLine($"[INFO] Text saved to: {outputPath}");
    }
}
