// FileHelper.cs
public static class FileHelper
{
    public static void SaveText(string outputPath, string content)
    {
        File.WriteAllText(outputPath, content);
        TestContext.WriteLine($"[INFO] Text saved to: {outputPath}");
    }
}
