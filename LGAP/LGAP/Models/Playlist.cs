using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Text;
using static Android.Text.Style.TtsSpan;

namespace LGAP.Models;

public partial class Playlist : ObservableObject
{
    [ObservableProperty] private string path;
    [ObservableProperty] private string name;

    [ObservableProperty] private string rawText;

    [ObservableProperty]
    ObservableCollection<string> trackFilePaths;

    public Playlist(string path, string name)
    {
        Path = path;
        Name = name;

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

        RawText = textBuilder.ToString();

        trackFilePaths = new ObservableCollection<string>();

        var testMediaPath = ".\02. Liberating Prayer.mp3";

        trackFilePaths.Append(testMediaPath);
    }
}