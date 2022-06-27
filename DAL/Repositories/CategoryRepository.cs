using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.Entity;
using BAL.Interfaces;
using BAL.Validation;
using Microsoft.EntityFrameworkCore;

namespace BAL.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private protected FileDBContext _context;
        public CategoryRepository(FileDBContext context)
        {
            _context = context;
        }

        public async Task Add(Category model)
        {
            if (_context.Categories.Any(x => x.CategoryId == model.CategoryId))
                throw new RepositoryExcpetion();
            await _context.Categories.AddAsync(model);
        }

        public void Delete(Category model)
        {
            _context.Categories.Remove(model);
        }
        public async Task DeleteByIdAsync(int modelid)
        {
            _context.Categories.Remove(await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == modelid));
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _context.Categories.ToArrayAsync();
        }


        public async Task<Category> GetById(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
        }

        public void Update(Category model)
        {
            _context.Categories.Update(model);
        }
    }
}
