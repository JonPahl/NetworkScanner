using NetworkScanner.Domain.Entities;

namespace NetworkScanner.Application.Common.Interface
{
    public interface IUnitOfWork
    {
        //string SnmpLookUp(string ip, string search);
        //string WmiLookUp(string ip, string search);
        //string MdnsLookup(string ip, string service);

        TOutput BuildObject<TOutput, TInput>(TInput raw);
        TOutput BuildObject<TOutput>(string Ip);

        Result FindDeviceName(FoundDevice device);
        Result FindDeviceId(FoundDevice device);

        void Commit<T>(T item);
    }
}
