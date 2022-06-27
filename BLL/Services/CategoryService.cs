using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BAL;
using BAL.Entity;
using BAL.Interfaces;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Validation;

namespace BusinessLogicLayer.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        public CategoryService(IUnitOfWork unitOf, IMapper mapper)
        {
            unitOfWork = unitOf;
            _mapper = mapper;
        }

        public async Task AddAsync(CategoryModel model)
        {
            if(model is null)
                throw new FileExcpetion("Model can't be null");
            if (model.CategoryName == "")
                throw new FileExcpetion("CategoryName can't be null");
            await unitOfWork.CategoryRepository.Add(_mapper.Map<CategoryModel, Category>(model));
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await unitOfWork.CategoryRepository.DeleteByIdAsync(modelId);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<CategoryModel>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryModel>>(await unitOfWork.CategoryRepository.GetAll());
        }

        public async Task<CategoryModel> GetByIdAsync(int id)
        {
            return _mapper.Map<Category, CategoryModel>(await unitOfWork.CategoryRepository.GetById(id));
        }

        public async Task UpdateAsync(CategoryModel model)
        {
            if (model is null)
                throw new FileExcpetion("Model can't be null");
            if (model.CategoryName == "")
                throw new FileExcpetion("CategoryName can't be null");

            unitOfWork.CategoryRepository.Update(_mapper.Map<CategoryModel, Category>(model));
            await unitOfWork.SaveChangesAsync();
        }
    }
}
