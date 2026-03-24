namespace ModernWPF.Controls;

public class EdgeWindow : Window
{
    /// <summary>
    /// 标题栏高度
    /// </summary>
    public double CaptionHeight
    {
        get { return (double)GetValue(CaptionHeightProperty); }
        set { SetValue(CaptionHeightProperty, value); }
    }
    public static readonly DependencyProperty CaptionHeightProperty = DependencyProperty.Register(
        nameof(CaptionHeight), typeof(double), typeof(EdgeWindow), new PropertyMetadata(32.0));

    /// <summary>
    /// 标题栏背景色
    /// </summary>
    public Brush CaptionBackground
    {
        get { return (Brush)GetValue(CaptionBackgroundProperty); }
        set { SetValue(CaptionBackgroundProperty, value); }
    }
    public static readonly DependencyProperty CaptionBackgroundProperty = DependencyProperty.Register(
        nameof(CaptionBackground), typeof(Brush), typeof(EdgeWindow), new PropertyMetadata(default));

    /// <summary>
    /// 标题栏内容
    /// </summary>
    public UIElement CaptionContent
    {
        get { return (UIElement)GetValue(CaptionContentProperty); }
        set { SetValue(CaptionContentProperty, value); }
    }
    public static readonly DependencyProperty CaptionContentProperty = DependencyProperty.Register(
        nameof(CaptionContent), typeof(UIElement), typeof(EdgeWindow), new PropertyMetadata(default));

    /// <summary>
    /// 是否显示遮罩层
    /// </summary>
    public bool IsShowMaskLayer
    {
        get { return (bool)GetValue(IsShowMaskLayerProperty); }
        set { SetValue(IsShowMaskLayerProperty, value); }
    }
    public static readonly DependencyProperty IsShowMaskLayerProperty = DependencyProperty.Register(
        nameof(IsShowMaskLayer), typeof(bool), typeof(EdgeWindow), new PropertyMetadata(false));

    /// <summary>
    /// 窗口构造方法
    /// </summary>
    public EdgeWindow()
    {
        // 窗口自定义样式
        var chrome = new WindowChrome
        {
            GlassFrameThickness = new Thickness(1),
            CornerRadius = new CornerRadius(0),
            ResizeBorderThickness = new Thickness(4)
        };
        WindowChrome.SetWindowChrome(this, chrome);

        // 将标题栏高度绑定给Chrome
        BindingOperations.SetBinding(chrome, WindowChrome.CaptionHeightProperty,
            new Binding(CaptionHeightProperty.Name) { Source = this });

        // 窗口默认居中
        WindowStartupLocation = WindowStartupLocation.CenterScreen;

        ////////////////

        // 窗口最小化
        CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, (sender, e) =>
        {
            WindowState = WindowState.Minimized;
        }));

        // 窗口最大化
        CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, (sender, e) =>
        {
            WindowState = WindowState.Maximized;
        }));

        // 窗口还原
        CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, (sender, e) =>
        {
            WindowState = WindowState.Normal;
        }));

        // 窗口关闭
        CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, (sender, e) =>
        {
            Close();
        }));
    }

    /// <summary>
    /// 显示遮罩层
    /// </summary>
    public virtual void ShowMaskLayer() { IsShowMaskLayer = true; }

    /// <summary>
    /// 隐藏遮罩层
    /// </summary>
    public virtual void HideMaskLayer() { IsShowMaskLayer = false; }

    /// <summary>
    /// 显示模态对话框
    /// </summary>
    public void ShowModalDialog()
    {
        if (this.Owner as EdgeWindow is EdgeWindow edgeWindow)
        {
            edgeWindow.ShowMaskLayer();
            this.ShowDialog();
            edgeWindow.HideMaskLayer();
        }
    }
}