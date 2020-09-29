using System;
using System.Collections.Generic;
using System.Management;
using System.Runtime.InteropServices;

namespace NetworkScanner.Infrastructure.WMI
{
    public class WmiManager
    {
        private readonly ManagementScope scope;

        public WmiManager(string ipAddress)
        {
            scope = new ManagementScope($"\\\\{ipAddress}\\root\\cimv2");
        }

        public Dictionary<string, string> FindProperty(string table, List<string> fields)
        {
            var results = new Dictionary<string, string>();

            try
            {
                scope.Connect();
                if (scope.IsConnected)
                {
                    var selectList = string.Join(',', fields.ToArray());

                    ObjectQuery query = new ObjectQuery($"SELECT {selectList} FROM {table}");
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

                    foreach (ManagementObject mo in searcher.Get())
                    {
                        foreach (PropertyData prop in mo.Properties)
                        {
                            results[prop.Name] = prop.Value.ToString();
                        }
                    }
                    return results;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}