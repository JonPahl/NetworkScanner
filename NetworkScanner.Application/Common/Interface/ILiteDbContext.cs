using LiteDB;
using System.Collections.Generic;

namespace NetworkScanner.Application.Common.Interface
{
    public interface ILiteDbContext
    {
        public ILiteDatabase Database { get; set; }
        public List<T> LoadAll<T>();
        public T Load<T>(object search);
        public bool Merge<T>(T item);
    }
}