using BF1MarneTools.Helper;
using BF1MarneTools.Resources;
using BF1MarneTools.Utils;

namespace BF1MarneTools.Core;

public static class Ready
{
    /// <summary>
    /// 启动核心功能
    /// </summary>
    public static void Run()
    {
        try
        {
            // 启动服务进程（静默）
            LoggerHelper.Info(Lang.Ready_RunService);
            ProcessHelper.OpenProcess(CoreUtil.File_Service_EADesktop, string.Empty, true);

            LoggerHelper.Info(Lang.Ready_RunLSXService);
            LSXTcpServer.Run();

            LoggerHelper.Info(Lang.Ready_RunLocalHttpService);
            LocalHttpServer.Run();

            LoggerHelper.Info(Lang.Ready_RunEaWatchDogService);
            EaWatchDog.Run();

            // 检查EaApp注册表
            RegistryHelper.CheckAndAddEaAppRegistryKey();
        }
        catch (Exception ex)
        {
            LoggerHelper.Error(Lang.Ready_RunServiceException, ex);
        }
    }

    /// <summary>
    /// 停止核心功能
    /// </summary>
    public static void Stop()
    {
        try
        {
            LoggerHelper.Info(Lang.Ready_StopLocalHttpService);
            LocalHttpServer.Stop();

            LoggerHelper.Info(Lang.Ready_StopLSXService);
            LSXTcpServer.Stop();

            LoggerHelper.Info(Lang.Ready_StopEaWatchDogService);
            EaWatchDog.Stop();

            // 关闭服务进程
            CoreUtil.CloseServiceProcess();
            // 清理游戏目录第三方文件
            CoreUtil.ClearGameDirThirdFile();
        }
        catch (Exception ex)
        {
            LoggerHelper.Error(Lang.Ready_StopServiceException, ex);
        }
    }
}