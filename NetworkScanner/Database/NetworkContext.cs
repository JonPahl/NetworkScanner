using LiteDB;
using NetworkScanner.Abstract;
using NetworkScanner.Entities;
using System;

namespace NetworkScanner.Database
{
    public class NetworkContext : ICrud
    {
        protected string DbName { get; }
        protected string CollectionName { get; }

        public NetworkContext()
        {
            DbName = @"Filename=C:\Temp\NetworkData.db;Connection=shared";
            CollectionName = "Devices";
        }

        public int Insert<T>(T item)
        {
            try
            {
                var device = item as FoundDevice;
                using var db = new LiteDatabase(DbName);
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<FoundDevice>(CollectionName);
                //col.EnsureIndex(x => x.Id, true);

                var result = col.Insert(device);
                return result;
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

        public bool KeyExists<T>(T key)
        {
            using var db = new LiteDatabase(DbName);
            var result = db.GetCollection<FoundDevice>(CollectionName).Query().Where(x => x.DeviceId == Convert.ToString(key)).ToList();
            var found = result.Count > 0;
            return found;
        }
    }
}
