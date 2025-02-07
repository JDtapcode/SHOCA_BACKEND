using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.JobModels
{
    public class JobUpdateModel
    {
        public string ProjectTitle { get; set; }
        public string Categories { get; set; }
        public decimal Budget { get; set; }
        public string TimeFrame { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public string? FileAttachment { get; set; }
        public string? PersonalInformation { get; set; }
    }
}
