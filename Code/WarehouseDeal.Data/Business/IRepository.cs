using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDeal.Data.Business
{
    interface IRepository<T,TKey>  where T : class
    {
        IEnumerable<T> GetAll();
        T Get (TKey id);
        void AddNew (T item);
        void Update (T item);
        void Delete (TKey item);
    }
}
