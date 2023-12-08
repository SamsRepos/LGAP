using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using Xamarin.Essentials;
using Xamarin;
using System.IO;
using System.Threading.Tasks;
using SQLiteNetExtensions.Attributes;

namespace LGAP.Models;

public partial class Playlist : ObservableObject
{
    [ObservableProperty] private int id;

    [ObservableProperty] private string m3uFilePath;
    [ObservableProperty] private string name;

    [ObservableProperty] private string rawText;

    //[ObservableProperty]
    [TextBlob("trackFilePathsBlobbed")]
    public ObservableCollection<string> trackFilePaths { get; set; }
    public string trackFilePathsBlobbed { get; set; }

    //public paramterless constructor required for SQL
    public Playlist()
    {
        trackFilePaths = new ObservableCollection<string>();
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

        string testMediaRelativePath = ".\02. Liberating Prayer.mp3";
        //string testMediaRelativePath = "02. Liberating Prayer.mp3";

        string directoryPath = Path.GetDirectoryName(M3uFilePath);

        //directoryPath = System.IO.Path.GetFullPath(directoryPath);

        string testMediaPath = directoryPath + "/" + testMediaRelativePath;
        string betterTestMediaPath = Path.Join(directoryPath, testMediaRelativePath);
        string evenBetterTestMediaPath = Path.Combine(directoryPath, testMediaRelativePath);

        trackFilePaths.Add(testMediaPath);
    }

}