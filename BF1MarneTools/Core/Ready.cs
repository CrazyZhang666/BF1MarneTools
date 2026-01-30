using BF1MarneTools.Helper;
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
            LoggerHelper.Info("正在启动服务进程...");
            ProcessHelper.OpenProcess(CoreUtil.File_Service_EADesktop, string.Empty, true);

            LoggerHelper.Info("正在启动 LSX 服务...");
            LSXTcpServer.Run();

            LoggerHelper.Info("正在启动 Local HTTP 服务...");
            LocalHttpServer.Run();

            LoggerHelper.Info("正在启动EA看门狗服务...");
            EaWatchDog.Run();

            // 检查EaApp注册表
            RegistryHelper.CheckAndAddEaAppRegistryKey();
        }
        catch (Exception ex)
        {
            LoggerHelper.Error("启动核心服务出现异常", ex);
        }
    }

    /// <summary>
    /// 停止核心功能
    /// </summary>
    public static void Stop()
    {
        try
        {
            LoggerHelper.Info("正在停止 Local HTTP 服务...");
            LocalHttpServer.Stop();

            LoggerHelper.Info("正在停止 LSX 服务...");
            LSXTcpServer.Stop();

            LoggerHelper.Info("正在停止EA看门狗服务...");
            EaWatchDog.Stop();

            // 关闭服务进程
            CoreUtil.CloseServiceProcess();
            // 清理游戏目录第三方文件
            CoreUtil.ClearGameDirThirdFile();
        }
        catch (Exception ex)
        {
            LoggerHelper.Error("停止核心服务出现异常", ex);
        }
    }
}