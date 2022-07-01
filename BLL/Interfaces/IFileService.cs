using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BAL.Entity;
using BAL.Entity.Auth;
using BusinessLogicLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;

namespace BusinessLogicLayer.Interfaces
{
    public interface IFileService : ICrud<FileModel>
    {
        Task FilePatch(int fileid, JsonPatchDocument<Files> model);
        public Task UploadFile(int categoryId, User user, IFormFileCollection files);
    }
}
