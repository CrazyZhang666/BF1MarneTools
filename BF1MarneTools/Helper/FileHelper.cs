namespace BF1MarneTools.Helper;

public static class FileHelper
{
    /// <summary>
    /// 创建MD5静态实例
    /// </summary>
    private static readonly MD5 _md5 = MD5.Create();
    /// <summary>
    /// 文件大小后缀名
    /// </summary>
    private static readonly string[] _suffixes = ["B", "KB", "MB", "GB", "TB", "PB"];

    /// <summary>
    /// UTF8无BOM编码
    /// </summary>
    public static readonly UTF8Encoding Encoding_UTF8_NoBOM = new(false);

    /// <summary>
    /// 创建文件夹
    /// </summary>
    public static void CreateDirectory(string targetPath)
    {
        var dirPath = targetPath;
        // 判断是文件还是文件夹
        if (Path.HasExtension(targetPath))
            dirPath = Path.GetDirectoryName(targetPath);

        Directory.CreateDirectory(dirPath);
    }

    /// <summary>
    /// 异步删除文件夹及其子文件夹
    /// </summary>
    public static Task DeleteDirectoryAsync(string dirPath)
    {
        return Task.Run(() =>
        {
            try
            {
                if (!Directory.Exists(dirPath))
                    return;

                Directory.Delete(dirPath, true);
            }
            catch { }
        });
    }

    /// <summary>
    /// 异步删除文件
    /// </summary>
    public static Task DeleteFileAsync(string filePath)
    {
        return Task.Run(() =>
        {
            try
            {
                if (!File.Exists(filePath))
                    return;

                File.Delete(filePath);
            }
            catch { }
        });
    }

    /// <summary>
    /// 异步复制文件
    /// </summary>
    public static Task CopyFileAsync(string oldPath, string newPath)
    {
        return Task.Run(() =>
        {
            if (!File.Exists(oldPath))
                return;

            // 智能自动创建文件夹
            CreateDirectory(newPath);

            File.Copy(oldPath, newPath, true);
        });
    }

    /// <summary>
    /// 清空指定文件夹下的文件及文件夹
    /// </summary>
    public static void ClearDirectory(string dirPath)
    {
        try
        {
            if (!Directory.Exists(dirPath))
                return;

            var dir = new DirectoryInfo(dirPath);
            var fileInfo = dir.GetFileSystemInfos();

            foreach (var file in fileInfo)
            {
                if (file is DirectoryInfo)
                {
                    var subdir = new DirectoryInfo(file.FullName);
                    subdir.Delete(true);
                }
                else
                {
                    File.Delete(file.FullName);
                }
            }

            LoggerHelper.Info($"清空文件夹成功 {dirPath}");
        }
        catch (Exception ex)
        {
            LoggerHelper.Error($"清空文件夹出现异常 {dirPath}", ex);
        }
    }

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
        var stream = GetEmbeddedResourceStream(resPath);
        if (stream is null)
            return string.Empty;

        var streamReader = new StreamReader(stream);
        var content = streamReader.ReadToEnd();

        streamReader?.Close();
        stream?.Close();

        return content;
    }

    /// <summary>
    /// 从资源文件中抽取资源文件（默认覆盖源文件）
    /// </summary>
    public static void ExtractResFile(string resPath, string outputPath, bool isOverride = true)
    {
        // 如果输出文件存在，并且不覆盖文件，则退出
        if (!isOverride && File.Exists(outputPath))
            return;

        var stream = GetEmbeddedResourceStream(resPath);
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

            LoggerHelper.Info($"提取资源文件成功 {outputPath}");
        }
        catch (Exception ex)
        {
            LoggerHelper.Error($"提取资源文件出现异常 {outputPath}", ex);
        }
        finally
        {
            outStream?.Close();
            inStream?.Close();

            stream?.Close();
        }
    }

    /// <summary>
    /// 获取文件MD5值
    /// </summary>
    public static async Task<string> GetFileMD5(string filePath)
    {
        if (!File.Exists(filePath))
            return string.Empty;

        using var fileStream = File.OpenRead(filePath);
        var fileMD5 = await _md5.ComputeHashAsync(fileStream);

        return Convert.ToHexString(fileMD5);
    }

    /// <summary>
    /// 获取文件大小的显示字符串
    /// </summary>
    public static string BytesToReadableValue(long number)
    {
        var last = 1.0;

        for (int i = 0; i < _suffixes.Length; i++)
        {
            var current = Math.Pow(1024, i + 1);

            var temp = number / current;
            if (temp < 1)
                return $"{(number / last):n2} {_suffixes[i]}";

            last = current;
        }

        return number.ToString();
    }

    /// <summary>
    /// 使用 UTF8-NoBom 格式读取文本文件
    /// </summary>
    public static string ReadAllTextUtf8NoBom(string readPath)
    {
        return File.ReadAllText(readPath, Encoding_UTF8_NoBOM);
    }

    /// <summary>
    /// 使用 UTF8-NoBom 格式写入文本文件
    /// </summary>
    public static void WriteAllTextUtf8NoBom(string savePath, string contents)
    {
        File.WriteAllText(savePath, contents, Encoding_UTF8_NoBOM);
    }
}