/*
using Elasticsearch.Net;
using Nest;
using NetworkScanner.Application.Common.Interface;
using NetworkScanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetworkScanner.Infrastructure.Database
{
    public class ElasticSearchContext : ICrud
    {
        protected ElasticClient Client;

        public ElasticSearchContext()
        {
            var pool = new SingleNodeConnectionPool(new Uri("http://192.168.1.28:9200"));
            var pagesIndex = "pages";

            var connectionSettings = new ConnectionSettings(pool)
                    .DefaultIndex(pagesIndex)
                    .PrettyJson()
                    .DisableDirectStreaming()
                    .OnRequestCompleted(_ => { });

            Client = new ElasticClient(connectionSettings);
        }

        public int Delete<T>(T item) => throw new NotImplementedException();

        public List<T> FindById<T>(string id)
        {
            throw new NotImplementedException();
        }

        public int Insert<T>(T item)
        {
            var device = item as FoundDevice;
            device.DeviceId = device.DeviceId.ToLower();

            var response = Client.Index(device, idx => idx.Index("devices"));
            var seqNum = response.SequenceNumber;
            return Convert.ToInt32(response.IsValid);
        }

        public bool KeyExists<T>(T key) => false;

        public async Task<List<T>> LoadAllAsync<T>()
        {
            throw new NotImplementedException();
        }

        public int Update<T>(T item) => -1;
        
    }
}
*/