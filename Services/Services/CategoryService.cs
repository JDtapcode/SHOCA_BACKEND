using AutoMapper;
using Repositories.Entities;
using Repositories.Interfaces;
using Repositories.Models.CategoryModels;
using Services.Common;
using Services.Interfaces;
using Services.Models.CategoryModels;
using Services.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Pagination<CategoryModel>> GetAllCategoryAsync(CategoryFilterModel filterModel)
        {
            var queryResult = await _unitOfWork.CategoryRepository.GetAllAsync(
                filter: c => (c.IsDeleted == filterModel.isDelete) &&
                             (string.IsNullOrEmpty(filterModel.Name) || c.Name.Contains(filterModel.Name)),
                pageIndex: filterModel.PageIndex,
                pageSize: filterModel.PageSize
            );

            var categories = _mapper.Map<List<CategoryModel>>(queryResult.Data);
            return new Pagination<CategoryModel>(categories, filterModel.PageIndex, filterModel.PageSize, queryResult.TotalCount);
        }

        public async Task<ResponseModel> CreateCategoryAsync(CategoryCreateModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name))
            {
                return new ResponseModel { Status = false, Message = "Invalid input data" };
            }

            var category = _mapper.Map<Category>(model);
            await _unitOfWork.CategoryRepository.AddAsync(category);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "Category created successfully" };
        }

        public async Task<ResponseModel> UpdateCategoryAsync(Guid id, CategoryUpdateModel model)
        {
            var category = await _unitOfWork.CategoryRepository.GetAsync(id);
            if (category == null)
                return new ResponseModel { Status = false, Message = "Category not found" };

            _mapper.Map(model, category);
            _unitOfWork.CategoryRepository.Update(category);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "Category updated successfully" };
        }

        public async Task<ResponseModel> DeleteCategoryAsync(Guid id)
        {
            var category = await _unitOfWork.CategoryRepository.GetAsync(id);
            if (category == null) return new ResponseModel { Status = false, Message = "Category not found" };

            _unitOfWork.CategoryRepository.SoftDelete(category);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "Category deleted successfully" };
        }

        public async Task<ResponseDataModel<CategoryModel>> GetCategoryByIdAsync(Guid id)
        {
            var category = await _unitOfWork.CategoryRepository.GetAsync(id);
            if (category == null || category.IsDeleted)
            {
                return new ResponseDataModel<CategoryModel> { Status = false, Message = "Category not found" };
            }

            var categoryModel = _mapper.Map<CategoryModel>(category);
            return new ResponseDataModel<CategoryModel> { Status = true, Data = categoryModel };
        }

        public async Task<ResponseModel> RestoreCategory(Guid id)
        {
            var category = await _unitOfWork.CategoryRepository.GetAsync(id);
            if (category == null)
            {
                return new ResponseModel { Status = false, Message = "Category not found" };
            }

            if (!category.IsDeleted)
            {
                return new ResponseModel { Status = false, Message = "Category is not deleted" };
            }

            category.IsDeleted = false;
            category.DeletionDate = null;
            category.DeletedBy = null;

            _unitOfWork.CategoryRepository.Update(category);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "Category restored successfully" };
        }
    }
}
