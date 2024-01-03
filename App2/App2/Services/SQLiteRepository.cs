using App2.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App2.Services
{
    public class SQLiteRepository
    {
        readonly SQLiteAsyncConnection _database;
        public SQLiteRepository(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<InventoriedItem>().Wait();
        }

        public Task<List<InventoriedItem>> GetItemsAsync(int trn_Trnid)
        {
            return _database.Table<InventoriedItem>()
                .Where(s=>s.DokumentId==trn_Trnid)
                .ToListAsync();
        }

        public Task<InventoriedItem> GetItemAsync(int dokid, string twrEan)
        {
            return _database.Table<InventoriedItem>()
                            .Where(i => i.Twr_Ean.Equals(twrEan) && i.DokumentId==dokid)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(InventoriedItem item)
        {
            if (item.ID != 0)
            {
                return _database.UpdateAsync(item);
            }
            else
            {
                return _database.InsertAsync(item);
            }
        }
 
         

        public Task<int> DeleteItemAsync(InventoriedItem item)
        {
            return _database.DeleteAsync(item);
        }

        public async Task<InventoriedItem> GetLastItemOrderForTwrGidnumer(int trnGidnumer)
        {
            // Zwraca element z największym ItemOrder dla danego Twr_Gidnumer
            return await _database.Table<InventoriedItem>()
                                  .Where(x => x.DokumentId == trnGidnumer)
                                  .OrderByDescending(x => x.ItemOrder)
                                  .FirstOrDefaultAsync();
        }

    }
}
