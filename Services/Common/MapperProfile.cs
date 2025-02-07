using AutoMapper;
using Repositories.Entities;
using Repositories.Models.AccountModels;
using Repositories.Models.CategoryModels;
using Repositories.Models.FreelancerServiceModels;
using Repositories.Models.JobModels;
using Services.Models.AccountModels;
using Services.Models.CategoryModels;
using Services.Models.FreelancerServiceModels;
using Services.Models.JobModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Common
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //Account
            CreateMap<AccountRegisterModel, Account>();
            CreateMap<AccountModel, Account>().ReverseMap();

            //Freelancer
            CreateMap<FreelancerService, FreelancerServiceModel>()
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl ?? string.Empty))
            .ForMember(dest => dest.DeliveryTime, opt => opt.MapFrom(src => src.DeliveryTime))
            .ForMember(dest => dest.NumConcepts, opt => opt.MapFrom(src => src.NumConcepts))
            .ForMember(dest => dest.NumRevisions, opt => opt.MapFrom(src => src.NumRevisions))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));
            CreateMap<FreelancerService, FreelancerServiceCreateModel>().ReverseMap();
            CreateMap<FreelancerService, FreelancerServiceUpdateModel>().ReverseMap();

            // Job
            CreateMap<Job, JobModel>()
                .ForMember(dest => dest.ProjectTitle, opt => opt.MapFrom(src => src.ProjectTitle))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories))
                .ForMember(dest => dest.Budget, opt => opt.MapFrom(src => src.Budget))
                .ForMember(dest => dest.TimeFrame, opt => opt.MapFrom(src => src.TimeFrame))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.FileAttachment, opt => opt.MapFrom(src => src.FileAttachment ?? string.Empty))
                .ForMember(dest => dest.PersonalInformation, opt => opt.MapFrom(src => src.PersonalInformation ?? string.Empty))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));

            CreateMap<Job, JobCreateModel>().ReverseMap();
            CreateMap<Job, JobUpdateModel>().ReverseMap();
            // Category
            CreateMap<Category, CategoryModel>().ReverseMap();
            CreateMap<Category, CategoryCreateModel>().ReverseMap();
            CreateMap<Category, CategoryUpdateModel>().ReverseMap();


        }
    }
}
