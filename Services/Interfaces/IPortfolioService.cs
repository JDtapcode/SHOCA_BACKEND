using Repositories.Models.PortfolioModels;
using Services.Common;
using Services.Models.PortfolioModels;
using Services.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IPortfolioService
    {
        Task<Pagination<PortfolioModel>> GetAllPortfolioAsync(PortfolioFilterModel filterModel);
        Task<ResponseModel> CreatePortfolioAsync(PortfolioCreateModel model);
        Task<ResponseModel> UpdatePortfolioAsync(Guid id, PortfolioUpdateModel model);
        Task<ResponseModel> DeletePortfolioAsync(Guid id);
        Task<ResponseDataModel<PortfolioModel>> GetPortfolioByIdAsync(Guid id);
        Task<ResponseModel> RestorePortfolio(Guid id);
    }
}
