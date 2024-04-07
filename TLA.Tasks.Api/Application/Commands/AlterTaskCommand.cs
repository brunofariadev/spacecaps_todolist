using FluentValidation;
using System;
using TLA.Tasks.Api.Domain.Enums;
using TLA.WebApi.Core.Messages;

namespace TLA.Tasks.Api.Application.Commands
{
    public class AlterTaskCommand : Command
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public StatusEnum Status { get; private set; }


        public AlterTaskCommand(Guid id, string title, string description, StatusEnum status)
        {
            Id = id;
            Title = title;
            Description = description;
            Status = status;
        }

        public override bool IsValid()
        {
            ValidationResult = new TaskValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class TaskValidation : AbstractValidator<AlterTaskCommand>
        {
            public TaskValidation()
            {
                RuleFor(c => c.Id)
                   .NotEqual(Guid.Empty)
                   .WithMessage("Id não informado");

                RuleFor(c => c.Title)
                    .NotEmpty()
                    .WithMessage("Informe o título");

                RuleFor(c => c.Description)
                    .NotEmpty()
                    .WithMessage("Informe a descrição");

                RuleFor(c => c.Status)
                    .NotNull()
                    .WithMessage("Informe o status");
            }
        }
    }
}
