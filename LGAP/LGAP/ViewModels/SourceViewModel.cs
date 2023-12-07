using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using LGAP.Models;
using LGAP.Views;

namespace LGAP.ViewModels;
public partial class SourceViewModel : ObservableObject
{
    [ObservableProperty]
    ObservableCollection<Playlist> playlists;

    public SourceViewModel()
    {
        playlists = new ObservableCollection<Playlist>();
    }

    [RelayCommand]
    private async Task SourceNewPls()
    {
        var plsFileType = new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                {DevicePlatform.Android, new[]{ "audio/x-scpls" } },
                {DevicePlatform.WinUI, new[]{ ".pls" } },
                {DevicePlatform.iOS, new[]{ "" } },
                {DevicePlatform.macOS, new[]{ "pls" } },
            }
        );

        PickOptions pickOptions = new PickOptions
        {
            PickerTitle = "Pick a .pls file",
            FileTypes = plsFileType
        };

        var result = await FilePicker.PickAsync(pickOptions);

        if (result == null) return;

        string path = result.FullPath;

        await Shell.Current.DisplayAlert(
            "You picked...",
            path,
            "OK"
        );

        string name = (await Shell.Current.DisplayPromptAsync(
            title: "New Playlist",
            message: "Input a name for new playlist",
            placeholder: "Playlist name here",
            accept: "OK"
        ));

        playlists.Add(new Playlist
        {
            Name = name,
            Path = path
        });
    }

    [RelayCommand]
    private async Task GoToMedia(Playlist pls)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { "ThePlaylist", pls },
        };

        await Shell.Current.GoToAsync(
            nameof(MediaPage),
            navigationParameter
        );
    }

    [RelayCommand]
    private async Task DeletePls(Playlist pls)
    {
        if (playlists.Contains(pls))
        {
            playlists.Remove(pls);
        }

    }
}
