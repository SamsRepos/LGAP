﻿using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Text;
using SQLiteNetExtensions.Attributes;
using SQLite;
using Newtonsoft.Json;

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
                textBuilder.AppendLine(streamReader.ReadToEnd());
            }

        }
        catch (Exception ex)
        {
            textBuilder.AppendLine($"File at {path} could not be read");
            textBuilder.AppendLine(ex.Message);
        }

        RawText = textBuilder.ToString();

        trackFilePaths = new ObservableCollection<string>();

        string testMediaRelativePath = ".\\02. Liberating Prayer.mp3"; // in m3u file like: ".\02. Liberating Prayer.mp3", need \\ as escape char for \

#if ANDROID
        testMediaRelativePath = "./02. Liberating Prayer.mp3";
#endif

        string directoryPath = Path.GetDirectoryName(M3uFilePath);

        char[] endChars = { '/' };
        directoryPath = directoryPath.TrimEnd(endChars);

        char[] startChars = { '.' };
        testMediaRelativePath = testMediaRelativePath.TrimStart(startChars);

        //directoryPath = System.IO.Path.GetFullPath(directoryPath);

        string testMediaPath = Path.Join(directoryPath, testMediaRelativePath);

        //trackFilePaths.Add("C:\\Users\\samue\\Desktop\\LGAPtesting\\TestPlaylistDir\\02. Liberating Prayer.mp3");
        trackFilePaths.Add(testMediaPath);
    }

}