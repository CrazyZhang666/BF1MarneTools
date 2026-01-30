using BF1MarneTools.Helper;
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
        if (!DialogWindow.Show("你确定要清理《战地1》Mod数据吗？", "警告"))
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
            NotifierHelper.Warning("未发现战地1Mod数据文件夹，操作取消");
            return;
        }

        try
        {
            FileHelper.ClearDirectory(modDataDir);
            NotifierHelper.Success("清理Mod数据操作成功");
        }
        catch (Exception ex)
        {
            LoggerHelper.Error("清理Mod数据出现异常", ex);
        }
    }

    /// <summary>
    /// 结束战地1进程
    /// </summary>
    [RelayCommand]
    private void KillBf1Process()
    {
        if (!DialogWindow.Show("你确定要结束《战地1》进程吗？", "警告"))
            return;

        // 检查 战地1 是否未运行
        if (GameUtil.IsCheckBF1NotRun())
            return;

        ProcessHelper.CloseProcess(CoreUtil.Name_BF1);
        ProcessHelper.CloseProcess(CoreUtil.Name_FrostyModManager);
        ProcessHelper.CloseProcess(CoreUtil.Name_MarneLauncher);

        NotifierHelper.Success("结束战地1进程操作成功");
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