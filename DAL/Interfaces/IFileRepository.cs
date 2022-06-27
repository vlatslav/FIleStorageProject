using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BAL.Entity;
using Microsoft.AspNetCore.JsonPatch;

namespace BAL.Interfaces
{
    public interface IFileRepository : IRepository<Files>
    {
        Task<IEnumerable<Files>> GetAllWithDetails();
        void FilePatch(Files file, JsonPatchDocument<Files> model);
    }
}
