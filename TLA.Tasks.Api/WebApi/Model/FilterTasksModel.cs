using TLA.Tasks.Api.Domain.Enums;
using TLA.Tasks.Api.Domain.Utils;

namespace TLA.Tasks.Api.WebApi.Model
{
    public class FilterTasksModel : PaginationModel
    {
        public StatusEnum? Status { get; set; }

        public FilterTasks ToFilterTasks()
        {
            return new FilterTasks()
            {
                Status = Status,
                UsuarioId = UsuarioId,
                Page = Page,
                PageSize = PageSize
            };
        }
    }
}
