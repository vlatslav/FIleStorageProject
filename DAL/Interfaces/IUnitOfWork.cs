using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface IUnitOfWork
    {
        public IFileRepository FileRepository { get; }

        public ICategoryRepository CategoryRepository { get; }

        public Task SaveChangesAsync();
    }
}
