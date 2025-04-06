using Repositories.Models.ProPackages;
using Services.Common;
using Services.Models.ProPackageModels;
using Services.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IProPackageService
    {
        Task<Pagination<ProPackageModel>> GetAllProPackageAsync(ProPackageFilterModel filterModel);
        Task<ResponseModel> CreateProPackageAsync(ProPackageCreateModel model);
        Task<ResponseModel> UpdateProPackageAsync(Guid id, ProPackageUpdateModel model);
        Task<ResponseModel> DeleteProPackageAsync(Guid id);
        Task<ResponseDataModel<ProPackageModel>> GetProPackageByIdAsync(Guid id);
        Task<ResponseModel> RestoreProPackage(Guid id);
    }
}
