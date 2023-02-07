using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkClassLibrary.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        ITodoRepository todo { get; }
        Task Save();
        void Dispose();

    }
}
