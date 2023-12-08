using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using LGAP.Models;
using LGAP.Views;
using System.Text;

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
    private async Task SourceNewM3u()
    {
        //var m3uFileType = new FilePickerFileType(
        //    new Dictionary<DevicePlatform, IEnumerable<string>>
        //    {
        //        {DevicePlatform.Android, new[]{ "application/x-mpegurl", "audio/mpegurl", "application/vnd.apple.mpegurl" } },  // for .pls files: { "audio/x-scpls" } },
        //        {DevicePlatform.WinUI, new[]{ ".m3u" } },
        //        {DevicePlatform.iOS, new[]{ "" } },
        //        {DevicePlatform.macOS, new[]{ "m3u" } },
        //    }
        //);

        PickOptions pickOptions = new PickOptions
        {
            PickerTitle = "Pick a .m3u file",
            //FileTypes = m3uFileType
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

        var textBuilder = new StringBuilder();
        try
        {
            using (var streamReader = new StreamReader(path))
            {
                textBuilder.Append(streamReader.ReadToEnd());
            }
            
        }
        catch (Exception ex)
        {
            textBuilder.AppendLine($"File at {path} could not be read");
            textBuilder.AppendLine(ex.Message);
        }

        playlists.Add(new Playlist
        {
            Name = name,
            Path = path,
            Text = textBuilder.ToString()
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
