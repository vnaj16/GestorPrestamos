using GestorPrestamos.Data.Interfaces;
using GestorPrestamos.Services.Interfaces;

namespace GestorPrestamos.Services
{
    public class MyService : IMyService
    {
        public readonly IRepository repository;

        public MyService(IRepository repository)
        {
            this.repository = repository;
        }

        public int GetData()
        {
            return repository.GetData();
        }
    }
}
