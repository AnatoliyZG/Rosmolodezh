using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using RosMolExtension;
using System.Reflection.PortableExecutable;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RosMolServer
{
    public class DataBase : IDisposable
    {
        public required MemoryCache cache;

        public required SqlConnection sqlConnection;

        public string secretKey = "root";

        public Data[]? GetCachedContent<Data>(string? key, ulong? version = null) where Data : ReadableData, new()
        {
            if (key == null) return null;

            if(cache.TryGetValue(key, out var content))
            {
                var cache = (content as CacheContainer<Data>);

                if (version != null && version >= cache?.Version)
                {
                    throw new ContentException(Response.Status.AlreadyUpdated);
                }

                return cache?.Content;
            }

            var data = SelectDBData<Data>(key);

            cache.Set(key, new CacheContainer<Data>(key,data));

            return data;
        }


        public void ReloadDB<Data>(string key) where Data : ReadableData, new()
        {
            var data = SelectDBData<Data>(key);

            cache.Set(key, new CacheContainer<Data>(key, data));
        }


        public Data[] SelectDBData<Data>(string table) where Data : ReadableData, new()
        {
            var command = new SqlCommand($"SELECT * FROM {table}", sqlConnection);
            using SqlDataReader reader = command.ExecuteReader();

            List<Data> results = new();

            while(reader.Read())
            {
                results.Add(new Data()
                {
                    Reader = reader,
                });
            }

            reader.Close();

            return results.ToArray();
        }


        public static async Task<DataBase> CreateAsync(string server = "(localdb)\\MSSQLLocalDB", string database = "master")
        {
            return new DataBase()
            {
                cache = new MemoryCache(new MemoryCacheOptions()),
                sqlConnection = await DBConnect(server, database),
            };
        }

        public static async Task<SqlConnection> DBConnect(string server, string database)
        {
            var connection = new SqlConnection($"Server={server};Database={database};Trusted_Connection=True;");

            await connection.OpenAsync();

            Console.WriteLine("Свойства подключения:");
            Console.WriteLine($"\tСтрока подключения: {connection.ConnectionString}");
            Console.WriteLine($"\tБаза данных: {connection.Database}");
            Console.WriteLine($"\tСервер: {connection.DataSource}");
            Console.WriteLine($"\tВерсия сервера: {connection.ServerVersion}");
            Console.WriteLine($"\tСостояние: {connection.State}");
            Console.WriteLine($"\tWorkstationld: {connection.WorkstationId}");

            return connection;
        }

        public void Dispose()
        {
            sqlConnection.Close();
            sqlConnection.Dispose();
            Console.WriteLine("-> Подключение закрыто.");

            cache.Clear();
            cache.Dispose();
            Console.WriteLine("-> Кэш отчищен.");

            GC.SuppressFinalize(this);
        }

        public class CacheContainer<Data> where Data : ReadableData
        {
            public ulong Version;

            public Data[]? Content;

            public CacheContainer(string key, Data[]? content)
            {
                Version = ulong.Parse(DateTime.UtcNow.AddSeconds(-5).ToString("yyyyMMddHHmmss"));

                Console.WriteLine($"[DB] Cached \"{key}\": {Version}");

                Content = content;
            }
        }
    }
}
