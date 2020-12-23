using NetworkScanner.Domain;
using Renci.SshNet;
using System;
using System.Collections.Generic;

namespace NetworkScanner.Infrastructure.SSH
{
    public class SshLookup
    {
        protected string cmdResult;
        protected List<KeyValuePair<string, string>> valuePairs;

        public SshLookup()
        {
            valuePairs = new List<KeyValuePair<string, string>>();
        }

        /// <summary>
        /// Finds the values.
        /// </summary>
        /// <param name="options">Remote Connection options</param>
        /// <param name="cmd">The Linux command to execute.</param>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> FindValues(ConnectionOptions options, string cmd)
        {
            try
            {
                using SshClient ssh = new SshClient(options.IpAddress, options.User, options.Pwd);
                ssh.Connect();
                if (ssh.IsConnected)
                    cmdResult = ssh.RunCommand(cmd).Result;
                ssh.Disconnect();

                if (cmdResult != null)
                    valuePairs = BuildResponse();

                return valuePairs;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private List<KeyValuePair<string, string>> BuildResponse()
        {
            string[] keyValue;
            var results = new List<KeyValuePair<string, string>>();
            foreach (var item in cmdResult.Split("\n"))
            {
                try
                {
                    keyValue = item.Split("\t", StringSplitOptions.RemoveEmptyEntries);

                    var value = "";
                    if (keyValue.Length > 1)
                    {
                        value = (keyValue[1].Contains(":")) ? keyValue[1].Replace(":", "") : keyValue[1];
                    }
                    else
                    {
                        value = keyValue[0]?.Replace(":", "") ?? "";
                    }

                    var pair = new KeyValuePair<string, string>(keyValue[0], value);
                    results.Add(pair);
                }
                catch (IndexOutOfRangeException)
                {
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return results;
        }
    }
}