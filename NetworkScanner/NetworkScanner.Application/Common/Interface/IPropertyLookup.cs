using NetworkScanner.Domain.Entities;

namespace NetworkScanner.Application.Common.Interface
{
    public interface IPropertyLookup
    {
        Result FindValue(FoundDevice device, string SearchType);
    }
}
