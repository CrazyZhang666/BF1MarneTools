using BF1MarneTools.Helper;
using BF1MarneTools.Resources;
using BF1MarneTools.Utils;
using BF1MarneTools.Windows;
using CommunityToolkit.Mvvm.Input;

namespace BF1MarneTools.Views;

/// <summary>
/// ToolsView.xaml 的交互逻辑
/// </summary>
public partial class ToolsView : UserControl
{
    /// <summary>
    /// 构造方法
    /// </summary>
    public ToolsView()
    {
        InitializeComponent();
        // 初始化待做任务
        ToDoList();
    }

    /// <summary>
    /// 初始化待做任务
    /// </summary>
    private void ToDoList()
    {
    }

    /// <summary>
    /// 清理Mod数据
    /// </summary>
    [RelayCommand]
    private void ClearModData()
    {
        if (!DialogWindow.Show(Lang.ToolsView_ClearModDataWarning, Lang.ToolsView_Warning))
            return;

        // 检查 战地1 是否正在运行
        if (GameUtil.IsCheckBF1Running())
            return;

        // 检查 FrostyModManager 是否正在运行2
        if (GameUtil.IsCheckFrostyModManagerRunning2())
            return;

        var modDataDir = Path.Combine(Globals.BF1InstallDir, "ModData");
        if (!Directory.Exists(modDataDir))
        {
            NotifierHelper.Warning(Lang.ToolsView_ClearModDataNotFound);
            return;
        }

        try
        {
            FileHelper.ClearDirectory(modDataDir);
            NotifierHelper.Success(Lang.ToolsView_ClearModDataSuccess);
        }
        catch (Exception ex)
        {
            LoggerHelper.Error(Lang.ToolsView_ClearModDataException, ex);
        }
    }

    /// <summary>
    /// 结束战地1进程
    /// </summary>
    [RelayCommand]
    private void KillBf1Process()
    {
        if (!DialogWindow.Show(Lang.ToolsView_KillBf1ProcessWarning, Lang.ToolsView_Warning))
            return;

        // 检查 战地1 是否未运行
        if (GameUtil.IsCheckBF1NotRun())
            return;

        ProcessHelper.CloseProcess(CoreUtil.Name_BF1);
        ProcessHelper.CloseProcess(CoreUtil.Name_FrostyModManager);
        ProcessHelper.CloseProcess(CoreUtil.Name_MarneLauncher);

        NotifierHelper.Success(Lang.ToolsView_KillBF1ProcessSuccess);
    }

    /// <summary>
    /// 编辑服务器设置
    /// </summary>
    [RelayCommand]
    private void EditServerSettings()
    {
        var settingWindow = new SettingWindow()
        {
            Owner = Application.Current.MainWindow
        };
        settingWindow.ShowModalDialog();
    }

    /// <summary>
    /// 编辑地图列表
    /// </summary>
    [RelayCommand]
    private void EditMapList()
    {
        var mapWindow = new MapWindow()
        {
            Owner = Application.Current.MainWindow
        };
        mapWindow.ShowModalDialog();
    }

    /// <summary>
    /// IP重定向
    /// </summary>
    [RelayCommand]
    private void OpenIPOverrideWeb()
    {
        ProcessHelper.OpenLink("https://marne.io/api/override.php");
    }
}