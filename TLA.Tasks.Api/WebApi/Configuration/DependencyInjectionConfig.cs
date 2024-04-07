using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TLA.Tasks.Api.Application.Commands;
using TLA.Tasks.Api.Application.Queries;
using TLA.Tasks.Api.Data;
using TLA.Tasks.Api.Data.Repository;
using TLA.Tasks.Api.Domain.Interface;
using TLA.WebApi.Core.Identity;

namespace TLA.Tasks.Api.WebApi.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            // Queries
            services.AddScoped<ITaskQuerie, TaskQuerie>();

            // Commands
            services.AddScoped<IRequestHandler<AddTaskCommand, ValidationResult>, TaskCommandHandler>();
            services.AddScoped<IRequestHandler<AlterTaskCommand, ValidationResult>, TaskCommandHandler>();

            // Data
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<TaskContext>();
        }
    }
}
