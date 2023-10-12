using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using RosMolExtension;
using System.Reflection.PortableExecutable;
using MySql.Data.MySqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RosMolServer
{
    public class DataBase : IDisposable
    {
        public required MemoryCache cache;

        public required MySqlConnection sqlConnection;

        public string secretKey = "v0cu6u2L_h";

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


        public DataBase CacheValue<Data>(string key) where Data : ReadableData, new()
        {
            GetCachedContent<Data>(key);

            if (!Directory.Exists(key))
            {
                Directory.CreateDirectory(key);
            }

            return this;
        }


        public void ReloadDB<Data>(string key) where Data : ReadableData, new()
        {
            var data = SelectDBData<Data>(key);

            cache.Set(key, new CacheContainer<Data>(key, data));
        }


        public Data[] SelectDBData<Data>(string table) where Data : ReadableData, new()
        {
            var command = new MySqlCommand($"SELECT * FROM {table}", sqlConnection);
            using MySqlDataReader reader = command.ExecuteReader();

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


        public void AddUser(string id)
        {
            try
            {
                string queryString = $"INSERT INTO Users(id) Values id='{id}';";

                MySqlCommand command = new MySqlCommand(queryString, sqlConnection);

                command.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public bool HasUser(string id)
        {
            string queryString = $"SELECT COUNT(1) FROM Users WHERE id='{id}';";

            MySqlCommand command = new MySqlCommand(queryString , sqlConnection);

            using MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read()) return true;

            return false;
        }

        public static async Task<DataBase> CreateAsync(string server, string port, string database, string? userId , string? password)
        {
            return new DataBase()
            {
                cache = new MemoryCache(new MemoryCacheOptions()),
                sqlConnection = await DBConnect($"server={server};port={port};user id={userId};password={password};database={database}"),
            };
        }

        public static async Task<MySqlConnection> DBConnect(string connectionString)
        {
            var connection = new MySqlConnection(connectionString);

            await connection.OpenAsync();

            Console.WriteLine("Свойства подключения:");
            Console.WriteLine($"\tСтрока подключения: {connection.ConnectionString}");
            Console.WriteLine($"\tБаза данных: {connection.Database}");
            Console.WriteLine($"\tСервер: {connection.DataSource}");
            Console.WriteLine($"\tВерсия сервера: {connection.ServerVersion}");
            Console.WriteLine($"\tСостояние: {connection.State}");

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
