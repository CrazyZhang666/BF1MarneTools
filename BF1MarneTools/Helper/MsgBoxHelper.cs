using BF1MarneTools.Resources;

namespace BF1MarneTools.Helper;

public static class MsgBoxHelper
{
    /// <summary>
    /// 通用信息弹窗，Information
    /// </summary>
    public static void Information(string content, string title = "提示")
    {
        MessageBox.Show(content, title, MessageBoxButton.OK, MessageBoxImage.Information);
    }

    /// <summary>
    /// 通用警告弹窗，Warning
    /// </summary>
    public static void Warning(string content, string title = "警告")
    {
        MessageBox.Show(content, title, MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    /// <summary>
    /// 通用错误弹窗，Error
    /// </summary>
    public static void Error(string content, string title = "错误")
    {
        MessageBox.Show(content, title, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    /// <summary>
    /// 通用异常弹窗，Exception
    /// </summary>
    public static void Exception(Exception ex, string title = "异常")
    {
        MessageBox.Show($"{Lang.MsgBoxHelper_ExceptionMsg}{ex.Message}", title,
            MessageBoxButton.OK, MessageBoxImage.Error);
    }
}