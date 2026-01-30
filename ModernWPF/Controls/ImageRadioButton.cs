namespace ModernWPF.Controls;

public class ImageRadioButton : RadioButton
{
    /// <summary>
    /// 图片路径
    /// </summary>
    public ImageSource Source
    {
        get { return (ImageSource)GetValue(SourceProperty); }
        set { SetValue(SourceProperty, value); }
    }
    public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
        nameof(Source), typeof(ImageSource), typeof(ImageRadioButton), new PropertyMetadata(default));
}