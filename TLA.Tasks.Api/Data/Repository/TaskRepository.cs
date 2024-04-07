using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TLA.Tasks.Api.Domain.Entities;
using TLA.Tasks.Api.Domain.Extension;
using TLA.Tasks.Api.Domain.Interface;
using TLA.Tasks.Api.Domain.Utils;
using TLA.WebApi.Core.Data.Interface;

namespace TLA.Tasks.Api.Data.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskContext _taskContext;
        public IUnitOfWork UnitOfWork => _taskContext;

        public TaskRepository(TaskContext taskContext)
        {
            _taskContext = taskContext;
        }

        public void Add(TaskTL task)
        {
            _taskContext.Add(task);
        }

        public void Dispose()
        {
            _taskContext.Dispose();
        }

        public void Alter(TaskTL task)
        {
            _taskContext.Update(task);
        }

        public async Task<TaskTL> Get(Guid id)
        {
            return await _taskContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<PagedList<TaskTL>> GetAll(FilterTasks filterTasks)
        {
            var queryTasks = _taskContext.Tasks
                .AsQueryable();

            if (filterTasks.UsuarioId.HasValue)
            {
                queryTasks = queryTasks.Where(t => t.UsuarioId == filterTasks.UsuarioId);
            }

            if (filterTasks.Status.HasValue)
            {
                queryTasks = queryTasks.Where(t => t.Status == filterTasks.Status.GetValueOrDefault());
            }

            return await queryTasks.ToPagedList(filterTasks.Page, filterTasks.PageSize);
        }
    }
}
