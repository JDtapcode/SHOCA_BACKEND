using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Common
{
    public class PaginationResult<T>
    {
        public List<T> Data { get; set; }
        public int TotalCount { get; set; }

        public PaginationResult(List<T> data, int totalCount)
        {
            Data = data ?? new List<T>();
            TotalCount = totalCount;
        }
    }

}
