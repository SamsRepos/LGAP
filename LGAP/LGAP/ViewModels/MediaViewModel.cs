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
    private string trackPositionText;

    [ObservableProperty]
    private string currentTrackText;

    [ObservableProperty]
    private string playlistInfoText;

    [ObservableProperty]
    private string playPauseButTxt;

    private MediaElement _mediaElem;
    private int _currentTrackIndex;
    
    public void Init(ref MediaElement mElem)
    {
        if (mElem is null)
        {
            CurrentTrackText = "Error: MediaElement to MediaViewModel is null";
            return;
        }

        _mediaElem = mElem;

        if(Playlist.trackFilePaths.Count == 0)
        {
            CurrentTrackText = "Error: Playlist contains zero tracks";
            return;
        }

        PlayPauseButTxt = "Play";

        StringBuilder playlistInfoSb = new StringBuilder();
        foreach (var path in Playlist.trackFilePaths)
        {
            playlistInfoSb.AppendLine(path);
        }
        PlaylistInfoText = playlistInfoSb.ToString();

        _currentTrackIndex = 0;
        LoadNextTrack();
    }

    private void UpdateCurrentTrackText(string trackFilePath, int currentIndex)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Currently playing: {currentIndex + 1} of {playlist.trackFilePaths.Count()}");

        sb.AppendLine(trackFilePath);
        
        CurrentTrackText = sb.ToString();
    }

    public void UpdateTrackPositionText()
    {
        StringBuilder sb = new StringBuilder();
        string currentTrackPosition = _mediaElem.Position.ToString();
        string fullTrackDuration = _mediaElem.Duration.ToString();

        sb.AppendLine($"{currentTrackPosition} of {fullTrackDuration}");

        TrackPositionText = sb.ToString();
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

    public void MediaEnded()
    {
        _currentTrackIndex++;

        if(_currentTrackIndex >= Playlist.trackFilePaths.Count) 
        {
            CurrentTrackText = "Media ended";
            return;
        }

        LoadNextTrack();

    }

    private void LoadNextTrack()
    {
        try
        {
            string audioPath   = Playlist.trackFilePaths[_currentTrackIndex];
            var mediaSrc       = MediaSource.FromFile(audioPath);

            //_mediaElem.Source  = null; // resetting/unloading the MediaElement
            _mediaElem.Source  = mediaSrc;
            //_mediaElem.Play();
            UpdateCurrentTrackText(audioPath, _currentTrackIndex);
        }
        catch (Exception ex)
        {
            CurrentTrackText = ex.Message;
        }
    }
}
