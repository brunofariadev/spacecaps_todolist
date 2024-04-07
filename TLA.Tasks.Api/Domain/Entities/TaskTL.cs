using System;
using TLA.Tasks.Api.Domain.Enums;
using TLA.WebApi.Core.DomainObjects;

namespace TLA.Tasks.Api.Domain.Entities
{
    public class TaskTL : Entity, IAggregateRoot
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public StatusEnum Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Guid UsuarioId { get; private set; }

        public TaskTL(string title, string description, Guid usuarioId)
        {
            Title = title;
            Description = description;
            Status = StatusEnum.Pendente;
            CreatedAt = DateTime.Now;
            UsuarioId = usuarioId;
        }

        protected TaskTL() { }

        public void UpdateTitle(string title)
        {
            Title = title;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }

        public void UpdateStatus(StatusEnum status)
        {
            Status = status;
        }
    }
}
