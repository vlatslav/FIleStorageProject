using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public Task Add(TEntity model);
        public Task<IEnumerable<TEntity>> GetAll();

        public Task<TEntity> GetById(int id);

        public void Delete(TEntity model);
        public Task DeleteByIdAsync(int modelid);

        public void Update(TEntity model);
    }
}
