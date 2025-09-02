using BF1MarneTools.Helper;
using BF1MarneTools.Resources;
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

            LoggerHelper.Warn(Lang.LocaleWin_NotFoundBF1RegLangInfo);
        }
        catch (Exception ex)
        {
            LoggerHelper.Error(Lang.LocaleWin_GetBF1RegLangInfoException, ex);
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
                LoggerHelper.Info($"{Lang.LocaleWin_GetBF1RegLangInfoSuccess} {locale}");
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

                NotifierHelper.Info($"{Lang.LocaleWin_ChangeBF1RegLangInfoSuccess} {locale}");
                LoggerHelper.Info($"{Lang.LocaleWin_ChangeBF1RegLangInfoSuccess} {locale}");
            }
            else
            {
                NotifierHelper.Warning(Lang.LocaleWin_PlayerNotSelectBF1LangInfoTaskCanceled);
                LoggerHelper.Warn(Lang.LocaleWin_PlayerNotSelectBF1LangInfoTaskCanceled);
            }
        }
        catch (Exception ex)
        {
            NotifierHelper.Error(Lang.LocaleWin_ChangeBF1RegLangInfoException);
            LoggerHelper.Error(Lang.LocaleWin_ChangeBF1RegLangInfoException, ex);
        }

        this.Close();
    }
}