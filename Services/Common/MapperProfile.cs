﻿using AutoMapper;
using Repositories.Entities;
using Repositories.Models.AccountModels;
using Repositories.Models.ArtworkModels;
using Repositories.Models.CategoryModels;
using Repositories.Models.FreelancerServiceModels;
using Repositories.Models.JobModels;
using Repositories.Models.PortfolioModels;
using Repositories.Models.ProPackages;
using Repositories.Models.RatingModels;
using Services.Models.AccountModels;
using Services.Models.ArtworkModels;
using Services.Models.CategoryModels;
using Services.Models.FreelancerServiceModels;
using Services.Models.JobModels;
using Services.Models.PortfolioModels;
using Services.Models.ProPackageModels;
using Services.Models.RatingModels;
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
            // Mapping từ Artwork sang ArtworkModel
            CreateMap<Artwork, ArtworkModel>()
    .ForMember(dest => dest.Categories, opt => opt.MapFrom(src =>
        src.ArtworkCategories.Select(ac => ac.Category).ToList()))
    .ReverseMap();
            CreateMap<Artwork, ArtworkCreateModel>().ReverseMap();
            CreateMap<Artwork, ArtworkUpdateModel>().ReverseMap();

            //Rating
            CreateMap<Rating, RatingModel>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.FirstName + " " + src.Customer.LastName : string.Empty))
                .ForMember(dest => dest.ArtworkTitle, opt => opt.MapFrom(src => src.Artwork != null ? src.Artwork.Title : string.Empty))
                .ForMember(dest => dest.CommentsList, opt => opt.MapFrom(src => src.CommentsList ?? new List<RatingComment>()));
            CreateMap<RatingCommentCreateModel, RatingComment>().ReverseMap();
            CreateMap<RatingComment, RatingCommentModel>()
                .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account != null ? src.Account.FirstName + " " + src.Account.LastName : string.Empty))
                .ForMember(dest => dest.ChildComments, opt => opt.MapFrom(src => src.ChildComments))
                .ReverseMap();

            //ProPackage
            CreateMap<ProPackage, ProPackageModel>().ReverseMap();
            CreateMap<ProPackageCreateModel, ProPackage>();
            CreateMap<ProPackageUpdateModel, ProPackage>();

            //Portfolio
            CreateMap<Portfolio, PortfolioModel>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom(src => src.CoverImageUrl))
            .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.Skills))
            .ForMember(dest => dest.Experience, opt => opt.MapFrom(src => src.Experience))
            .ForMember(dest => dest.ContactUrl, opt => opt.MapFrom(src => src.ContactUrl))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));

            CreateMap<Portfolio, PortfolioCreateModel>().ReverseMap();
            CreateMap<Portfolio, PortfolioUpdateModel>().ReverseMap();

        }
    }
}
