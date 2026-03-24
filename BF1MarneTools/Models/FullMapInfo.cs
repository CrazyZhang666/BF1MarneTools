using CommunityToolkit.Mvvm.ComponentModel;

namespace BF1MarneTools.Models;

public partial class FullMapInfo : ObservableObject
{
    [ObservableProperty]
    private int index;

    public string Image { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string ModName { get; set; }
    public string ModCode { get; set; }
    public string DLC { get; set; }
    public string Url { get; set; }
}