using BF1MarneTools.Helper;

namespace BF1MarneTools.Utils;

public static class MiscUtil
{
    /// <summary>
    /// 生成自定义异常消息
    /// </summary>
    public static string GetExceptionInfo(Exception ex, string backStr)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"程序版本: {CoreUtil.VersionInfo}");
        builder.AppendLine($"用户名称: {CoreUtil.UserName}");
        builder.AppendLine($"电脑名称: {CoreUtil.MachineName}");
        builder.AppendLine($"系统版本: {CoreUtil.OSVersion}");
        builder.AppendLine($"系统目录: {CoreUtil.SystemDirectory}");
        builder.AppendLine($"运行库平台: {CoreUtil.RuntimeVersion}");
        builder.AppendLine($"运行库版本: {CoreUtil.OSArchitecture}");
        builder.AppendLine($"运行库环境: {CoreUtil.RuntimeIdentifier}");
        builder.AppendLine("------------------------------");
        builder.AppendLine($"崩溃时间: {DateTime.Now}");

        if (ex is not null)
        {
            builder.AppendLine($"异常类型: {ex.GetType().Name}");
            builder.AppendLine($"异常信息: {ex.Message}");
            builder.AppendLine($"堆栈追踪: \n{ex.StackTrace}");
        }
        else
        {
            builder.AppendLine($"未处理异常: {backStr}");
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