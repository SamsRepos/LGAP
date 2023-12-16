using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Text;
using SQLiteNetExtensions.Attributes;
using SQLite;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace LGAP.Models;

public partial class Playlist : ObservableObject
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }

    public string IdStr 
    {
        get => Id.ToString();
        set { }
    }

    [ObservableProperty] private string m3uFilePath;
    [ObservableProperty] private string name;

    [ObservableProperty] private string rawText;

    [TextBlob(nameof(trackFilePathsBlobbed))]
    public ObservableCollection<string> trackFilePaths { get; set; }
    public string trackFilePathsBlobbed 
    {
        get => JsonConvert.SerializeObject(trackFilePaths);
        set => trackFilePaths = JsonConvert.DeserializeObject<ObservableCollection<string>>(value);
    }

    //public paramterless constructor required for SQL
    public Playlist()
    {
        
    }

    public Playlist(string path, string name)
    {
        M3uFilePath = path;
        Name        = name;

        var textBuilder = new StringBuilder();
        try
        {
            using (var streamReader = new StreamReader(path))
            {
                textBuilder.AppendLine(streamReader.ReadToEnd());
            }

        }
        catch (Exception ex)
        {
            textBuilder.AppendLine($"File at {path} could not be read");
            textBuilder.AppendLine(ex.Message);
            RawText = textBuilder.ToString();
            return;
        }

        RawText = textBuilder.ToString();

        trackFilePaths = new ObservableCollection<string>();

        string m3uDirectoryPath = Path.GetDirectoryName(M3uFilePath);

        char[] endCharsToTrim = { '/', '\\' };
        m3uDirectoryPath = m3uDirectoryPath.TrimEnd(endCharsToTrim);

        string pattern = @"(\r\n[^#\r\n]+\.mp3)";
        foreach (Match m in Regex.Matches(RawText, pattern))
        {
            string mediaRelativePath = m.Groups[1].Value;
            
            char[] startCharsToTrim = { '.', '\r', '\n',  };
            mediaRelativePath = mediaRelativePath.TrimStart(startCharsToTrim);

#if ANDROID
            mediaRelativePath = mediaRelativePath.Replace('\\', '/');
#endif

            string mediaAbsolutePath = Path.Join(m3uDirectoryPath, mediaRelativePath);

            trackFilePaths.Add(mediaAbsolutePath);
        }
    }


}