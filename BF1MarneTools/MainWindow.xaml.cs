using BF1MarneTools.API;
using BF1MarneTools.Core;
using BF1MarneTools.Helper;
using BF1MarneTools.Models;
using BF1MarneTools.Utils;
using BF1MarneTools.Windows;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Hardcodet.Wpf.TaskbarNotification;

namespace BF1MarneTools;

/// <summary>
/// MainWindow.xaml 的交互逻辑
/// </summary>
public partial class MainWindow
{
    /// <summary>
    /// 数据模型绑定
    /// </summary>
    public MainModel MainModel { get; set; } = new();

    /// <summary>
    /// 窗口关闭识别标志
    /// </summary>
    private bool _isCodeClose = false;
    /// <summary>
    /// 第一次通知标志
    /// </summary>
    private bool _isFirstNotice = false;

    /// <summary>
    /// 构造方法
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 窗口加载完成事件
    /// </summary>
    private void Window_Main_Loaded(object sender, RoutedEventArgs e)
    {
        LoggerHelper.Info("启动主程序成功");

        Title = $"战地1马恩工具箱 v{CoreUtil.VersionInfo} - {CoreUtil.GetWorkMode()}";

        //////////////////////////////////////////

        // 释放 dinput8.dll 文件
        // 因为 dinput8.dll 更新较为频繁，所以每次打开都重新释放
        if (Directory.Exists(Globals.BF1InstallDir))
            FileHelper.ExtractResFile("Data.dinput8.dll", Path.Combine(Globals.BF1InstallDir, "dinput8.dll"));

        // 初始化工作
        Ready.Run();

        // 注册 RunBf1Game 启动游戏事件
        WeakReferenceMessenger.Default.Register<string, string>(this, "RunBf1Game", async (s, e) =>
        {
            await this.Dispatcher.BeginInvoke(async () =>
            {
                await Launch.RunBf1Game();
            });
        });
    }

    /// <summary>
    /// 窗口渲染完成事件
    /// </summary>
    private async void Window_Main_ContentRendered(object sender, EventArgs e)
    {
        // 检查更新
        await CheckUpdate();
    }

    /// <summary>
    /// 窗口关闭时事件
    /// </summary>
    private void Window_Main_Closing(object sender, CancelEventArgs e)
    {
        // 当用户从UI点击关闭时才执行
        if (!_isCodeClose)
        {
            // 取消关闭事件，隐藏主窗口
            e.Cancel = true;
            this.Hide();

            // 仅第一次通知
            if (!_isFirstNotice)
            {
                NotifyIcon_Main.ShowBalloonTip("战地1马恩工具箱", "可通过托盘右键菜单完全退出程序", BalloonIcon.Info);
                _isFirstNotice = true;
            }

            return;
        }

        // 清理工作
        Ready.Stop();

        // 释放托盘图标
        NotifyIcon_Main.CloseBalloon();
        NotifyIcon_Main?.Dispose();
        NotifyIcon_Main = null;

        LoggerHelper.Info("关闭主程序成功");
    }

    /// <summary>
    /// 检查更新
    /// </summary>
    private async Task CheckUpdate()
    {
        LoggerHelper.Info("正在检测新版本中...");
        NotifierHelper.Notice("正在检测新版本中...");

        // 最多执行4次
        for (int i = 0; i <= 4; i++)
        {
            // 当第4次还是失败，显示提示
            if (i > 3)
            {
                MainModel.IsNeedUpdate = true;
                LoggerHelper.Error("检测新版本失败，请检查网络连接");
                NotifierHelper.Error("检测新版本失败，请检查网络连接");
                return;
            }

            // 第1次不提示重试
            if (i > 0)
            {
                // 最多重试3次
                LoggerHelper.Warn($"检测新版本失败，重试中... {i}/3");
            }

            var webVersion = await CoreApi.GetWebUpdateVersion();
            if (webVersion != null)
            {
                if (CoreUtil.VersionInfo >= webVersion)
                {
                    MainModel.IsNeedUpdate = false;
                    LoggerHelper.Info($"你当前使用的已是最新版本 v{CoreUtil.VersionInfo}");
                    NotifierHelper.Info($"你当前使用的已是最新版本 v{CoreUtil.VersionInfo}");
                    return;
                }

                MainModel.IsNeedUpdate = true;
                LoggerHelper.Info($"发现新版本，请下载更新 v{webVersion}");
                NotifierHelper.Warning($"发现新版本，请下载更新 v{webVersion}");
                return;
            }
        }
    }

    /// <summary>
    /// 设置启动账号
    /// </summary>
    [RelayCommand]
    private void SetLaunchAccount()
    {
        // 检查 战地1 是否正在运行
        if (GameUtil.IsCheckBF1Running())
            return;

        var accountWindow = new AccountWindow
        {
            Owner = this,
        };
        accountWindow.ShowModalDialog();
    }

    /// <summary>
    /// 设置游戏语言
    /// </summary>
    [RelayCommand]
    private void SetGameLocale()
    {
        // 检查 战地1 是否正在运行
        if (GameUtil.IsCheckBF1Running())
            return;

        var localeWindow = new LocaleWindow
        {
            Owner = this,
        };
        localeWindow.ShowModalDialog();
    }

    /// <summary>
    /// 打开Marne目录
    /// </summary>
    [RelayCommand]
    private void OpenMarneFolder()
    {
        var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var marneDir = Path.Combine(appDataDir, "Marne");

        ProcessHelper.OpenDirectory(marneDir);
    }

    /// <summary>
    /// 打开Crash目录
    /// </summary>
    [RelayCommand]
    private void OpenCrashFolder()
    {
        var localAppDataDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var crashDir = Path.Combine(localAppDataDir, "CrashDumps");

        ProcessHelper.OpenDirectory(crashDir);
    }

    /// <summary>
    /// 打开战地1目录
    /// </summary>
    [RelayCommand]
    private void OpenBF1Folder()
    {
        ProcessHelper.OpenDirectory(Globals.BF1InstallDir);
    }

    /// <summary>
    /// 打开配置目录
    /// </summary>
    [RelayCommand]
    private void OpenConfigFolder()
    {
        ProcessHelper.OpenDirectory(CoreUtil.Dir_Default);
    }

    /// <summary>
    /// 显示主窗口
    /// </summary>
    [RelayCommand]
    private void ShowWindow()
    {
        this.Show();

        if (this.WindowState == WindowState.Minimized)
            this.WindowState = WindowState.Normal;

        this.Activate();
        this.Focus();
    }

    /// <summary>
    /// 退出程序
    /// </summary>
    [RelayCommand]
    private void ExitApp()
    {
        // 设置关闭标志
        _isCodeClose = true;
        this.Close();
    }
}