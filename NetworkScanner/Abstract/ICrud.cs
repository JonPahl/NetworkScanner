using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetworkScanner.Abstract
{
    public interface ICrud
    {

        public int Insert<T>(T item);
        public int Update<T>(T item);
        public bool KeyExists<T>(T key);

        public virtual async Task<List<T>> Search<T>(object term)
        {
            throw new NotImplementedException("Not implemented");
        }

    }
}
