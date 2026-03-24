namespace ModernWPF.Controls;

public class WinButton : Button
{
    /// <summary>
    /// 字体图标
    /// </summary>
    public string Icon
    {
        get { return (string)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon), typeof(string), typeof(WinButton), new PropertyMetadata(default));

    /// <summary>
    /// 鼠标悬浮前景色
    /// </summary>
    public Brush OverForeground
    {
        get { return (Brush)GetValue(OverForegroundProperty); }
        set { SetValue(OverForegroundProperty, value); }
    }
    public static readonly DependencyProperty OverForegroundProperty = DependencyProperty.Register(
        nameof(OverForeground), typeof(Brush), typeof(WinButton), new PropertyMetadata(default));

    /// <summary>
    /// 鼠标悬浮背景色
    /// </summary>
    public Brush OverBackground
    {
        get { return (Brush)GetValue(OverBackgroundProperty); }
        set { SetValue(OverBackgroundProperty, value); }
    }
    public static readonly DependencyProperty OverBackgroundProperty = DependencyProperty.Register(
        nameof(OverBackground), typeof(Brush), typeof(WinButton), new PropertyMetadata(default));

    /// <summary>
    /// 鼠标按下前景色
    /// </summary>
    public Brush PressedForeground
    {
        get { return (Brush)GetValue(PressedForegroundProperty); }
        set { SetValue(PressedForegroundProperty, value); }
    }
    public static readonly DependencyProperty PressedForegroundProperty = DependencyProperty.Register(
        nameof(PressedForeground), typeof(Brush), typeof(WinButton), new PropertyMetadata(default));

    /// <summary>
    /// 鼠标按下背景色
    /// </summary>
    public Brush PressedBackground
    {
        get { return (Brush)GetValue(PressedBackgroundProperty); }
        set { SetValue(PressedBackgroundProperty, value); }
    }
    public static readonly DependencyProperty PressedBackgroundProperty = DependencyProperty.Register(
        nameof(PressedBackground), typeof(Brush), typeof(WinButton), new PropertyMetadata(default));
}