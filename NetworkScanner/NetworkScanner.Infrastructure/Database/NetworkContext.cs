using LiteDB;
using NetworkScanner.Application.Common.Interface;
using NetworkScanner.Domain.Entities;
using System;

namespace NetworkScanner.Infrastructure.Database
{
    public class NetworkContext : ICrud
    {
        protected string DbName { get; }
        protected string CollectionName { get; }

        public NetworkContext()
        {
            DbName = @"Filename=f:\Temp\NetworkData.db;Connection=shared";
            CollectionName = "Device";
        }

        public int Insert<T>(T item)
        {
            try
            {
                var device = item as FoundDevice;
                using var db = new LiteDatabase(DbName);
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<FoundDevice>(CollectionName);
                col.EnsureIndex(x => x.Id, true);

                var result = col.Insert(device);
                return Convert.ToInt32(result.RawValue);
            }
            catch (LiteException ex)
            {
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
            using var db = new LiteDatabase(DbName);
            //var result = db.GetCollection<FoundDevice>(CollectionName).Query().Where(x => x.DeviceId == Convert.ToString(key)).ToList();

            var result = db.GetCollection<FoundDevice>(CollectionName).Query().Where(x => x.IpAddress == Convert.ToString(key)).ToList();

            var found = result.Count > 0;
            return found;
        }
    }
}
