using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TLA.Tasks.Api.Application.Commands;
using TLA.Tasks.Api.Application.Queries;
using TLA.Tasks.Api.WebApi.Model;
using TLA.WebApi.Core.Controllers;
using TLA.WebApi.Core.Enums;
using TLA.WebApi.Core.Identity;

namespace TLA.Tasks.Api.WebApi.Controllers
{
    [Route("api/task")]
    [Authorize]
    public class TaskController : MainController
    {
        private readonly IAspNetUser _user;
        private readonly IMediator _mediator;
        private readonly ITaskQuerie _taskQuerie;

        public TaskController(IAspNetUser user, IMediator mediator, ITaskQuerie taskQuerie)
        {
            _user = user;
            _mediator = mediator;
            _taskQuerie = taskQuerie;
        }

        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] TaskRegisterModel taskRegisterModel)
        {
            var usuarioId = _user.ObterUserId();
            return CustomResponse(await _mediator.Send(
                new AddTaskCommand(usuarioId, taskRegisterModel.Title, taskRegisterModel.Description)));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(Guid id)
        {
            var task = await _taskQuerie.Get(id);

            if (task == null)
            {
                return NotFound();
            }

            if (task.UserId != _user.ObterUserId() && !_user.PossuiRole(nameof(RoleEnum.Admin)))
            {
                return Unauthorized();
            }

            return CustomResponse(task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AltertTask([FromBody] TaskEditionModel taskEditionModel)
        {
            return CustomResponse(await _mediator.Send(
                new AlterTaskCommand(taskEditionModel.Id, taskEditionModel.Title, taskEditionModel.Description, taskEditionModel.Status)));
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetAll([FromQuery] FilterTasksModel filterTasksModel)
        {
            var usuarioId = _user.ObterUserId();
            filterTasksModel.UsuarioId = usuarioId;
            var task = await _taskQuerie.GetAll(filterTasksModel);
            return task == null ? NotFound() : CustomResponse(task);
        }
    }
}
