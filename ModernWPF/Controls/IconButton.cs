namespace ModernWPF.Controls;

public class IconButton : Button
{
    /// <summary>
    /// Icon图标
    /// </summary>
    public string Icon
    {
        get { return (string)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
       nameof(Icon), typeof(string), typeof(IconButton), new PropertyMetadata(default));

    /// <summary>
    /// Icon图标大小
    /// </summary>
    public double IconSize
    {
        get { return (double)GetValue(IconSizeProperty); }
        set { SetValue(IconSizeProperty, value); }
    }
    public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
        nameof(IconSize), typeof(double), typeof(IconButton), new PropertyMetadata(default));

    /// <summary>
    /// 圆角值
    /// </summary>
    public CornerRadius CornerRadius
    {
        get { return (CornerRadius)GetValue(CornerRadiusProperty); }
        set { SetValue(CornerRadiusProperty, value); }
    }
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius), typeof(CornerRadius), typeof(IconButton), new PropertyMetadata(default));
}