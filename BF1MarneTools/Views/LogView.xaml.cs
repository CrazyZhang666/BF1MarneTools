﻿using BF1MarneTools.Extend;
using BF1MarneTools.Models;
using NLog;
using NLog.Common;

namespace BF1MarneTools.Views;

/// <summary>
/// LogView.xaml 的交互逻辑
/// </summary>
public partial class LogView : UserControl
{
    public ObservableCollection<LogInfo> ObsCol_LogInfos { get; set; } = new();

    private const int _maxRowCount = 100;

    public LogView()
    {
        InitializeComponent();

        ToDoList();
    }

    private void ToDoList()
    {
        if (!DesignerProperties.GetIsInDesignMode(this))
        {
            var targetResult = LogManager.Configuration.AllTargets
                .Where(t => t is NlogViewerTarget).Cast<NlogViewerTarget>();

            foreach (var target in targetResult)
            {
                target.LogReceived += LogReceived;
            }
        }
    }

    private void LogReceived(AsyncLogEventInfo logEventInfo)
    {
        var logEvent = logEventInfo.LogEvent;

        this.Dispatcher.BeginInvoke(() =>
        {
            if (_maxRowCount > 0 && ObsCol_LogInfos.Count > _maxRowCount)
                ObsCol_LogInfos.RemoveAt(0);

            var item = new LogInfo()
            {
                Time = logEvent.TimeStamp.ToString("HH:mm:ss"),
                Level = logEvent.Level.Name,
                Message = $"{logEvent.Message} {logEvent.Exception?.Message}"
            };

            ObsCol_LogInfos.Add(item);

            // 滚动最后一行
            ScrollToLast();
        });
    }

    /// <summary>
    /// 日志滚动到最后一行
    /// </summary>
    private void ScrollToLast()
    {
        if (ListView_Logger.Items.Count <= 0)
            return;

        ListView_Logger.SelectedIndex = ListView_Logger.Items.Count - 1;
        ListView_Logger.ScrollIntoView(ListView_Logger.SelectedItem);
    }
}