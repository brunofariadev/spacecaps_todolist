using System;
using System.Text.Json.Serialization;
using TLA.Tasks.Api.Domain.Entities;
using TLA.Tasks.Api.Domain.Enums;

namespace TLA.Tasks.Api.WebApi.Model
{
    public class TaskEditionModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public StatusEnum Status { get; set; }
        [JsonIgnore]
        public Guid UserId { get; set; }

        public static TaskEditionModel ToTaskEditionModel(TaskTL task)
        {
            return new TaskEditionModel()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                UserId = task.UsuarioId
            };
        }
    }
}
