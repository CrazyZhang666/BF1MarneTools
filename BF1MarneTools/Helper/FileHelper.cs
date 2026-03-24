using System.IO.Hashing;

namespace BF1MarneTools.Helper;

public static class FileHelper
{
    /// <summary>
    /// 文件大小后缀名
    /// </summary>
    private static readonly string[] _suffixes = ["B", "KB", "MB", "GB", "TB", "PB"];

    /// <summary>
    /// UTF8无BOM编码
    /// </summary>
    public static readonly UTF8Encoding Encoding_UTF8_NoBOM = new(false);

    /// <summary>
    /// 创建文件夹（如果存在则跳过）
    /// </summary>
    public static void CreateDirectory(string targetPath)
    {
        var dirPath = targetPath;
        // 判断是文件还是文件夹
        if (Path.HasExtension(targetPath))
            dirPath = Path.GetDirectoryName(targetPath);

        if (!Directory.Exists(dirPath))
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

            var newDir = Path.GetDirectoryName(newPath);
            CreateDirectory(newDir);

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
    /// 获取Crc32
    /// </summary>
    public static async Task<uint> GetCrc32(Stream stream)
    {
        var crc32 = new Crc32();
        await crc32.AppendAsync(stream);

        return crc32.GetCurrentHashAsUInt32();
    }

    /// <summary>
    /// 获取文件Crc32值
    /// </summary>
    public static async Task<uint> GetFileCrc32(string filePath)
    {
        if (!File.Exists(filePath))
            return 0;

        using var fileStream = File.OpenRead(filePath);
        return await GetCrc32(fileStream);
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