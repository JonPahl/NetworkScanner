using NetworkScanner.Domain;
using NetworkScanner.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetworkScanner.Infrastructure.Factory
{
    public abstract class ARpcFactory
    {
        public ServiceEnum ServiceName { get; set; }
        public List<object> Commands { get; set; }
        public virtual Task<Result> FindValue(string ip, string search)
        {
            return Task.Run(() => new Result { Value = Utils.Common });
        }
        public virtual void SetCommands(List<object> cmds)
        {
            Commands = cmds;
        }
        public virtual List<ARpcFactory> LoadFactories()
        {
            return null;
        }
    }
}
