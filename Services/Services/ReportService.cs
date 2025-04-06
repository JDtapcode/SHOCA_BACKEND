using AutoMapper;
using Microsoft.AspNetCore.Http;
using Repositories.Entities;
using Repositories.Enums;
using Repositories.Interfaces;
using Repositories.Models.ReportModels;
using Services.Interfaces;
using Services.Models.ReportModels;
using Services.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModel> CreateReportAsync(ReportCreateModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Description))
            {
                return new ResponseModel { Status = false, Message = "Invalid input data" };
            }

            var reporter = await _unitOfWork.AccountRepository.GetAsync(model.ReporterId);
            if (reporter == null)
            {
                return new ResponseModel { Status = false, Message = "Reporter not found" };
            }

            var artwork = await _unitOfWork.ArtworkRepository.GetAsync(model.ArtworkId);
            if (artwork == null)
            {
                return new ResponseModel { Status = false, Message = "Artwork not found" };
            }

            var report = _mapper.Map<Report>(model);
            report.Status = ReportStatus.Pending;
            await _unitOfWork.ReportRepository.AddAsync(report);
            await _unitOfWork.SaveChangeAsync();

            var createdReport = await _unitOfWork.ReportRepository.GetAsync(report.Id, includes: new[] { "Reporter", "Artwork" });
            var reportModel = _mapper.Map<ReportModel>(createdReport);

            return new ResponseModel
            {
                Status = true,
                Message = "Report created successfully",
                Data = reportModel
            };
        }

        public async Task<ResponseList<List<ReportModel>>> GetReportsByUserAsync(Guid userId)
        {
            var (reportList, totalCount) = await _unitOfWork.ReportRepository.GetReportsByUserAsync(userId);
            if (!reportList.Any())
            {
                return new ResponseList<List<ReportModel>>
                {
                    Status = false,
                    Message = "No reports found for this user",
                    Data = null
                };
            }

            var reportModels = _mapper.Map<List<ReportModel>>(reportList);

            return new ResponseList<List<ReportModel>>
            {
                Status = true,
                Message = "Reports retrieved successfully",
                Data = reportModels
            };
        }
        public async Task<ResponseList<List<ReportModel>>> GetAllReportsAsync(bool includeDeleted = false)
        {
            var (reportList, totalCount) = await _unitOfWork.ReportRepository.GetAllReportsAsync(includeDeleted);
            if (!reportList.Any())
            {
                return new ResponseList<List<ReportModel>>
                {
                    Status = false,
                    Message = "No reports found",
                    Data = null
                };
            }

            var reportModels = _mapper.Map<List<ReportModel>>(reportList);

            return new ResponseList<List<ReportModel>>
            {
                Status = true,
                Message = "Reports retrieved successfully",
                Data = reportModels
            };
        }
        public async Task<ResponseList<List<ReportModel>>> GetReportsByArtworkAsync(Guid artworkId)
        {
            var (reportList, totalCount) = await _unitOfWork.ReportRepository.GetReportsByArtworkAsync(artworkId);
            if (!reportList.Any())
            {
                return new ResponseList<List<ReportModel>>
                {
                    Status = false,
                    Message = "No reports found for this artwork",
                    Data = null
                };
            }

            var reportModels = _mapper.Map<List<ReportModel>>(reportList);

            return new ResponseList<List<ReportModel>>
            {
                Status = true,
                Message = "Reports retrieved successfully",
                Data = reportModels
            };
        }
        public async Task<ResponseModel> UpdateReportStatusAsync(Guid reportId, ReportStatusUpdateModel model)
        {
            var report = await _unitOfWork.ReportRepository.GetAsync(reportId, includes: new[] { "Reporter", "Artwork" });
            if (report == null)
            {
                return new ResponseModel { Status = false, Message = "Report not found" };
            }

            if (report.IsDeleted)
            {
                return new ResponseModel { Status = false, Message = "Cannot update status of a deleted report" };
            }

            report.Status = model.Status;
            _unitOfWork.ReportRepository.Update(report);
            await _unitOfWork.SaveChangeAsync();

            var updatedReport = await _unitOfWork.ReportRepository.GetAsync(reportId, includes: new[] { "Reporter", "Artwork" });
            var reportModel = _mapper.Map<ReportModel>(updatedReport);

            return new ResponseModel
            {
                Status = true,
                Message = "Report status updated successfully",
                Data = reportModel
            };
        }
    }
    }
