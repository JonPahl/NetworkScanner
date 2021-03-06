﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetworkScanner.Application.Common.Interface
{
    public interface ICrud
    {
        Task<List<T>> LoadAllAsync<T>();
        List<T> FindById<T>(string id);

        public int Insert<T>(T item);
        public int Update<T>(T item);
        public int Delete<T>(T item);
        public bool KeyExists<T>(T key);
    }
}
