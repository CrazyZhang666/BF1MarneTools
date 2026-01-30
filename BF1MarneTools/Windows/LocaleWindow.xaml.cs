using BF1MarneTools.Helper;
using CommunityToolkit.Mvvm.Input;
using ModernWPF.Controls;

namespace BF1MarneTools.Windows;

/// <summary>
/// LocaleWindow.xaml 的交互逻辑
/// </summary>
public partial class LocaleWindow
{
    private const string _regedit = "SOFTWARE\\EA Games\\Battlefield 1";
    private const string _regedit2 = "SOFTWARE\\WOW6432Node\\EA Games\\Battlefield 1";

    /// <summary>
    /// 构造方法
    /// </summary>
    public LocaleWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 窗口加载完成事件
    /// </summary>
    private void Window_Locale_Loaded(object sender, RoutedEventArgs e)
    {
    }

    /// <summary>
    /// 窗口渲染完成事件
    /// </summary>
    private void Window_Locale_ContentRendered(object sender, EventArgs e)
    {
        try
        {
            // 第一次查找
            var locale = RegistryHelper.GetRegistryTargetVaule(_regedit, "Locale");
            if (!string.IsNullOrWhiteSpace(locale))
            {
                SetRadioButtonIsChecked(locale);
                return;
            }
            // 第二次查找
            locale = RegistryHelper.GetRegistryTargetVaule(_regedit2, "Locale");
            if (string.IsNullOrWhiteSpace(locale))
            {
                SetRadioButtonIsChecked(locale);
                return;
            }

            LoggerHelper.Warn("未找到战地1注册表语言信息");
        }
        catch (Exception ex)
        {
            LoggerHelper.Error("获取BF1注册表语言信息时出现异常", ex);
        }
    }

    /// <summary>
    /// 窗口关闭事件
    /// </summary>
    private void Window_Locale_Closing(object sender, CancelEventArgs e)
    {
    }

    /// <summary>
    /// 获取选中的UI按钮信息
    /// </summary>
    private string GetRadioButtonIsChecked()
    {
        foreach (UIElement element in UniformGrid_Language.Children)
        {
            if (element is not ImageRadioButton radioButton)
                continue;

            if (radioButton.IsChecked == true)
                return radioButton.Tag.ToString();
        }

        return string.Empty;
    }

    /// <summary>
    /// 设置指定UI按钮选中
    /// </summary>
    private void SetRadioButtonIsChecked(string locale)
    {
        foreach (UIElement element in UniformGrid_Language.Children)
        {
            if (element is not ImageRadioButton radioButton)
                continue;

            if (radioButton.Tag.ToString().Equals(locale, StringComparison.OrdinalIgnoreCase))
            {
                radioButton.IsChecked = true;
                LoggerHelper.Info($"获取BF1注册表语言信息成功 {locale}");
                return;
            }
        }
    }

    /// <summary>
    /// 修改游戏语言
    /// </summary>
    [RelayCommand]
    private void ChangeGameLocale()
    {
        try
        {
            var locale = GetRadioButtonIsChecked();
            if (!string.IsNullOrWhiteSpace(locale))
            {
                RegistryHelper.SetRegistryTargetVaule(_regedit, "Locale", locale);
                RegistryHelper.SetRegistryTargetVaule(_regedit2, "Locale", locale);

                NotifierHelper.Info($"修改战地1注册表语言信息成功 {locale}");
                LoggerHelper.Info($"修改战地1注册表语言信息成功 {locale}");
            }
            else
            {
                NotifierHelper.Warning("玩家未选择战地1语言信息，操作取消");
                LoggerHelper.Warn("玩家未选择战地1语言信息，操作取消");
            }
        }
        catch (Exception ex)
        {
            NotifierHelper.Error("修改BF1注册表语言信息时出现异常");
            LoggerHelper.Error("修改BF1注册表语言信息时出现异常", ex);
        }

        this.Close();
    }
}