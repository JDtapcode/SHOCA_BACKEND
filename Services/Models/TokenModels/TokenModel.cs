using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.TokenModels
{
    public class TokenModel
    {
        public required string AccessToken { get; set; }
        public DateTime AccessTokenExpiryTime { get; set; }
        public string? RefreshToken { get; set; }
    }
}
