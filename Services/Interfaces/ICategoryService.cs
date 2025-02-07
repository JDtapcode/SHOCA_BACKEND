using Repositories.Models.CategoryModels;
using Services.Common;
using Services.Models.CategoryModels;
using Services.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Pagination<CategoryModel>> GetAllCategoryAsync(CategoryFilterModel filterModel);
        Task<ResponseModel> CreateCategoryAsync(CategoryCreateModel model);
        Task<ResponseModel> UpdateCategoryAsync(Guid id, CategoryUpdateModel model);
        Task<ResponseModel> DeleteCategoryAsync(Guid id);
        Task<ResponseDataModel<CategoryModel>> GetCategoryByIdAsync(Guid id);
        Task<ResponseModel> RestoreCategory(Guid id);
    }
}
