using BF1MarneTools.Helper;
using BF1MarneTools.Utils;
using CommunityToolkit.Mvvm.Input;

namespace BF1MarneTools.Views;

/// <summary>
/// NameView.xaml 的交互逻辑
/// </summary>
public partial class NameView : UserControl
{
    /// <summary>
    /// 默认玩家名称
    /// </summary>
    private const string DefaultName = "BF1MarneTools";

    /// <summary>
    /// 构造方法
    /// </summary>
    public NameView()
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
        // 使用默认玩家名称
        TextBox_PlayerName.Text = DefaultName;

        // 如果玩家名称文件不存在，则跳过
        if (!File.Exists(CoreUtil.File_Config_PlayerName))
            return;

        try
        {
            // 读取本地保存玩家名称
            var fileContent = FileHelper.ReadAllTextUtf8NoBom(CoreUtil.File_Config_PlayerName).Trim();
            // 如果玩家名称内容不为空
            if (!string.IsNullOrWhiteSpace(fileContent))
            {
                TextBox_PlayerName.Text = fileContent;
                return;
            }

            // 判断名称是否超过限制
            var nameHexBytes = Encoding.UTF8.GetBytes(DefaultName);
            if (nameHexBytes.Length <= 15)
            {
                // 写入玩家名称
                FileHelper.WriteAllTextUtf8NoBom(CoreUtil.File_Config_PlayerName, DefaultName);
            }
        }
        catch (Exception ex)
        {
            LoggerHelper.Error("初始化自定义玩家名称出现异常", ex);
        }
    }

    /// <summary>
    /// 修改玩家名称
    /// </summary>
    [RelayCommand]
    private void ChangePlayerName()
    {
        // 检查 战地1 是否正在运行
        if (GameUtil.IsCheckBF1Running())
            return;

        var playerName = TextBox_PlayerName.Text.Trim();
        if (string.IsNullOrWhiteSpace(playerName))
        {
            NotifierHelper.Warning("玩家名称不能为空，请重新修改");
            return;
        }

        var nameHexBytes = Encoding.UTF8.GetBytes(playerName);
        if (nameHexBytes.Length > 15)
        {
            NotifierHelper.Warning("玩家名称不能超过15字节，请重新修改");
            return;
        }

        try
        {
            // 写入玩家名称
            FileHelper.WriteAllTextUtf8NoBom(CoreUtil.File_Config_PlayerName, playerName);
            NotifierHelper.Success("修改玩家名称成功，请启动游戏查看");
        }
        catch (Exception ex)
        {
            LoggerHelper.Error("修改玩家名称出现异常", ex);
            NotifierHelper.Error("修改玩家名称出现异常");
        }
    }
}