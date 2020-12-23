using NetworkScanner.Application.Common.Interface;
using NetworkScanner.Domain;
using NetworkScanner.Domain.Entities;
using NetworkScanner.Infrastructure.Factory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace NetworkScanner.Infrastructure.Workflow
{
    /// <summary>
    /// Unit of Work
    /// </summary>
    public abstract class Auow : IUnitOfWork
    {
        protected RpcFactory Factory;
        protected List<ARpcFactory> Factories;

        protected Auow()
        {
            Factory = new RpcFactory();
            Factories = Factory.LoadFactories();
        }

        public void SetFactoryCommand(ServiceEnum service, string command)
        {
            var temp = Factories.Find(x => x.ServiceName.Equals(service));
            if (temp == null)
                throw new ArgumentException("Service {0} did not match any classes", service.ToString());

            temp.SetCommands(new List<object> { command });
        }

        public virtual TOutput BuildObject<TOutput, TInput>(TInput raw)
        {
            return (TOutput)Convert.ChangeType(raw, raw.GetType());
        }

        public virtual T BuildObject<T>(string ip)
        {
            var fd = new FoundDevice
            {
                IpAddress = ip,
                DeviceId = Utils.Common,
                DeviceName = Utils.Common,
                FoundAt = DateTime.Now,
                FoundUsing = Utils.Common
            };

            return (T)Convert.ChangeType(fd, fd.GetType());
        }

        public void Commit<T>(T item)
        {
            if (item == null)
                return;

            var device = item as FoundDevice;
            var callingName = NameOfCallingClass();

            Console.WriteLine($"{callingName} \t||\t {device}");

            if (string.IsNullOrEmpty(device.IpAddress))
                return;

            FoundDeviceCollection.Add(device);
        }

        protected static string GetHostName(string ipAddress)
        {
            try
            {
                IPHostEntry entry = Dns.GetHostEntry(ipAddress);
                return entry != null ? entry.HostName : Utils.Common;
            }
            catch (SocketException)
            {
                return Utils.Common;
            }
        }

        public virtual Result FindDeviceName(FoundDevice device)
            => null;

        public virtual Result FindDeviceId(FoundDevice device)
            => null;

        public string NameOfCallingClass()
        {
            string fullName;
            Type declaringType;
            int skipFrames = 2;
            do
            {
                MethodBase method = new StackFrame(skipFrames, false).GetMethod();
                declaringType = method.DeclaringType;
                if (declaringType == null)
                {
                    return method.Name;
                }
                skipFrames++;
                fullName = declaringType.FullName;
            }
            while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));
            return fullName;
        }
    }
}
