using LiteDB;
using Microsoft.Extensions.Options;
using NetworkScanner.Application.Common.Interface;
using NetworkScanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetworkScanner.Infrastructure.Database
{
    public class NetworkContext : ILiteDbContext
    {
        protected string DbName { get; }
        protected string CollectionName { get; }
        public ILiteDatabase Database { get; set; }

        public NetworkContext(IOptions<LiteDbOptions> options)
        {
            var filePath = options.Value.DatabaseLocation.Replace(@"\\", @"\");
            var connection = options.Value.Connection;
            DbName = $"Filename={filePath};Connection={connection}";
            // ;Connection={options.Value.Connection}";
            Database = new LiteDatabase(DbName);
            CollectionName = "Device";
        }

        public int Insert<T>(T item)
        {
            try
            {
                var device = item as FoundDevice;
                using LiteDatabase liteDatabase = new LiteDatabase(DbName);
                var col = liteDatabase.GetCollection<FoundDevice>(CollectionName);
                col.EnsureIndex(x => x.Id, true);

                var result = col.Insert(device);
                return Convert.ToInt32(result.RawValue);
            }
            catch (LiteException ex)
            {
                Console.WriteLine(ex.Message, ex);
                return -1;
            }
        }

        public int Update<T>(T item)
        {
            var x = item as FoundDevice;
            using var db = new LiteDatabase(DbName);
            var col = db.GetCollection<FoundDevice>(CollectionName);
            var result = col.Update(x);
            return Convert.ToInt32(result);
        }

        public int Delete<T>(T item)
        {
            var x = item as FoundDevice;
            using var db = new LiteDatabase(DbName);
            var col = db.GetCollection<FoundDevice>(CollectionName);
            var result = col.Delete(x.Id);
            return Convert.ToInt32(result);
        }

        public bool KeyExists<T>(T key)
        {
            //using var db = new LiteDatabase(DbName);
            //var result = db.GetCollection<FoundDevice>(CollectionName).Query().Where(x => x.DeviceId == Convert.ToString(key)).ToList();

            var result = Database
                .GetCollection<FoundDevice>(CollectionName)
                .Query()
                .Where(x => x.IpAddress == Convert.ToString(key))
                .ToList();

            var found = result.Count > 0;
            return found;
        }

        public List<T> LoadAll<T>()
        {
            using var db = new LiteDatabase(DbName);
            var collection = db.GetCollection(CollectionName).FindAll();
            return (List<T>)Convert.ChangeType(collection.ToList(), typeof(List<T>));
        }

        public T Load<T>(object search)
        {
            using var db = new LiteDatabase(DbName);
            BsonValue id = new BsonValue(search.ToString());

            var collection = db.GetCollection(CollectionName).FindById(id);
            return (T)Convert.ChangeType(collection.FirstOrDefault(), typeof(T));
        }

        public bool Merge<T>(T item)
        {
            //var device = item as FoundDevice;            
            var collection = Database.GetCollection<T>(CollectionName);
            return collection.Upsert(item);
        }
    }
}
