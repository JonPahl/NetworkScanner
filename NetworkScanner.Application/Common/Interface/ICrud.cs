namespace NetworkScanner.Application.Common.Interface
{
    public interface ICrud
    {
        public int Insert<T>(T item);
        public int Update<T>(T item);
        public int Delete<T>(T item);
        public bool KeyExists<T>(T key);
    }
}
