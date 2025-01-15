using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Interfaces;
using Services.Models.ResponseModels;
using Services.Models.FreelancerServiceModels;
using Services.Common;
using Repositories.Models.FreelancerServiceModels;
using AutoMapper;
using Repositories.Entities;

namespace Services.Services
{
    public class FreelancerServiceService : IFreelancerServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public FreelancerServiceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseModel> CreateFreelancerServiceAsync(FreelancerServiceCreateModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Description))
            {
                return new ResponseModel { Status = false, Message = "Invalid input data" };
            }
            var freelancerService = _mapper.Map<FreelancerService>(model);
            //var freelancerService = new FreelancerService
            //{
            //    Description = model.Description,
            //    Price = model.Price,
            //    ImageUrl = model.ImageUrl,
            //    DeliveryTime = model.DeliveryTime,
            //    NumConcepts = model.NumConcepts,
            //    NumRevisions = model.NumRevisions,
            //    UserId = model.UserId,
            //};
            await _unitOfWork.FreelancerServiceRepository.AddAsync(freelancerService);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "Freelancer Service created successfully" };
        }

        public async Task<ResponseModel> DeleteFreelancerServiceAsync(Guid id)
        {
            var service = await _unitOfWork.FreelancerServiceRepository.GetAsync(id);
            if (service == null) return new ResponseModel { Status = false, Message = "Freelancer Service not found" };

            _unitOfWork.FreelancerServiceRepository.SoftDelete(service);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "Freelancer Service deleted successfully" };
        }

        public async Task<Pagination<FreelancerServiceModel>> GetAllFreelancerServicesAsync(FreelancerServiceFilterModel filterModel)
        {
            var queryResult = await _unitOfWork.FreelancerServiceRepository.GetAllAsync(
                filter: s => (s.IsDeleted == filterModel.isDelete) &&
                             (filterModel.MinPrice == null || s.Price >= filterModel.MinPrice) &&
                             (filterModel.MaxPrice == null || s.Price <= filterModel.MaxPrice) &&
                             (filterModel.MinDeliveryTime == null || s.DeliveryTime >= filterModel.MinDeliveryTime) &&
                             (filterModel.MaxDeliveryTime == null || s.DeliveryTime <= filterModel.MaxDeliveryTime) &&
                             (filterModel.NumConcepts == null || s.NumConcepts == filterModel.NumConcepts) &&
                     (filterModel.NumRevisions == null || s.NumRevisions == filterModel.NumRevisions) &&
                     (filterModel.UserId == Guid.Empty || s.UserId == filterModel.UserId) ,
                //(string.IsNullOrEmpty(filterModel.) || s.Name.Contains(filterModel.Name)),
                include: "Category",
                pageIndex: filterModel.PageIndex,
                pageSize: filterModel.PageSize
            );

            var services = _mapper.Map<List<FreelancerServiceModel>>(queryResult.Data);
            return new Pagination<FreelancerServiceModel>(services, filterModel.PageIndex, filterModel.PageSize, queryResult.TotalCount);
        }

        public async Task<ResponseDataModel<FreelancerServiceModel>> GetFreelancerServiceByIdAsync(Guid id)
        {
            var service = await _unitOfWork.FreelancerServiceRepository.GetAsync(id, include: "Category");
            if (service == null || service.IsDeleted)
            {
                return new ResponseDataModel<FreelancerServiceModel> { Status = false, Message = "Freelancer Service not found" };
            }

            var serviceModel = _mapper.Map<FreelancerServiceModel>(service);
            return new ResponseDataModel<FreelancerServiceModel> { Status = true, Data = serviceModel };
        }

        public async Task<ResponseModel> RestoreFreelancerService(Guid id)
        {
            var freelancerService = await _unitOfWork.FreelancerServiceRepository.GetAsync(id);
            if (freelancerService == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Freelancer Service not found"
                };
            }

            if (!freelancerService.IsDeleted)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Freelancer Service is not deleted"
                };
            }

            freelancerService.IsDeleted = false;
            freelancerService.DeletionDate = null;
            freelancerService.DeletedBy = null;

            _unitOfWork.FreelancerServiceRepository.Update(freelancerService);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel
            {
                Status = true,
                Message = "Freelancer Service restored successfully"
            };
        }

        public async Task<ResponseModel> RestoreFreelancerServiceAsync(Guid id)
        {
            var service = await _unitOfWork.FreelancerServiceRepository.GetAsync(id);
            if (service == null)
            {
                return new ResponseModel { Status = false, Message = "Freelancer Service not found" };
            }

            if (!service.IsDeleted)
            {
                return new ResponseModel { Status = false, Message = "Freelancer Service is not deleted" };
            }

            service.IsDeleted = false;
            service.DeletionDate = null;
            service.DeletedBy = null;

            _unitOfWork.FreelancerServiceRepository.Update(service);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "Freelancer Service restored successfully" };
        }

        public async Task<ResponseModel> UpdateFreelancerServiceAsync(Guid id, FreelancerServiceUpdateModel model)
        {
            var service = await _unitOfWork.FreelancerServiceRepository.GetAsync(id);
            if (service == null)
                return new ResponseModel { Status = false, Message = "Freelancer Service not found" };

            _mapper.Map(model, service);
            _unitOfWork.FreelancerServiceRepository.Update(service);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel { Status = true, Message = "Freelancer Service updated successfully" };
        }

    }
}
