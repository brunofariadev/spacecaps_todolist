using System;
using System.Threading.Tasks;
using TLA.Tasks.Api.Domain.Entities;
using TLA.Tasks.Api.Domain.Utils;
using TLA.WebApi.Core.Data.Interface;

namespace TLA.Tasks.Api.Domain.Interface
{
    public interface ITaskRepository : IRepository<TaskTL>
    {
        void Add(TaskTL task);
        void Alter(TaskTL task);

        Task<TaskTL> Get(Guid id);
        Task<PagedList<TaskTL>> GetAll(FilterTasks filterTasks);
    }
}
