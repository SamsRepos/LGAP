using LGAP.Models;
using SQLite;

namespace LGAP.Database;

public class PlaylistDatabase
{
    private SQLiteAsyncConnection _database;

    private async Task InitAsync()
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

    public async Task<int> SavePlaylistAsync(Playlist playlist)
    {
        await InitAsync();

        if(playlist.Id != default(int))
        {
            return await _database.UpdateAsync(playlist);
        }
        else
        {
            return await _database.InsertAsync(playlist);
        }
    }

    public async Task<Playlist> GetPlaylistAsync(int id)
    {
        await InitAsync();
        return await _database.Table<Playlist>().Where(i => i.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Playlist>> GetAllPlaylistsAsync()
    {
        await InitAsync();
        
        var res = await _database.Table<Playlist>().ToListAsync();
        _database.UpdateAllAsync(res, true);
        return res;
    }

    public async Task DeletePlaylistAsync(Playlist playlist)
    {
        await InitAsync();
        if(playlist.Id != default(int))
        {
            await _database.DeleteAsync(playlist);
        }
    }

    public async Task DeleteAllPlaylistsAsync()
    {
        await InitAsync();
        await _database.DeleteAllAsync<Playlist>();
    }
}

