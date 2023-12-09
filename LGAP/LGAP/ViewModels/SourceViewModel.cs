using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using LGAP.Models;
using LGAP.Views;
using System.Text;
using LGAP.Database;

namespace LGAP.ViewModels;
public partial class SourceViewModel : ObservableObject
{
    private PlaylistDatabase _playlistDatabase;

    [ObservableProperty]
    private ObservableCollection<Playlist> playlists;

    public SourceViewModel()
    {
        _playlistDatabase = new PlaylistDatabase();
        playlists = new ObservableCollection<Playlist>();

        Task.Run(() => this.LoadPlaylistsAsync()).Wait();
    }

    [RelayCommand]
    private async Task LoadPlaylistsAsync()
    {
        List<Playlist> playlistsAsListCollection = await _playlistDatabase.GetPlaylists();
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

        playlists.Add(playlist);
        await _playlistDatabase.SavePlaylist(playlist);
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
    private async Task DeletePls(Playlist playlist)
    {
        if(playlists is null) return;

        if (playlists.Contains(playlist))
        {
            playlists.Remove(playlist);
            _playlistDatabase.DeletePlaylist(playlist);
        }

    }
}
