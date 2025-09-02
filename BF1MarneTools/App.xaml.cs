using BF1MarneTools.Helper;
using BF1MarneTools.Resources;
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
        // 设置UI语言
        try
        {
            var localeId = Globals.LocaleID;
            if (string.IsNullOrWhiteSpace(localeId))
                localeId = Globals.Chinese;

            var cultureInfo = new CultureInfo(localeId);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }
        catch
        {
            var cultureInfo = new CultureInfo(Globals.Chinese);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        //////////////////////////////////////////////////////

        LoggerHelper.Info($"{Lang.App_Welcome} {Globals.AppName}");

        // 注册异常捕获
        RegisterEvents();
        // 注册编码
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        //////////////////////////////////////////////////////

        AppMainMutex = new Mutex(true, Globals.AppName, out var createdNew);
        if (!createdNew)
        {
            LoggerHelper.Warn(Lang.App_NotRunAgain);
            MsgBoxHelper.Warning(Lang.App_NotRunAgain, Lang.MsgBoxHelper_Warning);
            Environment.Exit(0);
            return;
        }

        //////////////////////////////////////////////////////

        LoggerHelper.Info(Lang.App_CheckingPort);
        var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
        var ipEndPoints = ipProperties.GetActiveTcpListeners();
        foreach (var endPoint in ipEndPoints)
        {
            if (endPoint.Port == 3216)
            {
                LoggerHelper.Error(Lang.App_Port3216Warning);
                MsgBoxHelper.Warning(Lang.App_Port3216Warning, Lang.MsgBoxHelper_Warning);
                Environment.Exit(0);
                return;
            }

            if (endPoint.Port == 59743)
            {
                LoggerHelper.Error(Lang.App_Port59743Warning);
                MsgBoxHelper.Warning(Lang.App_Port59743Warning, Lang.MsgBoxHelper_Warning);
                Environment.Exit(0);
                return;
            }
        }
        LoggerHelper.Info(Lang.App_CheckPortPassed);

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

        LoggerHelper.Info($"{Lang.App_Close} {Globals.AppName}");
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