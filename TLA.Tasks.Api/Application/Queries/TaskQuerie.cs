using System;
using System.Linq;
using System.Threading.Tasks;
using TLA.Tasks.Api.Domain.Interface;
using TLA.Tasks.Api.Domain.Utils;
using TLA.Tasks.Api.WebApi.Model;

namespace TLA.Tasks.Api.Application.Queries
{
    public interface ITaskQuerie
    {
        Task<TaskEditionModel> Get(Guid id);
        Task<PagedList<TaskListModel>> GetAll(FilterTasksModel filterTasksModel);
    }

    public class TaskQuerie : ITaskQuerie
    {
        private readonly ITaskRepository _taskRepository;

        public TaskQuerie(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskEditionModel> Get(Guid id)
        {
            var task = await _taskRepository.Get(id);
            return TaskEditionModel.ToTaskEditionModel(task);
        }

        public async Task<PagedList<TaskListModel>> GetAll(FilterTasksModel filterTasksModel)
        {
            var pagedTasks = await _taskRepository.GetAll(filterTasksModel.ToFilterTasks());
            return new PagedList<TaskListModel>(pagedTasks.Items.Select(t => TaskListModel.ToTaskListModel(t)).ToList(), 
                pagedTasks.TotalItems, pagedTasks.PageSize, pagedTasks.PageCount);
        }
    }
}
