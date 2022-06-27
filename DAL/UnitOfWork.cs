using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BAL.Interfaces;
using BAL.Repositories;

namespace BAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private FileDBContext _context;
        private IFileRepository fileRepository;
        private ICategoryRepository categoryRepository;
        public UnitOfWork(FileDBContext context)
        {
            _context = context;
        }

        public IFileRepository FileRepository {
            get
            {
                if(fileRepository is null)
                    fileRepository = new FileRepository(_context);
                return fileRepository;
            }
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                if(categoryRepository is null)
                    categoryRepository = new CategoryRepository(_context);
                return categoryRepository;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
