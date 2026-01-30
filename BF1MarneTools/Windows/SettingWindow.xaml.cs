using BF1MarneTools.Helper;
using CommunityToolkit.Mvvm.Input;

namespace BF1MarneTools.Windows;

/// <summary>
/// SettingWindow.xaml 的交互逻辑
/// </summary>
public partial class SettingWindow
{
    /// <summary>
    /// 服务器设置文件路径
    /// </summary>
    private string SettingsFilePath { get; set; }

    /// <summary>
    /// 构造方法
    /// </summary>
    public SettingWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 窗口加载完成事件
    /// </summary>
    private void Window_Setting_Loaded(object sender, RoutedEventArgs e)
    {
    }

    /// <summary>
    /// 窗口渲染完成事件
    /// </summary>
    private void Window_Setting_ContentRendered(object sender, EventArgs e)
    {
        try
        {
            SettingsFilePath = Path.Combine(Globals.BF1InstallDir, "Instance\\ServerSettings.txt");

            if (!Globals.IsUseServer)
            {
                var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var marneDir = Path.Combine(appDataDir, "Marne");

                SettingsFilePath = Path.Combine(marneDir, "ServerSettings.txt");
            }

            // 智能自动创建文件夹
            FileHelper.CreateDirectory(SettingsFilePath);

            // 如果文件不存，则创建一个新文件
            if (!File.Exists(SettingsFilePath))
            {
                // 直接从资源文件提取到本地
                FileHelper.ExtractResFile("Docs.ServerSettings.txt", SettingsFilePath);
            }

            // 读取文本文件
            ReadSettingFile();
        }
        catch (Exception ex)
        {
            LoggerHelper.Error($"初始化服务器设置出现异常: {ex.Message}");
        }
    }

    /// <summary>
    /// 窗口关闭时事件
    /// </summary>
    private void Window_Setting_Closing(object sender, CancelEventArgs e)
    {
    }

    /// <summary>
    /// 使用默认设置
    /// </summary>
    [RelayCommand]
    private void UseDefaultSetting()
    {
        TextBox_ServerSettings.Text = FileHelper.GetEmbeddedResourceText("Docs.ServerSettings.txt");
    }

    /// <summary>
    /// 读取设置文件
    /// </summary>
    [RelayCommand]
    private void ReadSettingFile()
    {
        try
        {
            TextBox_ServerSettings.Text = FileHelper.ReadAllTextUtf8NoBom(SettingsFilePath);

            LoggerHelper.Info("读取服务器设置成功");
        }
        catch (Exception ex)
        {
            LoggerHelper.Error($"读取服务器设置出现异常: {ex.Message}");
        }
    }

    /// <summary>
    /// 保存设置文件
    /// </summary>
    [RelayCommand]
    private void SaveSettingFile()
    {
        try
        {
            var content = TextBox_ServerSettings.Text.Trim();
            FileHelper.WriteAllTextUtf8NoBom(SettingsFilePath, content);

            LoggerHelper.Info("保存服务器设置成功");
            LoggerHelper.Info($"服务器设置文件路径为 {SettingsFilePath}");
        }
        catch (Exception ex)
        {
            LoggerHelper.Error($"保存服务器设置出现异常: {ex.Message}");
        }

        this.Close();
    }
}