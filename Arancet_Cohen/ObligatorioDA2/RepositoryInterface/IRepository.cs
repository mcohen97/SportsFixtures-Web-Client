using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryInterface
{
    public interface IRepository<T,K>
    {
        bool IsEmpty();

        void Add(T entity);

        void Delete(K id);

        bool Exists(T record);

        void Clear();

        void Modify(T entity);

        T Get(K id);

        ICollection<T> GetAll();
    }
}
