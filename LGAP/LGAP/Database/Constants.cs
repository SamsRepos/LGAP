
namespace LGAP.Database;
public static class Constants
{
    public const string PlaylistsDatabaseFilename = "Playlists.db3";

    public const SQLite.SQLiteOpenFlags Flags =
        SQLite.SQLiteOpenFlags.ReadWrite   |
        SQLite.SQLiteOpenFlags.Create      |
        SQLite.SQLiteOpenFlags.SharedCache;

    public static string PlaylistDatabasePath =>
        Path.Combine(FileSystem.AppDataDirectory, PlaylistsDatabaseFilename);
}

