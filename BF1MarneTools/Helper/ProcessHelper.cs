﻿using BF1MarneTools.Resources;

namespace BF1MarneTools.Helper;

public static class ProcessHelper
{
    /// <summary>
    /// 判断进程是否运行
    /// </summary>
    public static bool IsAppRun(string appName)
    {
        if (string.IsNullOrWhiteSpace(appName))
            return false;

        if (appName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            appName = appName[..^4];

        return Process.GetProcessesByName(appName).Length > 0;
    }

    /// <summary>
    /// 打开http链接
    /// </summary>
    public static void OpenLink(string url)
    {
        if (!url.StartsWith("http"))
        {
            LoggerHelper.Warn($"{Lang.ProcessHelper_HttpFormatError} {url}");
            return;
        }

        try
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            LoggerHelper.Error($"{Lang.ProcessHelper_OpenHttpException} {url}", ex);
        }
    }

    /// <summary>
    /// 打开文件夹路径
    /// </summary>
    public static void OpenDirectory(string dirPath, bool isNotify = false)
    {
        if (!Directory.Exists(dirPath))
        {
            if (isNotify)
                NotifierHelper.Warning(Lang.ProcessHelper_OpenDirectoryNotExists);

            LoggerHelper.Warn($"{Lang.ProcessHelper_OpenDirectoryNotExists} {dirPath}");
            return;
        }

        try
        {
            Process.Start(new ProcessStartInfo(dirPath) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            if (isNotify)
                NotifierHelper.Error(Lang.ProcessHelper_OpenDirectoryException);

            LoggerHelper.Error($"{Lang.ProcessHelper_OpenDirectoryException} {dirPath}", ex);
        }
    }

    /// <summary>
    /// 打开指定进程
    /// </summary>
    public static void OpenProcess(string appPath, string args = "", bool isSilent = false)
    {
        if (!File.Exists(appPath))
        {
            LoggerHelper.Warn($"{Lang.ProcessHelper_OpenProcessNotExists} {appPath}");
            return;
        }

        var fileInfo = new FileInfo(appPath);

        try
        {
            // 如果应在启动进程时使用 shell，则为 true；如果直接从可执行文件创建进程，则为 false。
            // 默认值为 true .NET Framework 应用和 false .NET Core 应用。
            var processInfo = new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = fileInfo.FullName,
                WorkingDirectory = fileInfo.DirectoryName,
                Arguments = args
            };

            if (isSilent)
            {
                processInfo.WindowStyle = ProcessWindowStyle.Hidden;
                processInfo.CreateNoWindow = true;
            }

            Process.Start(processInfo);
            LoggerHelper.Info($"{Lang.ProcessHelper_OpenProcessSuccess} {appPath}");
        }
        catch (Exception ex)
        {
            LoggerHelper.Error($"{Lang.ProcessHelper_OpenProcessException} {appPath}", ex);
        }
    }

    /// <summary>
    /// 根据名字关闭指定进程
    /// </summary>
    public static void CloseProcess(string appName)
    {
        if (string.IsNullOrWhiteSpace(appName))
            return;

        if (appName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            appName = appName[..^4];

        try
        {
            var isFind = false;

            foreach (var process in Process.GetProcessesByName(appName))
            {
                // 关闭进程树
                process.Kill(true);
                LoggerHelper.Info($"{Lang.ProcessHelper_CloseProcessSuccess} {appName}.exe");

                isFind = true;
            }

            if (!isFind)
                LoggerHelper.Warn($"{Lang.ProcessHelper_CloseProcessNotFound} {appName}.exe");
        }
        catch (Exception ex)
        {
            LoggerHelper.Error($"{Lang.ProcessHelper_CloseProcessException} {appName}", ex);
        }
    }
}