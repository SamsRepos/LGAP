using LGAP.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGAP.Database;

public class PlaylistDatabase
{
    private SQLiteAsyncConnection _database;

    private async Task Init()
    {
        if(_database is not null)
        {
            return;
        }

        _database = new SQLiteAsyncConnection(
            Constants.PlaylistDatabasePath,
            Constants.Flags
        );

        await _database.CreateTableAsync<Playlist>();
    }

    public async Task<int> SavePlaylist(Playlist playlist)
    {
        await Init();
        if(playlist.Id != 0)
        {
            return await _database.UpdateAsync(playlist);
        }
        else
        {
            return await _database.InsertAsync(playlist);
        }
    }

    public async Task<List<Playlist>> GetPlaylists()
    {
        await Init();
        return await _database.Table<Playlist>().ToListAsync();
    }

    public async Task DeletePlaylist(Playlist playlist)
    {
        await Init();
        if(playlist.Id != 0)
        {
            await _database.DeleteAsync(playlist);
        }
    }
}

