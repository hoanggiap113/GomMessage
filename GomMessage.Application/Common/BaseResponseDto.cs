using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomMessage.Application.Common
{
    public class BaseResponseDto<T> where T : class
    {
        public int Page { get; set; }
        public int Total { get; set; }
        public int PageSize { get; set; }
        public int Limit { get; set; }
        public List<T> Data { get; set; }
        
        public BaseResponseDto() { }
        public BaseResponseDto(int page, int total, int pageSize, int limit, List<T> data)
        {
            this.Page = page;
            this.Total = total;
            this.PageSize = pageSize;
            this.Limit = limit;
            this.Data = data;
        }
    }
}
