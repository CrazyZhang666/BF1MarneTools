using BF1MarneTools.Helper;

namespace BF1MarneTools.Utils;

public static class GameUtil
{
    /// <summary>
    /// 检查 战地1 是否正在运行
    /// </summary>
    public static bool IsCheckBF1Running()
    {
        if (ProcessHelper.IsAppRun(CoreUtil.Name_BF1))
        {
            NotifierHelper.Warning("战地1正在运行，请先关闭游戏");
            return true;
        }

        return false;
    }

    /// <summary>
    /// 检查 战地1 是否未运行
    /// </summary>
    public static bool IsCheckBF1NotRun()
    {
        if (ProcessHelper.IsAppRun(CoreUtil.Name_BF1))
            return false;

        NotifierHelper.Warning("未发现战地1进程，操作取消");
        return true;
    }

    /// <summary>
    /// 检查 FrostyModManager 是否正在运行
    /// </summary>
    public static bool IsCheckFrostyModManagerRunning()
    {
        if (ProcessHelper.IsAppRun(CoreUtil.Name_FrostyModManager))
        {
            NotifierHelper.Warning("FrostyModManager正在运行，请不要重复启动");
            return true;
        }

        return false;
    }

    /// <summary>
    /// 检查 FrostyModManager 是否正在运行2
    /// </summary>
    public static bool IsCheckFrostyModManagerRunning2()
    {
        if (ProcessHelper.IsAppRun(CoreUtil.Name_FrostyModManager))
        {
            NotifierHelper.Warning("FrostyModManager正在运行，请先关闭程序");
            return true;
        }

        return false;
    }

    /// <summary>
    /// 检查 MarneLauncher 是否正在运行
    /// </summary>
    public static bool IsCheckMarneLauncherRunning()
    {
        if (ProcessHelper.IsAppRun(CoreUtil.Name_MarneLauncher))
        {
            NotifierHelper.Warning("MarneLauncher正在运行，请不要重复启动");
            return true;
        }

        return false;
    }
}