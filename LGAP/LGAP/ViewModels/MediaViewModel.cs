using CommunityToolkit.Mvvm.ComponentModel;
using LGAP.Models;

namespace LGAP.ViewModels;


[ObservableObject]
[QueryProperty(nameof(Pls), "ThePlaylist")]
public partial class MediaViewModel
{
    [ObservableProperty]
    private Playlist pls;

}
