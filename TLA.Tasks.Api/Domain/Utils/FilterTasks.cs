using TLA.Tasks.Api.Domain.Enums;

namespace TLA.Tasks.Api.Domain.Utils
{
    public class FilterTasks : Paginacao
    {
        public StatusEnum? Status { get; set; }

    }
}
