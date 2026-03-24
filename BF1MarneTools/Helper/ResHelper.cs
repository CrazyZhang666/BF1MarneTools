namespace BF1MarneTools.Helper;

public static class ResHelper
{
    /// <summary>
    /// 获取嵌入资源流（自动添加前缀）
    /// </summary>
    public static Stream GetEmbeddedResourceStream(string resPath)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Assets.Files.{resPath}");
    }

    /// <summary>
    /// 获取嵌入资源文本内容
    /// </summary>
    public static string GetEmbeddedResourceText(string resPath)
    {
        using var stream = GetEmbeddedResourceStream(resPath);
        if (stream is null)
            return string.Empty;

        using var streamReader = new StreamReader(stream);
        return streamReader.ReadToEnd();
    }

    /// <summary>
    /// 从资源文件中抽取资源文件（默认覆盖源文件）
    /// </summary>
    public static void ExtractResFile(string resPath, string outputPath, bool isOverride = true)
    {
        // 如果输出文件存在，并且不覆盖文件，则退出
        if (!isOverride && File.Exists(outputPath))
            return;

        using var stream = GetEmbeddedResourceStream(resPath);
        if (stream is null)
            return;

        BufferedStream inStream = null;
        FileStream outStream = null;

        try
        {
            inStream = new BufferedStream(stream);
            outStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);

            var buffer = new byte[1024];
            int length;

            while ((length = inStream.Read(buffer, 0, buffer.Length)) > 0)
                outStream.Write(buffer, 0, length);

            outStream.Flush();

            LoggerHelper.Info($"释放资源文件成功 {outputPath}");
        }
        catch (Exception ex)
        {
            LoggerHelper.Error($"释放资源文件异常 {outputPath}", ex);
        }
        finally
        {
            outStream?.Close();
            inStream?.Close();
        }
    }

    /// <summary>
    /// 获取嵌入的压缩包内指定文件Crc32
    /// </summary>
    public static uint GetEmbeddedZipResFileCrc32(string zipResPath, string fileName)
    {
        // 读取嵌入的ZIP资源
        using var zipStream = GetEmbeddedResourceStream(zipResPath);
        if (zipStream is null)
            return 0;

        // 打开ZIP归档
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);

        // 遍历ZIP中的所有文件
        foreach (var entry in archive.Entries)
        {
            // 跳过目录项
            if (string.IsNullOrWhiteSpace(entry.Name))
                continue;

            // 如果不是目标文件，则跳过
            if (entry.Name != fileName)
                continue;

            return entry.Crc32;
        }

        return 0;
    }
}
