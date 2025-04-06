using Microsoft.AspNetCore.Http;
using Repositories.Models.ReportModels;
using Services.Models.ReportModels;
using Services.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IReportService
    {
        Task<ResponseModel> CreateReportAsync(ReportCreateModel model);
        Task<ResponseList<List<ReportModel>>> GetReportsByUserAsync(Guid userId);
        Task<ResponseList<List<ReportModel>>> GetReportsByArtworkAsync(Guid artworkId);
        Task<ResponseModel> UpdateReportStatusAsync(Guid reportId, ReportStatusUpdateModel model);
        Task<ResponseList<List<ReportModel>>> GetAllReportsAsync(bool includeDeleted = false);
    }
}
