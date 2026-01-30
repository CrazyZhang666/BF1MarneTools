using BF1MarneTools.Helper;
using BF1MarneTools.Utils;

namespace BF1MarneTools.Core;

public static class EaWatchDog
{
    private static CancellationTokenSource _cancelToken = null;

    public static void Run()
    {
        if (_cancelToken != null)
        {
            LoggerHelper.Warn("EA看门狗服务正在运行，请勿重复启动");
            return;
        }

        ////////////////////////////////

        _cancelToken = new CancellationTokenSource();

        // EA看门狗任务
        Task.Run(EaWatchDogTask);

        LoggerHelper.Info("启动EA看门狗服务成功");
    }

    public static void Stop()
    {
        _cancelToken?.Cancel();

        LoggerHelper.Info("停止EA看门狗服务成功");
    }

    /// <summary>
    /// EA看门狗任务
    /// </summary>
    private static async Task EaWatchDogTask()
    {
        while (true)
        {
            if (_cancelToken.IsCancellationRequested)
            {
                LoggerHelper.Info("EA看门狗服务已取消");
                return;
            }

            try
            {
                foreach (var process in Process.GetProcesses())
                {
                    // 结束 Origin 进程树
                    if (process.ProcessName.Equals("Origin", StringComparison.OrdinalIgnoreCase))
                    {
                        process.Kill(true);
                        LoggerHelper.Info("结束Origin进程树成功");
                        continue;
                    }

                    // 结束 EADesktop 进程树
                    if (process.ProcessName.Equals("EADesktop", StringComparison.OrdinalIgnoreCase))
                    {
                        if (process.MainModule.FileName != CoreUtil.File_Service_EADesktop)
                        {
                            process.Kill(true);
                            LoggerHelper.Info("结束EADesktop进程树成功");
                            continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("EA看门狗任务出现异常", ex);
            }

            await Task.Delay(10000);
        }
    }
}