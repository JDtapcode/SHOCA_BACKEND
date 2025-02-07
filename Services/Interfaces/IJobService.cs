using Repositories.Models.JobModels;
using Services.Common;
using Services.Models.JobModels;
using Services.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IJobService
    {
        Task<Pagination<JobModel>> GetAllJobAsync(JobFilterModel filterModel);
        Task<ResponseModel> CreateJobAsync(JobCreateModel model);
        Task<ResponseModel> UpdateJobAsync(Guid id, JobUpdateModel model);
        Task<ResponseModel> DeleteJobAsync(Guid id);
        Task<ResponseDataModel<JobModel>> GetJobByIdAsync(Guid id);
        Task<ResponseModel> RestoreJob(Guid id);
    }
}
