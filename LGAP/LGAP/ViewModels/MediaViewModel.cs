using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LGAP.Models;
using System.Text;

namespace LGAP.ViewModels;


//[ObservableObject]
[QueryProperty(nameof(Playlist), "ThePlaylist")]
public partial class MediaViewModel : ObservableObject
{
    [ObservableProperty]
    private Playlist playlist;

    [ObservableProperty]
    private string currentTrackPosition;

    [ObservableProperty]
    private string currentTrackText;

    [ObservableProperty]
    private string playlistInfoText;

    [ObservableProperty]
    private string playPauseButTxt;

    private MediaElement _mediaElem;
    public void Init(ref MediaElement mElem)
    {
        _mediaElem = mElem;

        try
        {
            int currentIndex = 0;

            string firstAudioPath = Playlist.trackFilePaths[currentIndex];
            var mediaSrc = MediaSource.FromFile(firstAudioPath);
            _mediaElem.Source = mediaSrc;
            UpdateCurrentTrackText(firstAudioPath, currentIndex);

            StringBuilder playlistInfoSB = new StringBuilder();
            foreach(var path in Playlist.trackFilePaths)
            {
                playlistInfoSB.AppendLine(path);
            }
            PlaylistInfoText = playlistInfoSB.ToString();
        }
        catch (Exception ex)
        {
            CurrentTrackText = ex.Message;
        }

        PlayPauseButTxt = "Play";
    }

    private void UpdateCurrentTrackText(string trackFilePath, int currentIndex)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Currently playing: {currentIndex + 1} of {playlist.trackFilePaths.Count()}");

        string currentTrackPosition = _mediaElem.Position.ToString();
        string fullTrackDuration    = _mediaElem.Duration.ToString();

        sb.AppendLine($"{currentTrackPosition} of {fullTrackDuration}");

        sb.AppendLine(trackFilePath);
        
        CurrentTrackText = sb.ToString();
    }

    public void UpdateTrackPositionText()
    {
        StringBuilder sb = new StringBuilder();
        string currentTrackPosition = _mediaElem.Position.ToString();
        string fullTrackDuration = _mediaElem.Duration.ToString();

        sb.AppendLine($"{currentTrackPosition} of {fullTrackDuration}");

        CurrentTrackText = sb.ToString();
    }

    [RelayCommand]
    private void PlayPause()
    {
        if (_mediaElem.CurrentState == MediaElementState.Paused /*|| _mediaElem.CurrentState == MediaElementState.Stopped*/)
        {
            _mediaElem.Play();
            PlayPauseButTxt = "Pause";
        }
        else if (_mediaElem.CurrentState == MediaElementState.Playing)
        {
            _mediaElem.Pause();
            PlayPauseButTxt = "Play";
        }
    }

}
