using CommunityToolkit.Maui.Views;
using LGAP.ViewModels;
using Microsoft.Maui.Platform;

namespace LGAP.Views;

public partial class MediaPage : ContentPage
{
    private MediaElement mediaElem;
    public MediaPage(MediaViewModel vm)
    {
        BindingContext = vm;

        mediaElem = new MediaElement();
        mediaElem.IsVisible = false;
        string testPath = vm.Playlist.TrackFilePaths[0];
        mediaElem.Source = testPath;
        mediaElem.Play();

        InitializeComponent();
    }
}