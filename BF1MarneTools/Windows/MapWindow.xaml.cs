using BF1MarneTools.Data;
using BF1MarneTools.Helper;
using BF1MarneTools.Models;
using CommunityToolkit.Mvvm.Input;

namespace BF1MarneTools.Windows;

/// <summary>
/// MapWindow.xaml 的交互逻辑
/// </summary>
public partial class MapWindow
{
    public ObservableCollection<ModeInfo> Bind_GameModeInfos { get; set; } = [];
    public ObservableCollection<FullMapInfo> Bind_GameMapInfos { get; set; } = [];
    public ObservableCollection<FullMapInfo> Bind_GameMapModeInfos { get; set; } = [];

    /// <summary>
    /// 地图列表文件路径
    /// </summary>
    private string MapListFilePath { get; set; }

    /// <summary>
    /// 构造方法
    /// </summary>
    public MapWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 窗口加载完成事件
    /// </summary>
    private async void Window_Map_Loaded(object sender, RoutedEventArgs e)
    {
        await Task.Run(InitMapData);
    }

    /// <summary>
    /// 窗口渲染完成事件
    /// </summary>
    private async void Window_Map_ContentRendered(object sender, EventArgs e)
    {
        foreach (var item in ModeDB.GameModeInfoDb)
        {
            Bind_GameModeInfos.Add(item);
            await Task.Delay(1);
        }
        ListBox_GameModeInfos.SelectedIndex = 0;

        // 地图索引重新排序
        Bind_GameMapModeInfos.CollectionChanged += (s, e) =>
        {
            var index = 0;
            foreach (var item in Bind_GameMapModeInfos)
            {
                item.Index = ++index;
            }
        };
    }

    /// <summary>
    /// 窗口关闭时事件
    /// </summary>
    private void Window_Map_Closing(object sender, CancelEventArgs e)
    {
    }

    /// <summary>
    /// 初始化地图数据
    /// </summary>
    private async Task InitMapData()
    {
        try
        {
            MapListFilePath = Path.Combine(Globals.BF1InstallDir, "Instance\\MapList.txt");

            if (!Globals.IsUseServer)
            {
                var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var marneDir = Path.Combine(appDataDir, "Marne");

                MapListFilePath = Path.Combine(marneDir, "MapList.txt");
            }

            // 智能自动创建文件夹
            FileHelper.CreateDirectory(MapListFilePath);

            // 如果文件不存，则创建一个新文件
            if (!File.Exists(MapListFilePath))
            {
                // 直接创建一个空文件
                FileHelper.WriteAllTextUtf8NoBom(MapListFilePath, string.Empty);
            }

            ///////////////////////////////////////////////////////////////

            var allMapLines = File.ReadAllLines(MapListFilePath, FileHelper.Encoding_UTF8_NoBOM);
            var index = 0;
            foreach (var mapLine in allMapLines)
            {
                var mapArray = mapLine.Split(';');
                if (mapArray.Length != 3)
                    continue;

                // 根据地图路径获取地图详情
                var mapInfo = MapDB.GetMapInfoByUrl(mapArray[0]);
                if (mapInfo == null)
                    continue;

                // 根据模式代码获取模式详情
                var modeInfo = ModeDB.GetModeInfoByCode(mapArray[1]);
                if (modeInfo == null)
                    continue;

                this.Dispatcher.Invoke(() =>
                {
                    Bind_GameMapModeInfos.Add(new()
                    {
                        Index = ++index,
                        Image = MapDB.GetMapImageByName(mapInfo.Code),
                        Name = mapInfo.Name,
                        Code = mapInfo.Code,
                        ModName = modeInfo.Name,
                        ModCode = modeInfo.Code,
                        DLC = mapInfo.DLC,
                        Url = mapInfo.Url
                    });
                });

                await Task.Delay(1);
            }
        }
        catch (Exception ex)
        {
            LoggerHelper.Error($"初始化地图列表出现异常: {ex.Message}");
        }
    }

    /// <summary>
    /// 游戏模式列表选择项改变事件
    /// </summary>
    private void ListBox_GameModeInfos_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ListBox_GameModeInfos.SelectedItem is null)
            return;

        var gameMapInfo = ListBox_GameMapInfos.SelectedItem as FullMapInfo;
        var selectedMapCode = gameMapInfo?.Code;
        var selectedMapIndex = 0;

        Bind_GameMapInfos.Clear();

        var modeInfo = ListBox_GameModeInfos.SelectedItem as ModeInfo;

        var index = 0;
        var result = MapDB.GameMapInfoDb.Where(x => x.Modes.Contains(modeInfo.Code));
        foreach (var item in result)
        {
            Bind_GameMapInfos.Add(new()
            {
                Index = ++index,
                Image = MapDB.GetMapImageByName(item.Code),
                Name = item.Name,
                Code = item.Code,
                ModName = modeInfo.Name,
                ModCode = modeInfo.Code,
                DLC = item.DLC,
                Url = item.Url
            });

            // 记住上次选择的地图
            if (selectedMapCode != null && selectedMapCode == item.Code)
                selectedMapIndex = index - 1;
        }
        ListBox_GameMapInfos.SelectedIndex = selectedMapIndex;
    }

    /// <summary>
    /// 滚动到最后一行
    /// </summary>
    private void ScrollToLast()
    {
        if (ListBox_GameMapModeInfos.Items.Count <= 0)
            return;

        ListBox_GameMapModeInfos.SelectedIndex = ListBox_GameMapModeInfos.Items.Count - 1;
        ListBox_GameMapModeInfos.ScrollIntoView(ListBox_GameMapModeInfos.SelectedItem);
    }

    /// <summary>
    /// 添加地图
    /// </summary>
    [RelayCommand]
    private void AddMap()
    {
        if (ListBox_GameMapInfos.SelectedItem is null)
            return;

        var fullMapInfo = ListBox_GameMapInfos.SelectedItem as FullMapInfo;

        Bind_GameMapModeInfos.Add(new()
        {
            Index = fullMapInfo.Index,
            Image = fullMapInfo.Image,
            Name = fullMapInfo.Name,
            Code = fullMapInfo.Code,
            ModName = fullMapInfo.ModName,
            ModCode = fullMapInfo.ModCode,
            DLC = fullMapInfo.DLC,
            Url = fullMapInfo.Url
        });

        // 滚动到最后一行
        ScrollToLast();
    }

    /// <summary>
    /// 删除地图
    /// </summary>
    [RelayCommand]
    private void DeleteMap()
    {
        var index = ListBox_GameMapModeInfos.SelectedIndex;
        if (index == -1)
            return;

        Bind_GameMapModeInfos.RemoveAt(index);

        var count = ListBox_GameMapModeInfos.Items.Count;
        // 一般为三种情况，选中第一个，选中最后一个，保持不变
        if (index == 0 && count != 0)
            ListBox_GameMapModeInfos.SelectedIndex = 0;
        else if (index == count && count != 0)
            ListBox_GameMapModeInfos.SelectedIndex = index - 1;
        else
            ListBox_GameMapModeInfos.SelectedIndex = index;
    }

    /// <summary>
    /// 清空地图列表
    /// </summary>
    [RelayCommand]
    private void ClearMapList()
    {
        Bind_GameMapModeInfos.Clear();
    }

    /// <summary>
    /// 保存地图列表
    /// </summary>
    [RelayCommand]
    private void SaveMapList()
    {
        try
        {
            var strBuilder = new StringBuilder();

            // 地图为空则保存空文件
            // 当选择地图不为空时，填充新的地图数据
            if (Bind_GameMapModeInfos.Count != 0)
            {
                var lastItem = Bind_GameMapModeInfos.Last();
                foreach (var item in Bind_GameMapModeInfos)
                {
                    strBuilder.Append($"{item.Url};{item.ModCode};1");

                    if (item != lastItem)
                        strBuilder.Append(Environment.NewLine);
                }
            }

            FileHelper.WriteAllTextUtf8NoBom(MapListFilePath, strBuilder.ToString());

            LoggerHelper.Info("保存地图列表成功");
            LoggerHelper.Info($"地图列表文件路径为 {MapListFilePath}");
        }
        catch (Exception ex)
        {
            LoggerHelper.Error($"保存地图列表出现异常: {ex.Message}");
        }

        this.Close();
    }
}