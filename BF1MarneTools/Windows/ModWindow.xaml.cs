using BF1MarneTools.Helper;
using BF1MarneTools.Models;
using BF1MarneTools.Utils;
using CommunityToolkit.Mvvm.Input;

namespace BF1MarneTools.Windows;

/// <summary>
/// ModWindow.xaml 的交互逻辑
/// </summary>
public partial class ModWindow
{
    /// <summary>
    /// 数据模型绑定
    /// </summary>
    public ModModel ModModel { get; set; } = new();
    /// <summary>
    /// Mod信息动态集合
    /// </summary>
    public ObservableCollection<ModInfo> Bind_ModInfoList { get; set; } = [];

    /// <summary>
    /// 是否自动关闭窗口
    /// </summary>
    private bool _isAutoCloseWindow = false;

    /// <summary>
    /// 构造方法
    /// </summary>
    public ModWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 窗口加载完成事件
    /// </summary>
    private void Window_Mod_Loaded(object sender, RoutedEventArgs e)
    {
    }

    /// <summary>
    /// 窗口渲染完成事件
    /// </summary>
    private void Window_Mod_ContentRendered(object sender, EventArgs e)
    {
        try
        {
            _isAutoCloseWindow = false;
            ModModel.IsCanRunGame = false;

            Bind_ModInfoList.CollectionChanged += (s, e) => { ReSortMapIndex(); };

            ////////////////////////////////////////////

            if (!Directory.Exists(CoreUtil.Dir_Mods_Bf1))
                return;

            if (!File.Exists(CoreUtil.File_Config_ManagerConfig))
                return;

            var jsonText = FileHelper.ReadAllTextUtf8NoBom(CoreUtil.File_Config_ManagerConfig);
            var modConfig = JsonHelper.JsonDeserialize<ModConfig>(jsonText);

            var mods = modConfig.Games.bf1.Packs.Marne;
            var fbmodList = mods.Replace(":True", "").Split("|").ToList();

            int index = 0;
            Bind_ModInfoList.Clear();

            // 选择已有的Mod文件
            foreach (var name in fbmodList)
            {
                var path = Path.Combine(CoreUtil.Dir_Mods_Bf1, name);
                if (!File.Exists(path))
                    continue;

                var info = new FileInfo(path);

                Bind_ModInfoList.Add(new()
                {
                    Index = ++index,
                    Name = info.Name,
                    ChangeTime = info.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss"),
                    FullName = info.FullName,
                    FileSize = FileHelper.BytesToReadableValue(info.Length)
                });
            }

            // 如果已有Mod文件不为空，则允许启动游戏
            ModModel.IsCanRunGame = Bind_ModInfoList.Count > 0;
        }
        catch (Exception ex)
        {
            LoggerHelper.Error($"读取Mod配置信息出现异常: {ex.Message}");
        }
    }

    /// <summary>
    /// 窗口关闭时事件
    /// </summary>
    private void Window_Mod_Closing(object sender, CancelEventArgs e)
    {
        if (_isAutoCloseWindow)
            return;

        try
        {
            // 创建FrostyMod配置文件
            var modConfig = new ModConfig();

            // 设置战地1安装目录
            modConfig.Games.bf1.GamePath = Globals.BF1InstallDir;

            // 选择全部Mod名称
            var modNameList = from info in Bind_ModInfoList select $"{info.Name}:True";

            // 设置Mod名称并启用
            modConfig.Games.bf1.Packs.Marne = string.Join("|", modNameList);

            // 写入 Config\manager_config.json 配置文件
            FileHelper.WriteAllTextUtf8NoBom(CoreUtil.File_Config_ManagerConfig, JsonHelper.JsonSerialize(modConfig));
        }
        catch { }
    }

    /// <summary>
    /// 拖拽后重新排序
    /// </summary>
    private void ReSortMapIndex()
    {
        var index = 0;
        foreach (var item in Bind_ModInfoList)
        {
            item.Index = ++index;
        }
    }

    /// <summary>
    /// 选择Mods文件路径
    /// </summary>
    [RelayCommand]
    private void SelcetModsPath()
    {
        // 选择要安装的Mod文件（支持多选）
        var dialog = new OpenFileDialog
        {
            Title = "请选择要安装的战地1寒霜Mod文件（支持多选）",
            DefaultExt = ".fbmod",
            Filter = "寒霜Mod文件 (.fbmod)|*.fbmod",
            Multiselect = true,
            RestoreDirectory = true,
            AddExtension = true,
            CheckFileExists = true,
            CheckPathExists = true
        };

        // 当文件夹路径存在时才会赋值
        if (Directory.Exists(Globals.ModSelectDir))
            dialog.InitialDirectory = Globals.ModSelectDir;

        // 如果未选择，则退出程序
        if (dialog.ShowDialog() == false)
            return;

        // 记住本次选择的文件路径
        Globals.ModSelectDir = Path.GetDirectoryName(dialog.FileName);

        /////////////////////////////////////////////////////////

        try
        {
            ModModel.IsCanRunGame = false;

            // 智能自动创建文件夹
            // Mod文件夹如果不存在则创建
            FileHelper.CreateDirectory(CoreUtil.Dir_Mods_Bf1);

            // 清空Mod文件夹全部文件
            FileHelper.ClearDirectory(CoreUtil.Dir_Mods_Bf1);
            LoggerHelper.Info("清空旧版Mod文件成功");

            // 清空旧版模组文件夹（因为重新选择了Mod）
            var modDataDir = Path.Combine(Globals.BF1InstallDir, "ModData");
            if (Directory.Exists(modDataDir))
                FileHelper.ClearDirectory(modDataDir);

            int index = 0;
            Bind_ModInfoList.Clear();

            foreach (var url in dialog.FileNames)
            {
                // 检查mod文件是否存在
                if (!File.Exists(url))
                    continue;

                var info = new FileInfo(url);

                // 复制mod文件到 寒霜mod管理器 指定mod文件夹
                File.Copy(info.FullName, Path.Combine(CoreUtil.Dir_Mods_Bf1, info.Name), true);

                Bind_ModInfoList.Add(new()
                {
                    Index = ++index,
                    Name = info.Name,
                    ChangeTime = info.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss"),
                    FullName = info.FullName,
                    FileSize = FileHelper.BytesToReadableValue(info.Length)
                });
            }

            // 如果选择Mod文件不为空，则允许启动游戏
            ModModel.IsCanRunGame = Bind_ModInfoList.Count > 0;
        }
        catch (Exception ex)
        {
            LoggerHelper.Error($"选择安装Mods出现异常: {ex.Message}");
        }
    }

    /// <summary>
    /// 运行战地1Mod游戏
    /// </summary>
    [RelayCommand]
    private void RunBF1ModGame()
    {
        try
        {
            // 创建FrostyMod配置文件
            var modConfig = new ModConfig();

            // 设置战地1安装目录
            modConfig.Games.bf1.GamePath = Globals.BF1InstallDir;

            // 选择全部Mod名称
            var modNameList = from info in Bind_ModInfoList select $"{info.Name}:True";

            // 设置Mod名称并启用
            modConfig.Games.bf1.Packs.Marne = string.Join("|", modNameList);

            // 写入 Config\manager_config.json 配置文件
            FileHelper.WriteAllTextUtf8NoBom(CoreUtil.File_Config_ManagerConfig, JsonHelper.JsonSerialize(modConfig));
            LoggerHelper.Info("写入FrostyModManager配置文件成功");

            LoggerHelper.Info("正在启动FrostyModManager中...");
            ProcessHelper.OpenProcess(CoreUtil.File_Frosty_FrostyModManager);

            _isAutoCloseWindow = true;
            this.Close();
        }
        catch (Exception ex)
        {
            LoggerHelper.Error($"安装Mod出现异常: {ex.Message}");
        }
    }
}