using CommunityToolkit.Mvvm.ComponentModel;

namespace LGAP.Models;

public partial class Playlist : ObservableObject
{
    [ObservableProperty] private string path;
    [ObservableProperty] private string name;

}