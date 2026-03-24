using BF1MarneTools.Helper;
using BF1MarneTools.Utils;

namespace BF1MarneTools;

/// <summary>
/// App.xaml 的交互逻辑
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// 主程序互斥体
    /// </summary>
    public static Mutex AppMainMutex;

    /// <summary>
    /// App启动事件
    /// </summary>
    protected override void OnStartup(StartupEventArgs e)
    {
        LoggerHelper.Info($"欢迎使用 {Globals.AppName}");

        // 注册异常捕获
        RegisterEvents();
        // 注册编码
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        //////////////////////////////////////////////////////

        AppMainMutex = new Mutex(true, Globals.AppName, out var createdNew);
        if (!createdNew)
        {
            LoggerHelper.Warn("请不要重复打开，程序已经运行");
            MsgBoxHelper.Warning("请不要重复打开，程序已经运行", "警告");
            Environment.Exit(0);
            return;
        }

        //////////////////////////////////////////////////////

        LoggerHelper.Info("正在进行 TCP端口 可用性检测...");
        var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
        var ipEndPoints = ipProperties.GetActiveTcpListeners();
        foreach (var endPoint in ipEndPoints)
        {
            if (endPoint.Port == 3216)
            {
                LoggerHelper.Error("检测到 TCP端口 3216 被占用，请解除端口占用");
                MsgBoxHelper.Warning("检测到 TCP端口 3216 被占用，请解除端口占用", "警告");
                Environment.Exit(0);
                return;
            }

            if (endPoint.Port == 59743)
            {
                LoggerHelper.Error("检测到 TCP端口 59743 被占用，请解除端口占用");
                MsgBoxHelper.Warning("检测到 TCP端口 59743 被占用，请解除端口占用", "警告");
                Environment.Exit(0);
                return;
            }
        }
        LoggerHelper.Info("当前系统 TCP端口 检测正常");

        //////////////////////////////////////////////////////

        // 读取全局配置文件
        Globals.Read();

        base.OnStartup(e);
    }

    /// <summary>
    /// App关闭事件
    /// </summary>
    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);

        // 保存全局配置文件
        Globals.Save();

        LoggerHelper.Info($"程序已关闭 {Globals.AppName}");
    }

    /// <summary>
    /// 注册异常捕获事件
    /// </summary>
    private void RegisterEvents()
    {
        DispatcherUnhandledException += App_DispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
    }

    /// <summary>
    /// UI线程未捕获异常处理事件（UI主线程）
    /// </summary>
    private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        var msg = MiscUtil.GetExceptionInfo(e.Exception, e.ToString());
        MiscUtil.SaveCrashLog(msg);
    }

    /// <summary>
    /// 非UI线程未捕获异常处理事件（例如自己创建的一个子线程）
    /// </summary>
    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var msg = MiscUtil.GetExceptionInfo(e.ExceptionObject as Exception, e.ToString());
        MiscUtil.SaveCrashLog(msg);
    }

    /// <summary>
    /// Task线程内未捕获异常处理事件
    /// </summary>
    private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        var msg = MiscUtil.GetExceptionInfo(e.Exception, e.ToString());
        MiscUtil.SaveCrashLog(msg);
    }
}