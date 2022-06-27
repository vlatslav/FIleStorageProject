using System;
using System.Collections.Generic;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;

namespace BusinessLogicLayer.Services
{
    public class FileService : IFileService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public FileService(IUnitOfWork unitOf,IMapper mapper, UserManager<User> userManager)
        {
            unitOfWork = unitOf;
            _mapper = mapper;
            _userManager = userManager;
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
    }

       
}
