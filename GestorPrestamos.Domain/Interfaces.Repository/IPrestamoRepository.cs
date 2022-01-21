using GestorPrestamos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorPrestamos.Domain.Interfaces.Repository
{
    public interface IPrestamoRepository : IRepository<Prestamo, string>
    {
        public List<Prestamo> GetAllWithDeudorEntityIncluded();
        public Prestamo GetByIdWithDeudorEntityIncluded(string id);
    }
}
