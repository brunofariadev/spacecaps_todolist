using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using TLA.Tasks.Api.Application.Queries;
using TLA.Tasks.Api.Domain.Enums;
using TLA.Tasks.Api.WebApi.Controllers;
using TLA.Tasks.Api.WebApi.Model;
using TLA.WebApi.Core.Enums;
using TLA.WebApi.Core.Identity;
using Xunit;

namespace TLA.Tasks.Tests
{
    public class GetTaskControllerTest
    {
        private readonly Mock<IAspNetUser> _mockUserContext;
        private readonly Mock<IMediator> _mockMediator;
        private readonly Mock<ITaskQuerie> _mockTaskQuery;
        
        public GetTaskControllerTest()
        {
            _mockUserContext = new Mock<IAspNetUser>();
            _mockMediator = new Mock<IMediator>();
            _mockTaskQuery = new Mock<ITaskQuerie>();
        }

        [Fact]
        public async Task DeveObterUmaTarefaComSucesso()
        {
            // Arrange
            var taskId = Guid.Parse("05fb4f22-9549-462a-895f-3d4fca4455ae");
            var userId = Guid.Parse("1cc3474b-d24c-472d-843a-1b114011c65c");

            _mockUserContext.Setup(u => u.ObterUserId()).Returns(userId);
            _mockTaskQuery.Setup(q => q.Get(taskId))
                .ReturnsAsync(new TaskEditionModel() { Id = taskId, Title = "Task Title", Description = "Task Description", Status = StatusEnum.Pendente, UserId = userId });
            var controller = new TaskController(_mockUserContext.Object, _mockMediator.Object, _mockTaskQuery.Object);

            // Act
            var result = await controller.GetTask(taskId);

            // Assert
            var createdResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<TaskEditionModel>(createdResult.Value);
            Assert.Equal(taskId, model.Id);
            Assert.Equal("Task Title", model.Title);
            Assert.Equal("Task Description", model.Description);
            Assert.Equal(StatusEnum.Pendente, model.Status);
        }

        [Fact]
        public async Task NaoDeveObterUmaTarefaSeElaNaoPertencerAoUsuarioLogadoComFuncaoUser()
        {
            // Arrange
            var taskId = Guid.Parse("05fb4f22-9549-462a-895f-3d4fca4455ae");
            var userId = Guid.Parse("1cc3474b-d24c-472d-843a-1b114011c65c");

            _mockUserContext.Setup(u => u.ObterUserId()).Returns(userId);
            _mockUserContext.Setup(u => u.PossuiRole(nameof(RoleEnum.Admin))).Returns(false);
            _mockTaskQuery.Setup(q => q.Get(taskId))
                .ReturnsAsync(new TaskEditionModel() { Id = taskId, Title = "Task Title", Description = "Task Description", Status = StatusEnum.Pendente, UserId = Guid.Parse("7b0758a4-37fe-48f6-95fa-a7d85625bc12") });
            var controller = new TaskController(_mockUserContext.Object, _mockMediator.Object, _mockTaskQuery.Object);

            // Act
            var result = await controller.GetTask(taskId);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task DeveObterUmaTarefaSeElaNaoPertencerAoUsuarioLogadoComFuncaoAdmin()
        {
            // Arrange
            var taskId = Guid.Parse("05fb4f22-9549-462a-895f-3d4fca4455ae");
            var userId = Guid.Parse("1cc3474b-d24c-472d-843a-1b114011c65c");

            _mockUserContext.Setup(u => u.ObterUserId()).Returns(userId);
            _mockUserContext.Setup(u => u.PossuiRole(nameof(RoleEnum.Admin))).Returns(true);
            _mockTaskQuery.Setup(q => q.Get(taskId))
                .ReturnsAsync(new TaskEditionModel() { Id = taskId, Title = "Task Title", Description = "Task Description", Status = StatusEnum.Pendente, UserId = Guid.Parse("7b0758a4-37fe-48f6-95fa-a7d85625bc12") });
            var controller = new TaskController(_mockUserContext.Object, _mockMediator.Object, _mockTaskQuery.Object);

            // Act
            var result = await controller.GetTask(taskId);

            // Assert
            var createdResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<TaskEditionModel>(createdResult.Value);
            Assert.Equal(taskId, model.Id);
            Assert.Equal("Task Title", model.Title);
            Assert.Equal("Task Description", model.Description);
            Assert.Equal(StatusEnum.Pendente, model.Status);
        }
    }
}
