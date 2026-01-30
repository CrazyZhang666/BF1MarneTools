using BF1MarneTools.Helper;
using BF1MarneTools.Models;
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
        Title = $"战地1马恩工具箱 v{CoreUtil.VersionInfo}";
    }

    /// <summary>
    /// 窗口渲染完成事件
    /// </summary>
    private async void Window_Load_ContentRendered(object sender, EventArgs e)
    {
        AppendLogger("开始初始化资源数据...");

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
                AppendLogger("正在关闭其它服务进程...");
                CoreUtil.CloseServiceProcess();
                AppendLogger("其它服务进程，关闭成功");

                //////////////////////////////////////////

                if (await CoreUtil.IsMD5CheckFrosty())
                {
                    AppendLogger("资源文件 Frosty 检查通过");
                }
                else
                {
                    FileHelper.ClearDirectory(CoreUtil.Dir_Frosty);

                    AppendLogger("正在提取资源文件 Frosty.zip ...");
                    FileHelper.ExtractResFile("Data.Frosty.zip", CoreUtil.File_Zip_Frosty);
                    AppendLogger("正在解压文件 Frosty.zip ...");
                    ZipFile.ExtractToDirectory(CoreUtil.File_Zip_Frosty, CoreUtil.Dir_Frosty, true);
                    File.Delete(CoreUtil.File_Zip_Frosty);
                }

                /////////////

                if (await CoreUtil.IsMD5CheckMarne())
                {
                    AppendLogger("资源文件 Marne 检查通过");
                }
                else
                {
                    FileHelper.ClearDirectory(CoreUtil.Dir_Marne);

                    AppendLogger("正在提取资源文件 Marne.zip ...");
                    FileHelper.ExtractResFile("Data.Marne.zip", CoreUtil.File_Zip_Marne);
                    AppendLogger("正在解压文件 Marne.zip ...");
                    ZipFile.ExtractToDirectory(CoreUtil.File_Zip_Marne, CoreUtil.Dir_Marne, true);
                    File.Delete(CoreUtil.File_Zip_Marne);
                }

                /////////////

                if (await CoreUtil.IsMD5CheckService())
                {
                    AppendLogger("资源文件 Service 检查通过");
                }
                else
                {
                    FileHelper.ClearDirectory(CoreUtil.Dir_Service);

                    AppendLogger("正在提取资源文件 Service.zip ...");
                    FileHelper.ExtractResFile("Data.Service.zip", CoreUtil.File_Zip_Service);
                    AppendLogger("正在解压文件 Service.zip ...");
                    ZipFile.ExtractToDirectory(CoreUtil.File_Zip_Service, CoreUtil.Dir_Service, true);
                    File.Delete(CoreUtil.File_Zip_Service);
                }

                //////////////////////////////////////////

                LoadModel.IsInitSuccess = true;
                AppendLogger("程序资源数据，初始化成功");
            }
            catch (Exception ex)
            {
                AppendLogger($"资源初始化时出现异常 {ex.Message}");
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
                Title = "请选择战地1游戏安装目录 \"bf1.exe\" 文件路径",
                FileName = "bf1.exe",
                DefaultExt = ".exe",
                Filter = "可执行文件 (.exe)|*.exe",
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
                AppendLogger("不支持的战地1游戏版本，请重新选择");
                return;
            }
            AppendLogger("战地1游戏版本，检查通过");

            // 检查战地1所在目录磁盘格式
            var diskFlag = Path.GetPathRoot(dirPath);
            var driveInfo = new DriveInfo(diskFlag);
            if (!driveInfo.DriveFormat.Equals("NTFS", StringComparison.OrdinalIgnoreCase))
            {
                AppendLogger("战地1安装磁盘格式不是NTFS，请转换磁盘格式");
                return;
            }
            AppendLogger("战地1安装磁盘格式，检查通过");

            Globals.SetBF1AppPath(dialog.FileName);
            LoadModel.Bf1Path = dialog.FileName;

            AppendLogger("BF1游戏主程序路径，获取成功");
            return;
        }
        catch (Exception ex)
        {
            LoggerHelper.Error("选择战地1安装路径时出现异常", ex);
            AppendLogger($"选择战地1安装路径时出现异常 {ex.Message}");
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
}