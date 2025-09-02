using BF1MarneTools.Helper;
using BF1MarneTools.Resources;
using BF1MarneTools.Utils;

namespace BF1MarneTools.Core;

public static class EaWatchDog
{
    private static CancellationTokenSource _cancelToken = null;

    public static void Run()
    {
        if (_cancelToken != null)
        {
            LoggerHelper.Warn(Lang.EaWatchDog_NotAgainRun);
            return;
        }

        ////////////////////////////////

        _cancelToken = new CancellationTokenSource();

        // EA看门狗任务
        Task.Run(EaWatchDogTask);

        LoggerHelper.Info(Lang.EaWatchDog_RunSuccess);
    }

    public static void Stop()
    {
        _cancelToken?.Cancel();

        LoggerHelper.Info(Lang.EaWatchDog_StopSuccess);
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
                LoggerHelper.Info(Lang.EaWatchDog_Cancellation);
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
                        LoggerHelper.Info(Lang.EaWatchDog_KillOriginSuccess);
                        continue;
                    }

                    // 结束 EADesktop 进程树
                    if (process.ProcessName.Equals("EADesktop", StringComparison.OrdinalIgnoreCase))
                    {
                        if (process.MainModule.FileName != CoreUtil.File_Service_EADesktop)
                        {
                            process.Kill(true);
                            LoggerHelper.Info(Lang.EaWatchDog_KillEADesktopSuccess);
                            continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(Lang.EaWatchDog_Exception, ex);
            }

            await Task.Delay(10000);
        }
    }
}