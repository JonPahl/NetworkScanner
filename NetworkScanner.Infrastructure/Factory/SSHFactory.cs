using NetworkScanner.Domain;
using NetworkScanner.Domain.Entities;
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NetworkScanner.Infrastructure.Factory
{
    /// <summary>
    /// Connect to remote device over SSH.
    /// </summary>
    /// <seealso cref="ARpcFactory" />
    public class SSHFactory : ARpcFactory
    {
        private SshClient ssh;
        protected string cmdResult;
        protected List<KeyValuePair<string, string>> valuePairs;
        public SSHFactory()
        {
            ServiceName = ServiceEnum.SSH;
        }

        /// <summary>
        /// Finds the value.
        /// </summary>
        /// <param name="ip">The IP Address</param>
        /// <param name="search">The search.</param>
        /// <returns></returns>
        public override Task<Result> FindValue(string ip, string search)
        {
            List<KeyValuePair<string, string>> results = new List<KeyValuePair<string, string>>();
            var result = Utils.Common;

            try
            {
                foreach (var conn in Utils.LoadSshConections(ip))
                {
                    results = Lookup(conn);

                    if (results != null)
                        break;
                }

                if (results != null)
                {
                    result = results.Find(x => x.Key.Equals(search)).Value ?? Utils.Common;
                }
                else
                {
                    result = Utils.Common;
                }
            }
            catch (NullReferenceException)
            {
                return Task.Run(() => new Result { Value = Utils.Common });
            }
            return Task.Run(() => new Result() { Value = result.Trim(), FoundAt = ServiceName.ToString() });
        }

        #region

        /// <summary>
        /// Lookups the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> Lookup(ConnectionOptions options)
        {
            ssh = new SshClient(options.IpAddress, options.User, options.Pwd);

            try
            {
                ssh.Connect();
                if (ssh.IsConnected)
                {
                    var result = ssh.RunCommand(Commands.FirstOrDefault()?.ToString());
                    cmdResult = result.Result;
                }
                ssh.Disconnect();
                if (cmdResult != null)
                    valuePairs = BuildResponse();
                return valuePairs;
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (SocketException)
            {
                return null;
            }
            catch (SshAuthenticationException)
            {
                return null;
            }
        }
        private List<KeyValuePair<string, string>> BuildResponse()
        {
            var results = new List<KeyValuePair<string, string>>();

            foreach (var item in cmdResult.Split("\n"))
            {
                try
                {
                    var keyValue = item.Split("\t", StringSplitOptions.RemoveEmptyEntries);
                    if (keyValue?.Length > 0)
                    {
                        var value = (keyValue[1].Contains(":")) ? keyValue[1].Replace(":", "") : keyValue[1];
                        var pair = new KeyValuePair<string, string>(keyValue[0], value);
                        results.Add(pair);
                    }
                }
                catch (IndexOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return results;
            #endregion
        }
    }
}