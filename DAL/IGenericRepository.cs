using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A91WEBAPI.DAL
{
    public interface IGenericRepository<T> where T : class
    {
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        void Save();
    }
}
