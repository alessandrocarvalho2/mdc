using Microsoft.AspNetCore.Http;
using Volvo.Ecash.Domain.Entities;

namespace Volvo.Ecash.Domain.Filters
{
    public class PaginationFilter
    {
        public PaginationFilter()
        {
            ResultPagesModel = new ResultPagesModel
            {
                Offset = 10,
                Actual = 0
            };
        }

        public ResultPagesModel ResultPagesModel { get; set; }

        public void ProcessQueryString(IQueryCollection filter)
        {
            //var result = new PaginationFilter();
            ResultPagesModel ??= new ResultPagesModel();

            foreach (var item in filter)
            {
                switch (item.Key.ToLower())
                {
                    case "actual":
                        ResultPagesModel.Actual = int.Parse(item.Value);
                        break;
                    case "offset":
                        ResultPagesModel.Offset = int.Parse(item.Value);
                        break;
                }
            }

        }
    }
}
