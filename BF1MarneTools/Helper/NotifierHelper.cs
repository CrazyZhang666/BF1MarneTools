using Notification.Wpf;
using Notification.Wpf.Constants;
using Notification.Wpf.Controls;

namespace BF1MarneTools.Helper;

public static class NotifierHelper
{
    /// <summary>
    /// 通知管理器
    /// </summary>
    private static readonly NotificationManager _notificationManager = new();

    /// <summary>
    /// 通知显示时间
    /// </summary>
    private static readonly TimeSpan _expirationTime = TimeSpan.FromSeconds(1.0);

    /// <summary>
    /// 静态构造方法
    /// </summary>
    static NotifierHelper()
    {
        NotificationConstants.DefaultRowCounts = 1;
        NotificationConstants.NotificationsOverlayWindowMaxCount = 3;
        NotificationConstants.MessagePosition = NotificationPosition.BottomCenter;

        NotificationConstants.MinWidth = 320.0;
        NotificationConstants.MaxWidth = NotificationConstants.MinWidth;

        NotificationConstants.FontName = "Microsoft YaHei";
        NotificationConstants.TitleSize = 12.0;
        NotificationConstants.MessageSize = 12.0;
        NotificationConstants.MessageTextAlignment = TextAlignment.Left;
        NotificationConstants.TitleTextAlignment = TextAlignment.Left;

        NotificationConstants.DefaultForegroundColor = GetBrushColor("#DFE1E5");
        NotificationConstants.DefaultBackgroundColor = GetBrushColor("#4C4A48");

        NotificationConstants.InformationBackgroundColor = GetBrushColor("#2080F0");
        NotificationConstants.SuccessBackgroundColor = GetBrushColor("#18A058");
        NotificationConstants.WarningBackgroundColor = GetBrushColor("#F0A020");
        NotificationConstants.ErrorBackgroundColor = GetBrushColor("#D03050");
    }

    /// <summary>
    /// 将字符串转化为颜色值
    /// </summary>
    private static Brush GetBrushColor(string color)
    {
        return new BrushConverter().ConvertFrom(color) as Brush;
    }

    /// <summary>
    /// 显示Toast通知
    /// </summary>
    private static void Show(NotificationType type, string message)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            var title = type switch
            {
                NotificationType.None => "",
                NotificationType.Information => "信息",
                NotificationType.Success => "成功",
                NotificationType.Warning => "警告",
                NotificationType.Error => "错误",
                NotificationType.Notification => "通知",
                _ => "",
            };

            var content = new NotificationContent
            {
                Title = title,
                Type = type,
                Message = message,
                TrimType = NotificationTextTrimType.Trim
            };

            _notificationManager.Show(content, "MainWindowArea", _expirationTime, null, null, true, false);
        });
    }

    public static void Info(string message)
    {
        Show(NotificationType.Information, message);
    }

    public static void Success(string message)
    {
        Show(NotificationType.Success, message);
    }

    public static void Warning(string message)
    {
        Show(NotificationType.Warning, message);
    }

    public static void Error(string message)
    {
        Show(NotificationType.Error, message);
    }

    public static void Notice(string message)
    {
        Show(NotificationType.Notification, message);
    }
}