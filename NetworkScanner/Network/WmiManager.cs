using System;
using System.Collections.Generic;
using System.Management;
using System.Runtime.InteropServices;

namespace NetworkScanner.Network
{
    public class WmiManager
    {        
        ManagementScope scope;

        public WmiManager(string ipAddress)
        {
            scope = new ManagementScope($"\\\\{ipAddress}\\root\\cimv2");
        }

        public Dictionary<string,string> FindProperty(string table, List<string> fields)
        {
            try
            {
                scope.Connect();

                var results = new Dictionary<string, string>();
                var selectList = string.Join(',', fields.ToArray());

                ObjectQuery query = new ObjectQuery($"SELECT {selectList} FROM {table}");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
                //ManagementObjectCollection queryCollection = searcher.Get();

                foreach (ManagementObject mo in searcher.Get())
                {
                    foreach (PropertyData prop in mo.Properties)
                    {
                        Console.WriteLine("{0}: {1}", prop.Name, prop.Value);
                        results[prop.Name] = prop.Value.ToString();
                    }
                }
                return results;
            } catch(COMException ex)
            {

            }

            return null;
        }

    }
}
