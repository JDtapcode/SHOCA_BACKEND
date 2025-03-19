using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Repositories.Entities;
using Repositories.Models.AccountModels;
using Repositories.Models.ArtworkImageModels;
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
            CreateMap<Account, AccountModel>()
            .ForMember(dest => dest.PurchasedPackages, opt => opt.Ignore());
            CreateMap<AccountModel, Account>().ReverseMap();

            //Freelancer
            CreateMap<FreelancerService, FreelancerServiceModel>()
                .ForMember(dest => dest.Servicename, opt => opt.MapFrom(src => src.Servicename))
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
            //Artwork
            //CreateMap<Artwork, ArtworkModel>()
            //.ForMember(dest => dest.Categories, opt => opt.MapFrom(src =>
            //    src.ArtworkCategories.Select(ac => ac.Category.Name).ToList())) 
            //.ForMember(dest => dest.Images, opt => opt.MapFrom(src =>
            //    src.Images.Select(img => img.FileUrl).ToList())) 
            //.ReverseMap();
            CreateMap<Artwork, ArtworkModel>()
    .ForMember(dest => dest.Categories, opt => opt.MapFrom(src =>
        src.ArtworkCategories != null
        ? src.ArtworkCategories.Select(ac => ac.Category != null ? ac.Category.Name : "Unknown").ToList()
        : new List<string>()
    ))
    .ForMember(dest => dest.Images, opt => opt.MapFrom(src =>
        src.Images != null
        ? src.Images.Select(img => img.FileUrl).ToList()
        : new List<string>() // ✅ Tránh null
    ))
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
            CreateMap<ArtworkImage, ArtworkImageModel>();

           //Propackage
            CreateMap<ProPackage, ProPackageModel>()
      .ForMember(dest => dest.Features, opt => opt.MapFrom(src =>
          src.Features != null ? src.Features.Select(f => f.Name).ToList() : new List<string>()
      ))
      .ReverseMap()
      .ForMember(dest => dest.Features, opt => opt.Ignore()); 

            CreateMap<ProPackageCreateModel, ProPackage>()
                .ForMember(dest => dest.Features, opt => opt.Ignore()); 

            CreateMap<ProPackageUpdateModel, ProPackage>()
                .ForMember(dest => dest.Features, opt => opt.Ignore());



            CreateMap<Portfolio, PortfolioModel>()
                        .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src =>
                            src.PortfolioImages.Select(pi => pi.ArtworkImage.FileUrl).ToList()))
                        .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                        .ReverseMap();

            CreateMap<Account, UserDto>();

            CreateMap<PortfolioImage, PortfolioImageModel>().ReverseMap();

           
            CreateMap<PortfolioCreateModel, Portfolio>()
    .ForMember(dest => dest.PortfolioImages, opt => opt.MapFrom(src =>
        src.ArtworkImageIds.Select(id => new PortfolioImage
        {
            ArtworkImageId = id
        }).ToList()
    ));
            CreateMap<PortfolioUpdateModel, Portfolio>()
    .ForMember(dest => dest.PortfolioImages, opt => opt.MapFrom(src =>
        src.ArtworkImageIds != null
            ? src.ArtworkImageIds.Select(id => new PortfolioImage
            {
                ArtworkImageId = id
            }).ToList()
            : new List<PortfolioImage>()
    ));



        }
    }
}
