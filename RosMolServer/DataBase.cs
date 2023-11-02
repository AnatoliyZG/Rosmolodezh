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

namespace RosMolServer
{
    public class DataBase : IDisposable
    {
        public required MemoryCache cache;

        public required MySqlConnection sqlConnection;

        public string secretKey = "v0cu6u2L_h";

        public Dictionary<string, User> Users = new Dictionary<string, User>();

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
            sqlConnection.Open();

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

            sqlConnection.Close();

            return results.ToArray();
        }

        public void LoadUsers()
        {
            sqlConnection.Open();

            var command = new MySqlCommand($"SELECT * FROM Users", sqlConnection);

            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Users.Add(reader.GetString(1), new User()
                {
                    UserId = reader.GetInt32(0),
                    Password = reader.GetString(2),
                    Name = reader.GetString(3),
                    Phone = reader.GetString(4),
                    City = reader.GetString(5),
                    VkLink = reader.GetString(6),
                    Direction = reader.GetString(7),
                    BornDate = ParseDate(),
                });

                DateTime ParseDate()
                {
                    try
                    {
                        return reader.GetDateTime(8);
                    }
                    catch
                    {
                        return DateTime.MinValue;
                    }
                }
            }

            sqlConnection.Close();

            Console.WriteLine($"Loaded {Users.Count} users");
        }

        public void AddUser(int userId, RegisterRequest request)
        {
            sqlConnection.Open();

            try
            {
                string queryString = $"INSERT INTO Users(id, login, pass, name, phone, city, vkLink, direction, bornDate) Values({userId},'{request.login}','{request.password}', '{request.name}', '{request.phone}', '{request.city}', '{request.vkLink}', '{request.direction}', '{request.bornDate.ToString("yyyy.MM.dd")}');";

                MySqlCommand command = new MySqlCommand(queryString, sqlConnection);

                command.ExecuteNonQuery();

                Users.Add(request.login, new User()
                {
                    UserId = userId,
                    Password = request.password,
                    City = request.city,
                    BornDate = request.bornDate,
                    Direction = request.direction,
                    VkLink = request.vkLink,
                    Name = request.name,
                    Phone = request.phone,
                });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public User? GetUser(string login)
        {
            if(Users.TryGetValue(login, out var user))
            {
                return user;
            }
            return null;
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

            connection.Close();

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
