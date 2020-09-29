using Elasticsearch.Net;
using Nest;
using NetworkScanner.Application.Common.Interface;
using NetworkScanner.Domain.Entities;
using System;

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
                    .OnRequestCompleted(response =>
                    {
                        /*
                        // log out the request
                        if (response.RequestBodyInBytes != null)
                        {
                            Console.WriteLine(
                                $"{response.HttpMethod} {response.Uri} \n" +
                                $"{Encoding.UTF8.GetString(response.RequestBodyInBytes)}");
                        }
                        else
                        {
                            Console.WriteLine($"{response.HttpMethod} {response.Uri}");
                        }
                        Console.WriteLine();

                        // log out the response
                        if (response.ResponseBodyInBytes != null)
                        {
                            Console.WriteLine($"Status: {response.HttpStatusCode}\n" +
                         $"{Encoding.UTF8.GetString(response.ResponseBodyInBytes)}\n" +
                         $"{new string('-', 30)}\n");
                        } else {
                            Console.WriteLine($"Status: {response.HttpStatusCode}\n" +
                         $"{new string('-', 30)}\n");
                        }
                        */
                    });

            Client = new ElasticClient(connectionSettings);
        }

        public int Delete<T>(T item)
        {
            throw new NotImplementedException();
        }

        public int Insert<T>(T item)
        {
            var device = item as FoundDevice;
            device.DeviceId = device.DeviceId.ToLower();

            var response = Client.Index(device, idx => idx.Index("devices")); //or specify index via settings.DefaultIndex("mytweetindex");
            var seqNum = response.SequenceNumber;
            return Convert.ToInt32(response.IsValid);
        }

        public bool KeyExists<T>(T key)
        {
            throw new NotImplementedException();
        }

        public int Update<T>(T item)
        {
            throw new NotImplementedException();
        }
    }
}
