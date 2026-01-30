using CommunityToolkit.Mvvm.Input;

namespace BF1MarneTools.Windows;

/// <summary>
/// DialogWindow.xaml 的交互逻辑
/// </summary>
public partial class DialogWindow
{
    public string Caption { get; set; }
    public string MessageBoxText { get; set; }

    public bool Result { get; set; }

    public DialogWindow(string caption, string messageBoxText)
    {
        Caption = caption;
        MessageBoxText = messageBoxText;

        InitializeComponent();
    }

    public static bool Show(string messageBoxText, string caption)
    {
        if (Application.Current.MainWindow == null)
        {
            var dialogWindow = new DialogWindow(caption, messageBoxText)
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            return dialogWindow.ShowDialogNoOwner();
        }
        else
        {
            var dialogWindow = new DialogWindow(caption, messageBoxText)
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            return dialogWindow.ShowDialogHaveOwner();
        }
    }

    private bool ShowDialogNoOwner()
    {
        SystemSounds.Exclamation.Play();

        this.Topmost = true;

        StackPanel_NoOwnerDialog.Visibility = Visibility.Visible;
        StackPanel_HaveOwnerDialog.Visibility = Visibility.Collapsed;

        this.ShowDialog();
        return Result;
    }

    private bool ShowDialogHaveOwner()
    {
        SystemSounds.Exclamation.Play();

        StackPanel_NoOwnerDialog.Visibility = Visibility.Collapsed;
        StackPanel_HaveOwnerDialog.Visibility = Visibility.Visible;

        this.ShowModalDialog();
        return Result;
    }

    [RelayCommand]
    private void Enter()
    {
        Result = true;
        this.Close();
    }

    [RelayCommand]
    private void Cancel()
    {
        Result = false;
        this.Close();
    }
}