using CommunityToolkit.Maui.Views;
using LGAP.ViewModels;
using Microsoft.Maui.Platform;

namespace LGAP.Views;

public partial class MediaPage : ContentPage
{
    private readonly MediaViewModel _vm;

    //private MediaElement mediaElem;
    public MediaPage(MediaViewModel vm)
    {
        BindingContext = vm;
        _vm = vm;
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        try
        {
            string testPath = _vm.Playlist.trackFilePaths[0];
            mediaElem.Source = MediaSource.FromFile(testPath);
        }
        catch (Exception ex)
        {
            _vm.DebugText = ex.Message;
        }
        
        //try
        //{
        //    string testPath = _vm.Playlist.trackFilePaths[0];
        //    mediaElem = new MediaElement
        //    {
        //        IsVisible = false,
        //        ShouldAutoPlay = true,
        //        Source = MediaSource.FromFile(testPath)
        //    };
        //}
        //catch (Exception e)
        //{
        //    _vm.DebugText = e.Message;
        //}


    }
}