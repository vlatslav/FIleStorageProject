using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BAL.Entity;
using System.Linq;
using BAL.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using BAL.Validation;

namespace BAL.Repositories
{
    public class FileRepository : IFileRepository
    {
        protected readonly FileDBContext _context;
        public FileRepository(FileDBContext context)
        {
            _context = context;
        }

        public async Task Add(Files model)
        {
            if (_context.Files.Any(x => x.FileId == model.FileId))
                throw new RepositoryExcpetion();
            await _context.Files.AddAsync(model);
        }


        public void Delete(Files model)
        {
            _context.Files.Remove(model);
        }

        public async Task DeleteByIdAsync(int modelid)
        {
            _context.Files.Remove(await _context.Files.FirstOrDefaultAsync(x =>x.FileId == modelid));
        }

        public void FilePatch(Files file, JsonPatchDocument<Files> model)
        {
            model.ApplyTo(file);
        }

        public async Task<IEnumerable<Files>> GetAll()
        {
            return await _context.Files.ToArrayAsync();
        }

        public async Task<IEnumerable<Files>> GetAllWithDetails()
        {
            return await _context.Files.Include(x => x.Category).Include(x => x.User).ToArrayAsync();
        }

        public async Task<Files> GetById(int id)
        {
            return await _context.Files.FirstOrDefaultAsync(x => x.FileId == id);
        }

        public void Update(Files model)
        {
            _context.Files.Update(model);
        }
    }
}
