using NetworkScanner.Domain.Entities;

namespace NetworkScanner.Application.Common.Interface
{
    public interface IUnitOfWork
    {
        TOutput BuildObject<TOutput, TInput>(TInput raw);
        TOutput BuildObject<TOutput>(string Ip);

        Result FindDeviceName(FoundDevice device);
        Result FindDeviceId(FoundDevice device);

        void Commit<T>(T item);
    }
}
