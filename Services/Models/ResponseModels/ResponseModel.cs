using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.ResponseModels
{
    public class ResponseModel
    {
        public bool Status { get; set; } = false;
        public string Message { get; set; } = "";

        public object? Data { get; set; }
        // Optional
        public bool? EmailVerificationRequired { get; set; }
        public bool? IsBlocked { get; set; }
        public List<string>? Errors { get; internal set; }
    }
}
