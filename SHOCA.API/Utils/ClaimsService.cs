using Repositories.Interfaces;
using Repositories.Utils;
using Services.Common;
using System.Security.Claims;

namespace SHOCA.API.Utils
{
    public class ClaimsService : IClaimsService
    {
        public Guid? GetCurrentUserId { get; }

        public ClaimsService(IHttpContextAccessor httpContextAccessor)
        {
            var identity = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            GetCurrentUserId = AuthenticationTools.GetCurrentUserId(identity!);
        }
    }
}
