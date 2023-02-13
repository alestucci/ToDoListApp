using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameworkClassLibrary.Repository;

namespace EntityFrameworkClassLibrary.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ITodoRepository TodoRepository { get; }
        Task Save();
        void Dispose();

    }
}
