﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.PayOSModels
{
    public class PayOSCallbackModel
    {
        public string OrderCode { get; set; }
        public string Status { get; set; }
    }
}
