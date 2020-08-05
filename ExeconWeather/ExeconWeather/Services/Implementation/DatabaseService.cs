using ExeconWeather.Services.Interface;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ExeconWeather.Services.Implementation
{
    public class DatabaseService<T> : IDatabaseService<T> where T : class, new()
    {
        private readonly SQLiteAsyncConnection databaseConnection;

        public DatabaseService()
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ExeconWeatherDatabase.db");
            databaseConnection = new SQLiteAsyncConnection(databasePath);
        }

        public async Task CreateTable()
        {
            await databaseConnection.CreateTableAsync<T>();
        }

        public async Task<List<T>> Get()
        {
            return await databaseConnection.Table<T>().ToListAsync();
        }

        public async Task<int> Insert(T model)
        {
            return await databaseConnection.InsertAsync(model);
        }
    }
}
