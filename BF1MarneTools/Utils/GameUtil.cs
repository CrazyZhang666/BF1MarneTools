using BF1MarneTools.Helper;
using BF1MarneTools.Resources;

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
            NotifierHelper.Warning(Lang.GameUtil_IsCheckBF1Running);
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

        NotifierHelper.Warning(Lang.GameUtil_IsCheckBF1NotRun);
        return true;
    }

    /// <summary>
    /// 检查 FrostyModManager 是否正在运行
    /// </summary>
    public static bool IsCheckFrostyModManagerRunning()
    {
        if (ProcessHelper.IsAppRun(CoreUtil.Name_FrostyModManager))
        {
            NotifierHelper.Warning(Lang.GameUtil_IsCheckFrostyModManagerRunning);
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
            NotifierHelper.Warning(Lang.GameUtil_IsCheckFrostyModManagerRunning2);
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
            NotifierHelper.Warning(Lang.GameUtil_IsCheckMarneLauncherRunning);
            return true;
        }

        return false;
    }
}