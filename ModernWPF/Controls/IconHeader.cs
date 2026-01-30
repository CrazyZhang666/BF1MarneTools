namespace ModernWPF.Controls;

public class IconHeader : Control
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
        nameof(Icon), typeof(string), typeof(IconHeader), new PropertyMetadata(default));

    /// <summary>
    /// 字体图标大小
    /// </summary>
    public double IconSize
    {
        get { return (double)GetValue(IconSizeProperty); }
        set { SetValue(IconSizeProperty, value); }
    }
    public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
        nameof(IconSize), typeof(double), typeof(IconHeader), new PropertyMetadata(default));

    /// <summary>
    /// 字体图标边距
    /// </summary>
    public Thickness IconMargin
    {
        get { return (Thickness)GetValue(IconMarginProperty); }
        set { SetValue(IconMarginProperty, value); }
    }
    public static readonly DependencyProperty IconMarginProperty = DependencyProperty.Register(
        nameof(IconMargin), typeof(Thickness), typeof(IconHeader), new PropertyMetadata(default));

    /// <summary>
    /// 标题文字
    /// </summary>
    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title), typeof(string), typeof(IconHeader), new PropertyMetadata(default));
}