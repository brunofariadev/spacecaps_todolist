using FluentValidation;
using System;
using TLA.WebApi.Core.Messages;

namespace TLA.Tasks.Api.Application.Commands
{
    public class AddTaskCommand : Command
    {
        public Guid UsuarioId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        
        public AddTaskCommand(Guid usuarioId, string title, string description)
        {
            UsuarioId = usuarioId;
            Title = title;
            Description = description;
        }

        public override bool IsValid()
        {
            ValidationResult = new TaskValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class TaskValidation : AbstractValidator<AddTaskCommand>
        {
            public TaskValidation()
            {
                RuleFor(c => c.UsuarioId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Usuário da tarefa não informado");

                RuleFor(c => c.Title)
                    .NotEmpty()
                    .WithMessage("Informe o título");
            }
        }
    }
}
