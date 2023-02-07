using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameworkClassLibrary.Repository;

namespace EntityFrameworkClassLibrary.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ITodoRepository todo { get; set; }

        public UnitOfWork(ApplicationDbContext db)
        {
                _db = db;
                todo = new TodoRepository(_db);
        }

        public async void Dispose()
        {
            await _db.DisposeAsync();
        }

        public Task Save()
        {
            return _db.SaveChangesAsync();
        }
    }
}
