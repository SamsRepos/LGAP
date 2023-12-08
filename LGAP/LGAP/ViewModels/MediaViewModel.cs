using CommunityToolkit.Mvvm.ComponentModel;
using LGAP.Models;

namespace LGAP.ViewModels;


//[ObservableObject]
[QueryProperty(nameof(Playlist), "ThePlaylist")]
public partial class MediaViewModel : ObservableObject
{
    [ObservableProperty]
    private Playlist playlist;

    [ObservableProperty]
    private string debugText;
}
