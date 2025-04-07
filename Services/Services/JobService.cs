using AutoMapper;
using Repositories.Entities;
using Repositories.Interfaces;
using Repositories.Models.JobModels;
using Services.Common;
using Services.Interfaces;
using Services.Models.JobModels;
using Services.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class JobService : IJobService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public JobService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModel> CreateJobAsync(JobCreateModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.ProjectTitle))
            {
                return new ResponseModel { Status = false, Message = "Tittle,Categories and Budget can't be null" };
            }

            var job = _mapper.Map<Job>(model);
            await _unitOfWork.JobRepository.AddAsync(job);
            await _unitOfWork.SaveChangeAsync();

            var jobModel = _mapper.Map<JobModel>(job); 
            return new ResponseModel
            {
                Status = true,
                Message = "Job created successfully",
                Data = jobModel
            };
        }

        public async Task<ResponseModel> DeleteJobAsync(Guid id)
        {
            var job = await _unitOfWork.JobRepository.GetAsync(id);
            if (job == null)
                return new ResponseModel { Status = false, Message = "Job not found" };

            _unitOfWork.JobRepository.SoftDelete(job);
            await _unitOfWork.SaveChangeAsync();

            var jobModel = _mapper.Map<JobModel>(job); 
            return new ResponseModel
            {
                Status = true,
                Message = "Job deleted successfully",
                Data = jobModel
            };
        }
        public async Task<Pagination<JobModel>> GetAllJobAsync(JobFilterModel filterModel)
        {
            var queryResult = await _unitOfWork.JobRepository.GetAllAsync(
                filter: j => (j.IsDeleted == filterModel.isDelete) &&
                             (filterModel.MinBudget == null || j.Budget >= filterModel.MinBudget) &&
                             (filterModel.MaxBudget == null || j.Budget <= filterModel.MaxBudget) &&
                             (string.IsNullOrEmpty(filterModel.Location) || j.Location.Contains(filterModel.Location)) &&
                             (string.IsNullOrEmpty(filterModel.Category) || j.Categories.Contains(filterModel.Category)) &&
                             (filterModel.UserId == Guid.Empty || j.UserId == filterModel.UserId),
                pageIndex: filterModel.PageIndex,
                pageSize: filterModel.PageSize
            );

            var jobs = _mapper.Map<List<JobModel>>(queryResult.Data);
            return new Pagination<JobModel>(jobs, filterModel.PageIndex, filterModel.PageSize, queryResult.TotalCount);
        }

        public async Task<ResponseDataModel<JobModel>> GetJobByIdAsync(Guid id)
        {
            var job = await _unitOfWork.JobRepository.GetAsync(id);
            if (job == null || job.IsDeleted)
            {
                return new ResponseDataModel<JobModel> { Status = false, Message = "Job not found" };
            }

            var jobModel = _mapper.Map<JobModel>(job);
            return new ResponseDataModel<JobModel> { Status = true, Data = jobModel };
        }

        public async Task<ResponseModel> RestoreJob(Guid id)
        {
            var job = await _unitOfWork.JobRepository.GetAsync(id);
            if (job == null)
            {
                return new ResponseModel { Status = false, Message = "Job not found" };
            }

            if (!job.IsDeleted)
            {
                return new ResponseModel { Status = false, Message = "Job is not deleted" };
            }

            job.IsDeleted = false;
            job.DeletionDate = null;
            job.DeletedBy = null;

            _unitOfWork.JobRepository.Update(job);
            await _unitOfWork.SaveChangeAsync();

            var jobModel = _mapper.Map<JobModel>(job); 
            return new ResponseModel
            {
                Status = true,
                Message = "Job restored successfully",
                Data = jobModel
            };
        }

        public async Task<ResponseModel> UpdateJobAsync(Guid id, JobUpdateModel model)
        {
            var job = await _unitOfWork.JobRepository.GetAsync(id);
            if (job == null)
                return new ResponseModel { Status = false, Message = "Job not found" };

            _mapper.Map(model, job);
            _unitOfWork.JobRepository.Update(job);
            await _unitOfWork.SaveChangeAsync();

            var jobModel = _mapper.Map<JobModel>(job); 
            return new ResponseModel
            {
                Status = true,
                Message = "Job updated successfully",
                Data = jobModel
            };
        }
    }
}
