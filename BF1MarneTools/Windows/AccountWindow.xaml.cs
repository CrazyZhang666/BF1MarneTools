using CommunityToolkit.Mvvm.Input;

namespace BF1MarneTools.Windows;

/// <summary>
/// AccountWindow.xaml 的交互逻辑
/// </summary>
public partial class AccountWindow
{
    public AccountWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 窗口加载完成事件
    /// </summary>
    private void Window_Account_Loaded(object sender, RoutedEventArgs e)
    {
    }

    /// <summary>
    /// 窗口渲染完成事件
    /// </summary>
    private void Window_Account_ContentRendered(object sender, EventArgs e)
    {
        TextBoxHint_Email.Text = Globals.Email;
        TextBoxHint_Password.Text = Globals.Password;
    }

    /// <summary>
    /// 窗口关闭事件
    /// </summary>
    private void Window_Account_Closing(object sender, CancelEventArgs e)
    {
    }

    /// <summary>
    /// 确认账号
    /// </summary>
    [RelayCommand]
    private void EnterAccount()
    {
        Globals.Email = TextBoxHint_Email.Text.Trim();
        Globals.Password = TextBoxHint_Password.Text.Trim();

        this.Close();
    }
}