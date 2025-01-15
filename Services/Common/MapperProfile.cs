using AutoMapper;
using Repositories.Entities;
using Repositories.Models.AccountModels;
using Repositories.Models.FreelancerServiceModels;
using Services.Models.AccountModels;
using Services.Models.FreelancerServiceModels;
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
        }
        }
}
