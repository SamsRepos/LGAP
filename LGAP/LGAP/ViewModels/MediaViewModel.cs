using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LGAP.Models;
using System.Collections.ObjectModel;
using System.Text;

namespace LGAP.ViewModels;


//[ObservableObject]
[QueryProperty(nameof(Playlist), "ThePlaylist")]
public partial class MediaViewModel : ObservableObject
{
    [ObservableProperty]
    private Playlist playlist;

    [ObservableProperty]
    private ObservableCollection<MediaTrack> mediaTracks;

    private int _currentTrackIndex;

    [ObservableProperty] private string trackPositionText;
    [ObservableProperty] private string currentTrackText;
    [ObservableProperty] private string playlistInfoText;
    [ObservableProperty] private string playPauseButTxt;

    public void Init()
    {

        if(Playlist.trackFilePaths.Count == 0)
        {
            CurrentTrackText = "Error: Playlist contains zero tracks";
            return;
        }

        PlayPauseButTxt = "Play";

        MediaTracks = new ObservableCollection<MediaTrack>();

        StringBuilder playlistInfoSb = new StringBuilder();
        for(int index = 0; index < Playlist.trackFilePaths.Count(); ++index)
        {
            var path = Playlist.trackFilePaths[index];

            MediaTracks.Add(new MediaTrack(path, index));
            playlistInfoSb.AppendLine(path);
        }
        PlaylistInfoText = playlistInfoSb.ToString();

        _currentTrackIndex = 0;
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
        //if (_mediaElem is null) return;

        //StringBuilder sb = new StringBuilder();
        //string currentTrackPosition = _mediaElem.Position.ToString();
        //string fullTrackDuration = _mediaElem.Duration.ToString();

        //sb.AppendLine($"{currentTrackPosition} of {fullTrackDuration}");

        //TrackPositionText = sb.ToString();
    }

    [RelayCommand]
    private void PlayPause()
    {
        //if (_mediaElem.CurrentState == MediaElementState.Paused /*|| _mediaElem.CurrentState == MediaElementState.Stopped*/)
        //{
        //    _mediaElem.Play();
        //    PlayPauseButTxt = "Pause";
        //}
        //else if (_mediaElem.CurrentState == MediaElementState.Playing)
        //{
        //    _mediaElem.Pause();
        //    PlayPauseButTxt = "Play";
        //}
    }

    public void MediaEnded(Frame mediaElemsFrame)
    {
        _currentTrackIndex++;

        if(_currentTrackIndex >= Playlist.trackFilePaths.Count) 
        {
            CurrentTrackText = "Media ended";
            return;
        }

        var nextTrack = MediaTracks[_currentTrackIndex];

        string nextMediaElemName = nextTrack.Name;

        MediaElement mediaElem = (MediaElement)mediaElemsFrame.FindByName(nextMediaElemName);

        mediaElem.Play();
    }

}
