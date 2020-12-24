/*
using LiteDB;
using NetworkScanner.Application.Common.Interface;
using NetworkScanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetworkScanner.Infrastructure.Database
{
    public class ScanAddressContext : ICrud
    {
        protected string Table { get; }
        public string DbName { get; }
        public ILiteDatabase Database { get; set; }

        public ScanAddressContext()
        {
            DbName = @"Filename=F:\Temp\NetworkData.db;Connection=shared";
            Table = "ScanAddress";
            Database = new LiteDatabase(DbName);
        }

        public T Find<T>(string search)
        {
            using LiteDatabase db = new LiteDatabase(DbName);
            BsonValue id = new BsonValue(search);

            var collection = db.GetCollection(Table).FindById(id);
            return (T)Convert.ChangeType(collection.FirstOrDefault(), typeof(T));
        }

        public int Insert<T>(T item)
        {
            var value = item as ScanAddresses;
            using LiteDatabase db = new LiteDatabase(DbName);
            // Get a collection (or create, if doesn't exist)
            var col = db.GetCollection<ScanAddresses>(Table);
            col.EnsureIndex(x => x.Id, true);
            return col.Insert(value);
        }

        public List<T> LoadAll<T>()
        {
            using var db = new LiteDatabase(DbName);
            var records = db.GetCollection<ScanAddresses>(Table)
                .FindAll().ToList();

            return (List<T>)Convert.ChangeType(records, typeof(List<T>));
        }

        public T Load<T>(object search)
        {
            using var db = new LiteDatabase(DbName);
            BsonValue id = new BsonValue(search.ToString());

            var collection = db.GetCollection(Table).FindById(id);
            return (T)Convert.ChangeType(collection.FirstOrDefault(), typeof(T));
        }

        public bool Merge<T>(T item)
        {
            var collection = Database.GetCollection<T>(Table);
            return collection.Upsert(item);
        }

        public int Update<T>(T item)
        {
            var collection = Database.GetCollection<T>(Table);
            return Convert.ToInt32(collection.Update(item));
        }

        public IList<T> LoadDevLoadDevices<T>()
            => throw new NotImplementedException();

        public bool KeyExists<T>(T key)
        {
            var result = Database
                .GetCollection<ScanAddresses>(Table)
                .Query()
                .Where(x => x.Id.Equals(Convert.ToString(key)))
                .ToList();

            return result.Count > 0;
        }

        public int Delete<T>(T item)
        {
            throw new NotImplementedException();
        }

        public async Task<List<T>> LoadAllAsync<T>()
        {
            throw new NotImplementedException();
        }

        public List<T> FindById<T>(string id)
        {
            throw new NotImplementedException();
        }
    }
}
*/