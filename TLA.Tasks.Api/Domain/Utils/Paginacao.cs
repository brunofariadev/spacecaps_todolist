using System;

namespace TLA.Tasks.Api.Domain.Utils
{
    public class Paginacao
    {
        public Guid? UsuarioId { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}
