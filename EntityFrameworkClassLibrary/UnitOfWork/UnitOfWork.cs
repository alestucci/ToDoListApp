using EntityFrameworkClassLibrary.Repository;

namespace EntityFrameworkClassLibrary.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ITodoRepository TodoRepository { get; set; }

        public UnitOfWork(ApplicationDbContext db)
        {
                _db = db;
                TodoRepository = new TodoRepository(_db);
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
