using NetworkScanner.Application.Common.Interface;
using NetworkScanner.Application.Compare;
using NetworkScanner.Domain.Display;
using NetworkScanner.Domain.Entities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetworkScanner.Service
{
    public class DisplayResults : IDisposable
    {
        private readonly ILogger Logger;
        private readonly ICrud DbContext;
        private readonly IDisplayResult displayResult;

        public DisplayResults(ICrud dbContext, IDisplayResult result)
        {
            DbContext = dbContext;
            Logger = Log.ForContext<DisplayResults>();
            displayResult = result;
        }

        #region Display Results

        public void BuildTable()
        {
            try
            {
                var devices = FoundDeviceCollection
                    .collection
                    .Select(x => x.Value)
                    .OrderBy(x => x, new FoundDeviceCompare());

                var cnt = 1;
                foreach (var device in devices)
                {
                    device.Id = cnt;
                    device.Key = device.GetHashCode();
                    cnt++;
                }

                #region Store-To-Db

                foreach (var device in devices)
                {
                    try
                    {
                        var result = DbContext.Insert(device);
                        //var result = DbContext.Merge(device);
                        Console.WriteLine($"Database Result: {result}");
                        /* var found = liteDbContext.KeyExists(device);
                        if (found) { liteDbContext.Update(device); }
                        else { liteDbContext.Insert(device); } */
                    } catch(Exception ex)
                    {
                        //todo: reimplement key exits lookup.
                    }
                }

                #endregion

                //var x = devices.Select(x => x).OrderBy(y => y.IpAddress).ToList();
                var tempWrite = DisplayTable(devices.Select(x => x).OrderBy(y => y.IpAddress).ToList());
                displayResult.Display(tempWrite);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// TODO: setup that string tables columns are pulled from object.
        /// </summary>
        /// <param name="devices"></param>
        /// <returns></returns>
        public string DisplayTable(List<FoundDevice> devices)
        {
            try
            {
                var item = devices.FirstOrDefault();
                var total = item.GetType().GetProperties().Length;

                var columnHeaders = new string[total];

                foreach (var prop in item.GetType().GetProperties().ToList())
                {
                    var column = GetDisplayName(item, prop.Name);
                    columnHeaders[column.Item1] = column.Item2;
                }

                return devices.ToStringTable(columnHeaders.ToArray(),
                    a => a.Id, a => a.Key, a => a.IpAddress, a => a.DeviceName, a => a.DeviceId, a => a.FoundUsing, a => a.FoundAt);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message, ex);
                return ex.Message;
            }
        }

        private Tuple<int, string> GetDisplayName(object obj, string propertyName)
        {
            try
            {
                var displayAttribute = obj.GetType().GetProperty(propertyName).CustomAttributes
                    .FirstOrDefault(x => x.AttributeType.Name.Equals("DisplayAttribute"));

                var named = displayAttribute.NamedArguments[0].TypedValue.Value.ToString();
                var cnt = Convert.ToInt32(displayAttribute.NamedArguments[1].TypedValue.Value.ToString());

                return new Tuple<int, string>(cnt, named);
            }
            catch (Exception)
            {
                return new Tuple<int, string>(0, "");
            }
        }

        #endregion Display Results

        #region Dispose Object

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool dispose)
        {
            if (dispose)
            {
                displayResult.PipeWriter.Close();
                displayResult.PipeServer.Close();
            }
        }

        #endregion
    }
}