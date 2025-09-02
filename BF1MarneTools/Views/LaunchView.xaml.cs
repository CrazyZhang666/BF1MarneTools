using BF1MarneTools.Core;
using BF1MarneTools.Helper;
using BF1MarneTools.Utils;
using BF1MarneTools.Windows;
using CommunityToolkit.Mvvm.Input;

namespace BF1MarneTools.Views;

/// <summary>
/// LaunchView.xaml 的交互逻辑
/// </summary>
public partial class LaunchView : UserControl
{
    /// <summary>
    /// 构造方法
    /// </summary>
    public LaunchView()
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

    #region FrostyModManager
    /// <summary>
    /// 运行寒霜Mod管理器
    /// </summary>
    [RelayCommand]
    private async Task RunFrostyModManager()
    {
        // 检查 战地1 是否正在运行
        if (GameUtil.IsCheckBF1Running())
            return;

        // 检查 FrostyModManager 是否正在运行
        if (GameUtil.IsCheckFrostyModManagerRunning())
            return;

        // 如果不使用Mod文件，则直接启动战地1
        if (!Globals.IsUseMod)
        {
            await Launch.RunBf1Game();
            return;
        }

        // 寒霜Mod文件选择窗口
        var modWindow = new ModWindow
        {
            Owner = Application.Current.MainWindow,
        };
        modWindow.ShowModalDialog();
    }

    /// <summary>
    /// 关闭寒霜Mod管理器
    /// </summary>
    [RelayCommand]
    private void CloseFrostyModManager()
    {
        ProcessHelper.CloseProcess(CoreUtil.Name_FrostyModManager);
    }
    #endregion

    #region MarneLauncher
    /// <summary>
    /// 运行马恩启动器
    /// </summary>
    [RelayCommand]
    private void RunMarneLauncher()
    {
        // 检查 战地1 是否未运行
        if (GameUtil.IsCheckBF1NotRun())
            return;

        // 检查 MarneLauncher 是否正在运行
        if (GameUtil.IsCheckMarneLauncherRunning())
            return;

        ProcessHelper.OpenProcess(CoreUtil.File_Marne_MarneLauncher);
    }

    /// <summary>
    /// 关闭马恩启动器
    /// </summary>
    [RelayCommand]
    private void CloseMarneLauncher()
    {
        ProcessHelper.CloseProcess(CoreUtil.Name_MarneLauncher);
    }
    #endregion
}