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

    private async Task SourceNewMp3()
    {
        var mp3FileType = new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                {DevicePlatform.Android, new[]{ "*/*" } },  // "application/x-mpegurl", "audio/mpegurl", "application/vnd.apple.mpegurl" } },  // for .pls files: { "audio/x-scpls" } },
                {DevicePlatform.WinUI, new[]{ ".mp3" } },
                {DevicePlatform.iOS, new[]{ "" } },
                {DevicePlatform.macOS, new[]{ "mp3" } },
            }
        );

        PickOptions pickOptions = new PickOptions
        {
            PickerTitle = "Pick a .mp3 file",
            FileTypes = mp3FileType,
        };

        var fileData = await FilePicker.PickAsync(pickOptions);

        if (fileData == null) return;

        string path = fileData.FullPath;
        string fileName = fileData.FileName;

        if (!(Path.GetExtension(fileName).Equals(".mp3", StringComparison.OrdinalIgnoreCase)))
        {
            await Shell.Current.DisplayAlert(
                "Error: not a .mp3 file",
                path,
                "OK"
            );

            return;

        }

        
        _vm.DebugText = path;
        mediaElem.Source = path;
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();

        _vm.DebugText = _vm.Playlist.Id.ToString();

        try
        {
            //SourceNewMp3();

            string testPath = _vm.Playlist.trackFilePaths[0];
            var mediaSrc = MediaSource.FromFile(testPath);
            mediaElem.Source = mediaSrc;
        }
        catch (Exception ex)
        {
            _vm.DebugText = ex.Message;
        }

        mediaElem.Play();

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