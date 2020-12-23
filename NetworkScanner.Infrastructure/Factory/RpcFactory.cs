using NetworkScanner.Domain;
using NetworkScanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetworkScanner.Infrastructure.Factory
{
    public class RpcFactory : ARpcFactory
    {
        /// <summary>
        /// Loads the factory.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">No factory for type {name} was found.</exception>
        public ARpcFactory LoadFactory(ServiceEnum name)
        {
            var factory = LoadFactories().Find(x => x.ServiceName.Equals(name));
            return factory ?? throw new ArgumentException($"No factory for type {name} was found.");
        }

        /// <summary>
        /// Loads the factories.
        /// </summary>
        /// <returns></returns>
        public override List<ARpcFactory> LoadFactories()
        {
            return typeof(ARpcFactory)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(ARpcFactory)) && !t.IsAbstract)
                .Select(t => (ARpcFactory)Activator.CreateInstance(t)).ToList();
        }

        /// <summary>
        /// Finds the value.
        /// </summary>
        /// <param name="ip">The IP address</param>
        /// <param name="search">The search.</param>
        /// <returns></returns>
        public override async Task<Result> FindValue(string ip, string search)
        {
            return await Task.Run(() => new Result { Value = Utils.Common }).ConfigureAwait(false);
        }
    }
}