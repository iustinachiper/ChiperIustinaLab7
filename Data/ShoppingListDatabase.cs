using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChiperIustinaLab7.Models;


namespace ChiperIustinaLab7.Data
{
    public class ShoppingListDatabase
    {
        readonly SQLiteAsyncConnection _database;
        public ShoppingListDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<ShopList>().Wait();
            _database.CreateTableAsync<Product>().Wait();
            _database.CreateTableAsync<ListProduct>().Wait();
            _database.CreateTableAsync<Shop>().Wait();
        }

        public Task<List<ShopList>> GetShopListsAsync()
        {
            return _database.Table<ShopList>().ToListAsync();
        }

        public Task<ShopList> GetShopListAsync(int id)
        {
            return _database.Table<ShopList>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveShopListAsync(ShopList slist)
        {
            if (slist.ID != 0)
            {
                return _database.UpdateAsync(slist);
            }
            else
            {
                return _database.InsertAsync(slist);
            }
        }
        public Task<int> DeleteShopListAsync(ShopList slist)
        {
            return _database.DeleteAsync(slist);
        }

        public Task<int> SaveProductAsync(Product product)
        {
            if (product.ID != 0)
            {
                return _database.UpdateAsync(product);
            }
            else
            {
                return _database.InsertAsync(product);
            }
        }

        public Task<int> DeleteProductAsync(Product product)
        {
            return _database.DeleteAsync(product);
        }

        public Task<List<Product>> GetProductsAsync()
        {
            return _database.Table<Product>().ToListAsync();
        }

        internal async Task SaveListProductAsync(ListProduct listProduct)
        {
            if (listProduct.ID != 0)
            {
                await _database.UpdateAsync(listProduct);
            }
            else
            {
                await _database.InsertAsync(listProduct);
            }
        }

        internal async Task<IEnumerable<Product>> GetListProductsAsync(int shopListId)
        {
            return await _database.QueryAsync<Product>(
                "SELECT P.ID, P.Description " +
                "FROM Product P " +
                "INNER JOIN ListProduct LP ON P.ID = LP.ProductID " +
                "WHERE LP.ShopListID = ?",
                shopListId);
        }
        public Task<int> DeleteListProductAsync(ListProduct product)
        {
            return _database.DeleteAsync(product);
        }
        public Task<List<Shop>> GetShopsAsync()
        {
            return _database.Table<Shop>().ToListAsync();
        }
        public Task<int> SaveShopAsync(Shop shop)
        {
            if (shop.ID != 0)
            {
                return _database.UpdateAsync(shop);
            }
            else
            {
                return _database.InsertAsync(shop);
            }
        }

    }
}
