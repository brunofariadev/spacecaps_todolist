using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;

namespace TLA.Tasks.Api.WebApi.Model
{
    public class PaginationModel
    {
        [BindNever]
        public Guid UsuarioId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int Page { get; set; } = 1;

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int PageSize { get; set; } = 10;
    }
}
