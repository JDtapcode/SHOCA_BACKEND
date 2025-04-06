using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models.CategoryModels
{
    public class CategoryModel:BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
