using MongoDB.Bson;
using MongoDB.Driver;
using NetworkScanner.Application.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkScanner.Infrastructure.Database
{
    public class MongoDbContext : ICrud
    {
        protected string  Connection = "mongodb://192.168.1.28:27017";
        protected string  DatabaseName = "NetworkScanner";

        public int Delete<T>(T item)
        {
            throw new NotImplementedException();
        }

        public List<T> FindById<T>(string id)
        {
            throw new NotImplementedException();
        }

        public int Insert<T>(T item)
        {
            MongoClient dbClient = new MongoClient(Connection);
            var database = dbClient.GetDatabase(DatabaseName);            
            var collection = database.GetCollection<T>(item.GetType().Name);
            InsertOneOptions options = new InsertOneOptions();

            //UpdateOptions updateOption = new UpdateOptions { IsUpsert = true };
            ////UpdateDefinition<T> updateOption = new UpdateDefinition<T>();
            //FilterDefinition<T> filter = new FilterDefinition<T>();            
            //collection.UpdateOne(filter, item, updateOption);
            //var data = BsonDocument.Create(item);
            collection.InsertOne(item, options);
            return 1;
            //return (List<T>)Convert.ChangeType(collection, typeof(T));
        }
        public int Update<T>(T item)
        {
            throw new NotImplementedException();
        }

        public bool KeyExists<T>(T key)
        {
            return false;
        }

        public async Task<List<T>> LoadAllAsync<T>()
        {
            MongoClient dbClient = new MongoClient(Connection);
            var database = dbClient.GetDatabase(DatabaseName);
            var collection = database.GetCollection<T>(typeof(T).Name);
            return await collection.Find(_ => true).ToListAsync().ConfigureAwait(false);
        }
    }
}
