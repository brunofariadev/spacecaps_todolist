using FluentValidation.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TLA.Tasks.Api.Domain.Entities;
using TLA.Tasks.Api.Domain.Interface;
using TLA.WebApi.Core.Enums;
using TLA.WebApi.Core.Identity;
using TLA.WebApi.Core.Messages;

namespace TLA.Tasks.Api.Application.Commands
{
    public class TaskCommandHandler : CommandHandler,
        IRequestHandler<AddTaskCommand, ValidationResult>,
        IRequestHandler<AlterTaskCommand, ValidationResult>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IAspNetUser _userContext;

        public TaskCommandHandler(ITaskRepository taskRepository, IAspNetUser userContext)
        {
            _taskRepository = taskRepository;
            _userContext = userContext;
        }

        public async Task<ValidationResult> Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;

            var task = new TaskTL(request.Title, request.Description, request.UsuarioId);
            _taskRepository.Add(task);
            return await PersistirDados(_taskRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(AlterTaskCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;

            var task = await _taskRepository.Get(request.Id);

            if (task is null)
            {
                AddError("A tarefa editada não foi encontrada no sistema.");
                return ValidationResult;
            }

            if (task.UsuarioId != _userContext.ObterUserId() && !_userContext.PossuiRole(nameof(RoleEnum.Admin)))
            {
                AddError("O usuário não tem permissão para alterar a tarefa.");
                return ValidationResult;
            }

            task.UpdateTitle(request.Title);
            task.UpdateDescription(request.Description);
            task.UpdateStatus(request.Status);

            _taskRepository.Alter(task);
            return await PersistirDados(_taskRepository.UnitOfWork);
        }
    }
}
