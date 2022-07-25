using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BAL;
using BAL.Entity;
using BAL.Entity.Auth;
using BAL.Interfaces;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Validation;
using DAL.Entity;
using DAL.Extentions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.StaticFiles;

namespace BusinessLogicLayer.Services
{
    public class FileService : IFileService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        public FileService(IUnitOfWork unitOf,IMapper mapper, UserManager<User> userManager, IHostingEnvironment hosting )
        {
            unitOfWork = unitOf;
            _mapper = mapper;
            _userManager = userManager;
            _hostingEnvironment = hosting;
        }

        public async Task AddAsync(FileModel model)
        {
            if(model is null)
                throw new FileExcpetion("Model can't be null");
            if (model.FilePath == "")
                throw new FileExcpetion("File path can't be null");
            if (model.ContentType == "")
                throw new FileExcpetion("ContentType can't be null");
            if (model.FileName == "")
                throw new FileExcpetion("FileName can't be null");
            var map = _mapper.Map<FileModel, Files>(model);
            await unitOfWork.FileRepository.Add(map);
            await unitOfWork.SaveChangesAsync();
        }
        
        public async Task DeleteAsync(int modelId)
        {
            await unitOfWork.FileRepository.DeleteByIdAsync(modelId);
            await unitOfWork.SaveChangesAsync();

        }

        public async Task FilePatch(int fileid, JsonPatchDocument<Files> model)
        {
            var file = await unitOfWork.FileRepository.GetById(fileid);
            var basefile = _mapper.Map<Files>(file);
            unitOfWork.FileRepository.FilePatch(basefile, model);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<FileModel>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<Files>, IEnumerable<FileModel>>(await unitOfWork.FileRepository.GetAllWithDetails());
        }

        public async Task<FileModel> GetByIdAsync(int id)
        {
            return _mapper.Map<Files, FileModel>(await unitOfWork.FileRepository.GetById(id));
        }
        public async Task UploadFile(int categoryId, User user, IFormFileCollection files)
        {
            if (files != null && files.Count > 0 || user is null)
            {
                var file = files.First();
                FileInfo fl = new FileInfo(file.FileName);
                var newfilename = "File_" + DateTime.Now.TimeOfDay.Milliseconds + fl.Extension;
                var path = Path.Combine("", _hostingEnvironment.ContentRootPath + "\\Files\\" + newfilename);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var f = new FileModel()
                {
                    Date = DateTime.Now,
                    FilePath = path,
                    ContentType = file.ContentType,
                    FileName = newfilename,
                    UserId = user.Id,
                    CategoryId = categoryId
                };
                await AddAsync(f);
            }
            else
            {
                throw new FileExcpetion("Arguments can't be null");
            }
        }
        public async Task UpdateAsync(FileModel model)
        {

            if(model is null)
                throw new FileExcpetion("Model can't be null");
            if (model.FilePath == "")
                throw new FileExcpetion("File path can't be null");
            if (model.ContentType == "")
                throw new FileExcpetion("ContentType can't be null");
            if (model.FileName == "")
                throw new FileExcpetion("FileName can't be null");
            unitOfWork.FileRepository.Update(_mapper.Map<FileModel, Files>(model));
            await unitOfWork.SaveChangesAsync();
        }

        public void DownloadFile(FileModel model)
        {
            if (model is null)
                throw new FileExcpetion("File can't be null");
            if (!File.Exists(model.FilePath))
                throw new FileExcpetion("File path doesn't exist");
        }

        public async Task<PageList<FileModel>> GetAllFiles(FileParameters fileParameters)
        {
            var files = (await GetAllAsync()).ToList();
            return PageList<FileModel>.ToPagedList(files, fileParameters.PageNumber, fileParameters.PageSize);
        }
    }

       
}
