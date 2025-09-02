using BF1MarneTools.Helper;
using BF1MarneTools.Models;
using BF1MarneTools.Resources;
using BF1MarneTools.Utils;
using CommunityToolkit.Mvvm.Input;

namespace BF1MarneTools;

/// <summary>
/// LoadWindow.xaml 的交互逻辑
/// </summary>
public partial class LoadWindow
{
    /// <summary>
    /// 数据模型绑定
    /// </summary>
    public LoadModel LoadModel { get; set; } = new();

    /// <summary>
    /// 构造方法
    /// </summary>
    public LoadWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 窗口加载完成事件
    /// </summary>
    private void Window_Load_Loaded(object sender, RoutedEventArgs e)
    {
        Title = $"{Lang.WindowTitle} v{CoreUtil.VersionInfo}";
    }

    /// <summary>
    /// 窗口渲染完成事件
    /// </summary>
    private async void Window_Load_ContentRendered(object sender, EventArgs e)
    {
        if (Globals.LocaleID == Globals.English)
            RadioButton_Lang_enUS.IsChecked = true;
        else
            RadioButton_Lang_zhCN.IsChecked = true;

        /////////////////////////////////////////////////

        AppendLogger(Lang.LoadWin_BeginInitializeResources);

        // 战地1路径
        LoadModel.Bf1Path = Globals.BF1AppPath;
        // 默认初始化失败
        LoadModel.IsInitSuccess = false;

        /////////////////////////////////////////////////

        await Task.Run(async () =>
        {
            try
            {
                // 关闭其它服务进程
                AppendLogger(Lang.LoadWin_ClosingOtherServiceProcess);
                CoreUtil.CloseServiceProcess();
                AppendLogger(Lang.LoadWin_OtherServiceProcessClosedSuccess);

                //////////////////////////////////////////

                if (await CoreUtil.IsMD5CheckFrosty())
                {
                    AppendLogger(Lang.LoadWin_ResourceFileFrostyCheckPassed);
                }
                else
                {
                    FileHelper.ClearDirectory(CoreUtil.Dir_Frosty);

                    AppendLogger(Lang.LoadWin_ExtractingResourceFileFrostyZip);
                    FileHelper.ExtractResFile("Data.Frosty.zip", CoreUtil.File_Zip_Frosty);
                    AppendLogger(Lang.LoadWin_UnzippingTheFileFrostyZip);
                    ZipFile.ExtractToDirectory(CoreUtil.File_Zip_Frosty, CoreUtil.Dir_Frosty, true);
                    File.Delete(CoreUtil.File_Zip_Frosty);
                }

                /////////////

                if (await CoreUtil.IsMD5CheckMarne())
                {
                    AppendLogger(Lang.LoadWin_ResourceFileMarneCheckPassed);
                }
                else
                {
                    FileHelper.ClearDirectory(CoreUtil.Dir_Marne);

                    AppendLogger(Lang.LoadWin_ExtractingResourceFileMarneZip);
                    FileHelper.ExtractResFile("Data.Marne.zip", CoreUtil.File_Zip_Marne);
                    AppendLogger(Lang.LoadWin_UnzippingTheFileMarneZip);
                    ZipFile.ExtractToDirectory(CoreUtil.File_Zip_Marne, CoreUtil.Dir_Marne, true);
                    File.Delete(CoreUtil.File_Zip_Marne);
                }

                /////////////

                if (await CoreUtil.IsMD5CheckService())
                {
                    AppendLogger(Lang.LoadWin_ResourceFileServiceCheckPassed);
                }
                else
                {
                    FileHelper.ClearDirectory(CoreUtil.Dir_Service);

                    AppendLogger(Lang.LoadWin_ExtractingResourceFileServiceZip);
                    FileHelper.ExtractResFile("Data.Service.zip", CoreUtil.File_Zip_Service);
                    AppendLogger(Lang.LoadWin_UnzippingTheFileServiceZip);
                    ZipFile.ExtractToDirectory(CoreUtil.File_Zip_Service, CoreUtil.Dir_Service, true);
                    File.Delete(CoreUtil.File_Zip_Service);
                }

                //////////////////////////////////////////

                LoadModel.IsInitSuccess = true;
                AppendLogger(Lang.LoadWin_AppResourcesDataInitializeSuccess);
            }
            catch (Exception ex)
            {
                AppendLogger($"{Lang.LoadWin_ResourceInitializeException}  {ex.Message}");
            }
        });
    }

    /// <summary>
    /// 窗口关闭时事件
    /// </summary>
    private void Window_Load_Closing(object sender, CancelEventArgs e)
    {
        Globals.Save();
    }

    /// <summary>
    /// 追加日志
    /// </summary>
    private void AppendLogger(string log)
    {
        this.Dispatcher.Invoke(() =>
        {
            TextBox_Logger.AppendText($"[{DateTime.Now:HH:mm:ss}]  {log}{Environment.NewLine}");
            TextBox_Logger.ScrollToEnd();
        });
    }

    /// <summary>
    /// 选择战地1文件路径
    /// </summary>
    [RelayCommand]
    private void SelcetBf1Path()
    {
        try
        {
            // 战地1路径无效，重新选择
            var dialog = new OpenFileDialog
            {
                Title = Lang.LoadWin_FileDialogTitle,
                FileName = "bf1.exe",
                DefaultExt = ".exe",
                Filter = Lang.LoadWin_FileDialogFilter,
                Multiselect = false,
                RestoreDirectory = true,
                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true
            };

            // 当文件夹路径存在时才会赋值
            if (Directory.Exists(Globals.GameSelectDir))
                dialog.InitialDirectory = Globals.GameSelectDir;

            // 如果未选择，则退出程序
            if (dialog.ShowDialog() == false)
                return;

            var dirPath = Path.GetDirectoryName(dialog.FileName);

            // 记住本次选择的文件路径
            Globals.GameSelectDir = dirPath;

            // 开始校验文件有效性
            if (!CoreUtil.IsBf1MainAppFile(dialog.FileName))
            {
                AppendLogger(Lang.LoadWin_NoSupportBF1Version);
                return;
            }
            AppendLogger(Lang.LoadWin_BF1VersionCheckPassed);

            // 检查战地1所在目录磁盘格式
            var diskFlag = Path.GetPathRoot(dirPath);
            var driveInfo = new DriveInfo(diskFlag);
            if (!driveInfo.DriveFormat.Equals("NTFS", StringComparison.OrdinalIgnoreCase))
            {
                AppendLogger(Lang.LoadWin_BF1InstallDiskNotNTFS);
                return;
            }
            AppendLogger(Lang.LoadWin_BF1InstallDiskCheckPassed);

            Globals.SetBF1AppPath(dialog.FileName);
            LoadModel.Bf1Path = dialog.FileName;

            AppendLogger(Lang.LoadWin_GetBF1ExePathSuccess);
            return;
        }
        catch (Exception ex)
        {
            LoggerHelper.Error(Lang.LoadWin_SelectBF1PathException, ex);
            AppendLogger($"{Lang.LoadWin_SelectBF1PathException}  {ex.Message}");
            return;
        }
    }

    /// <summary>
    /// 启动主程序
    /// </summary>
    [RelayCommand]
    private void LaunchMainApp()
    {
        var mainWindow = new MainWindow();

        // 转移主程序控制权
        Application.Current.MainWindow = mainWindow;
        // 关闭当前窗口
        this.Close();

        // 显示主程序窗口
        mainWindow.Show();
    }

    /// <summary>
    /// 打开配置文件夹
    /// </summary>
    [RelayCommand]
    private void OpenConfigFolder()
    {
        ProcessHelper.OpenDirectory(CoreUtil.Dir_Default);
    }

    /// <summary>
    /// 切换语言
    /// </summary>
    [RelayCommand]
    private void SwitchLanguage(string langStr)
    {
        Globals.LocaleID = langStr;
        RestartApp();
    }

    /// <summary>
    /// 重启自身程序
    /// </summary>
    private void RestartApp()
    {
        App.AppMainMutex.ReleaseMutex();

        Process.Start(Environment.ProcessPath);
        Application.Current.Shutdown();
    }
}