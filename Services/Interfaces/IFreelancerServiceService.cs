using Repositories.Models.FreelancerServiceModels;
using Services.Common;
using Services.Models.FreelancerServiceModels;
using Services.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IFreelancerServiceService
    {
        Task<Pagination<FreelancerServiceModel>> GetAllFreelancerServicesAsync(FreelancerServiceFilterModel filterModel);
        Task<ResponseModel> CreateFreelancerServiceAsync(FreelancerServiceCreateModel model);
        Task<ResponseModel> UpdateFreelancerServiceAsync(Guid id, FreelancerServiceUpdateModel model);
        Task<ResponseModel> DeleteFreelancerServiceAsync(Guid id);
        Task<ResponseDataModel<FreelancerServiceModel>> GetFreelancerServiceByIdAsync(Guid id);
        Task<ResponseModel> RestoreFreelancerService(Guid id);
    }
}
