using BF1MarneTools.Helper;
using BF1MarneTools.Resources;

namespace BF1MarneTools.Utils;

public static class MiscUtil
{
    /// <summary>
    /// 生成自定义异常消息
    /// </summary>
    public static string GetExceptionInfo(Exception ex, string backStr)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"{Lang.MiscUtil_VersionInfo}: {CoreUtil.VersionInfo}");
        builder.AppendLine($"{Lang.MiscUtil_UserName}: {CoreUtil.UserName}");
        builder.AppendLine($"{Lang.MiscUtil_MachineName}: {CoreUtil.MachineName}");
        builder.AppendLine($"{Lang.MiscUtil_OSVersion}: {CoreUtil.OSVersion}");
        builder.AppendLine($"{Lang.MiscUtil_SystemDirectory}: {CoreUtil.SystemDirectory}");
        builder.AppendLine($"{Lang.MiscUtil_RuntimeVersion}: {CoreUtil.RuntimeVersion}");
        builder.AppendLine($"{Lang.MiscUtil_OSArchitecture}: {CoreUtil.OSArchitecture}");
        builder.AppendLine($"{Lang.MiscUtil_RuntimeIdentifier}: {CoreUtil.RuntimeIdentifier}");
        builder.AppendLine("------------------------------");
        builder.AppendLine($"{Lang.MiscUtil_CrashTime}: {DateTime.Now}");

        if (ex is not null)
        {
            builder.AppendLine($"{Lang.MiscUtil_ExceptionType}: {ex.GetType().Name}");
            builder.AppendLine($"{Lang.MiscUtil_ExceptionMsg}: {ex.Message}");
            builder.AppendLine($"{Lang.MiscUtil_StackTrace}: \n{ex.StackTrace}");
        }
        else
        {
            builder.AppendLine($"{Lang.MiscUtil_UnhandledException}: {backStr}");
        }

        return builder.ToString();
    }

    /// <summary>
    /// 保存崩溃日志
    /// </summary>
    public static void SaveCrashLog(string log)
    {
        try
        {
            var path = Path.Combine(CoreUtil.Dir_Log_Crash, $"CrashReport-{DateTime.Now:yyyyMMdd_HHmmss_ffff}.log");
            FileHelper.WriteAllTextUtf8NoBom(path, log);
        }
        catch { }
    }
}