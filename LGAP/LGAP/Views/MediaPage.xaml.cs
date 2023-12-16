using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using LGAP.ViewModels;
using Microsoft.Maui.Platform;
using System.Xml.Serialization;
using static SQLite.SQLite3;

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
            var mediaSrc = MediaSource.FromFile(testPath);
            mediaElem.Source = mediaSrc;
            _vm.DebugText = testPath;
        }
        catch (Exception ex)
        {
            _vm.DebugText = ex.Message;
        }

        mediaElem.Play();

    }
}