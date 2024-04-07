using System;
using TLA.Tasks.Api.Domain.Entities;
using TLA.Tasks.Api.Domain.Enums;

namespace TLA.Tasks.Api.WebApi.Model
{
    public class TaskListModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public StatusEnum Status { get; set; }

        public static TaskListModel ToTaskListModel(TaskTL task)
        {
            return new TaskListModel()
            {
                Id = task.Id,
                Title = task.Title,
                Status = task.Status
            };
        }
    }
}
