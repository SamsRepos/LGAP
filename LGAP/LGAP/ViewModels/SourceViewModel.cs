using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using LGAP.Models;
using LGAP.Views;
using System.Text;
using LGAP.Database;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui;
using static SQLite.SQLite3;

namespace LGAP.ViewModels;
public partial class SourceViewModel : ObservableObject
{
    private PlaylistDatabase _playlistDatabase;

    [ObservableProperty]
    private ObservableCollection<Playlist> playlists;

    [ObservableProperty]
    private string pageTitle;

    public SourceViewModel()
    {
        PageTitle = "Loading playlists...";
        Task.Run(() => LoadAllPlaylistsAsync()).Wait();
        PageTitle = "Playlists";
    }

    [RelayCommand]
    private async Task LoadAllPlaylistsAsync()
    {
        _playlistDatabase = new PlaylistDatabase();
        playlists = new ObservableCollection<Playlist>();
        List<Playlist> playlistsAsListCollection = await _playlistDatabase.GetAllPlaylistsAsync();
        foreach (Playlist playlist in playlistsAsListCollection)
        {
            playlists.Add(playlist);
        }
    }

    [RelayCommand]
    private async Task SourceNewM3u()
    {
        var m3uFileType = new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                {DevicePlatform.Android, new[]{ "*/*" } },  // "application/x-mpegurl", "audio/mpegurl", "application/vnd.apple.mpegurl" } },  // for .pls files: { "audio/x-scpls" } },
                {DevicePlatform.WinUI, new[]{ ".m3u" } },
                {DevicePlatform.iOS, new[]{ "" } },
                {DevicePlatform.macOS, new[]{ "m3u" } },
            }
        );

        PickOptions pickOptions = new PickOptions
        {
            PickerTitle = "Pick a .m3u file",
            FileTypes = m3uFileType,
            
        };

        var fileData = await FilePicker.PickAsync(pickOptions);

        if (fileData == null) return;

#if true //ANDROID
        //var docsDirectory = Android.App.Application.Context.GetExternalFilesDir(fileData.FullPath);

        //if(Android.OS.Environment.IsExternalStorageManager)
        //{
        //    await Shell.Current.DisplayAlert("Android VERSION  R OR ABOVE", "HAVE MANAGE_EXTERNAL_STORAGE GRANTED!", "ОК");
        //}
        //else
        //{
        //    await Shell.Current.DisplayAlert("Android VERSION  R OR ABOVE", "NO HAVE MANAGE_EXTERNAL_STORAGE GRANTED!", "ОК");
        //    string package = AppInfo.Current.PackageName;
        //    var uri = Android.Net.Uri.Parse($"package:{package}");
        //    Platform.CurrentActivity.StartActivityForResult(new Intent
        //        (Android.Provider.Settings.ActionManageAppAllFilesAccessPermission, uri), 10);
        //}

        //Intent intent = Intent;
        //var uri = intent.Data;
        //string uriPath = uri.Path;
        //Java.IO.File file = new Java.IO.File(uriPath);
        //string fullFilePath = file.AbsolutePath;
        //string f = file.Path;
        //Launcher.OpenAsync(new OpenFileRequest
        //{
        //    File = new ReadOnlyFile(fullFilePath)
        //});

        //var docsDirectory = Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments);

        //File.WriteAllText($"{docsDirectory.AbsoluteFile.Path}/atextfile.txt", "contents are here");


#endif

        string path     = fileData.FullPath;
        string fileName = fileData.FileName;

        if (!(Path.GetExtension(fileName).Equals(".m3u", StringComparison.OrdinalIgnoreCase)))
        {
            await Shell.Current.DisplayAlert(
                "Error: not a .m3u file",
                path,
                "OK"
            );

            return;

        }

        StringBuilder msgSB = new StringBuilder();
        msgSB.AppendLine(path);
        msgSB.AppendLine("");
        msgSB.AppendLine("Input a name for new playlist");

        string name = (await Shell.Current.DisplayPromptAsync(
            title: "New Playlist",
            message: msgSB.ToString(),
            placeholder: "Playlist name",
            accept: "OK"
        ));

        var playlist = new Playlist(path, name);

        await _playlistDatabase.SavePlaylistAsync(playlist);
        playlists.Add(playlist);
    }

    [RelayCommand]
    private async Task DeleteAllPlaylistsAsync()
    {
        playlists.Clear();
        await _playlistDatabase.DeleteAllPlaylistsAsync();
    }

    [RelayCommand]
    private async Task GoToMedia(Playlist playlist)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { "ThePlaylist", playlist },
        };

        await Shell.Current.GoToAsync(
            nameof(MediaPage),
            navigationParameter
        );
    }

    [RelayCommand]
    private async Task DeletePlaylistAsync(Playlist playlist)
    {
        if(playlists is null) return;

        if (playlists.Contains(playlist))
        {
            playlists.Remove(playlist);
            _playlistDatabase.DeletePlaylistAsync(playlist);
        }

    }

}
