using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorPrestamos.Data.Interfaces
{
    public interface IRepository<T, I>
    {
        public List<T> GetAll();
        public T GetById(I id);
        public T Add(T newEntity);
        public T Update(T updatedEntity);
        public bool Delete(I id);
    }
}
