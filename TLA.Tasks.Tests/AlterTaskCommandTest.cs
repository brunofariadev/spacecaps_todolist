using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using TLA.Tasks.Api.Application.Commands;
using TLA.Tasks.Api.Domain.Entities;
using TLA.Tasks.Api.Domain.Enums;
using TLA.Tasks.Api.Domain.Interface;
using TLA.WebApi.Core.Data.Interface;
using TLA.WebApi.Core.Enums;
using TLA.WebApi.Core.Identity;
using Xunit;

namespace TLA.Tasks.Tests
{
    public class AlterTaskCommandTest
    {
        private readonly Mock<ITaskRepository> _mockTaskRepository;
        private readonly Mock<IAspNetUser> _mockUserContext;
        private readonly Mock<IUnitOfWork> _mockUow;

        public AlterTaskCommandTest()
        {
            _mockTaskRepository = new Mock<ITaskRepository>();
            _mockUserContext = new Mock<IAspNetUser>();
            _mockUow = new Mock<IUnitOfWork>();
            _mockUow.Setup(u => u.Commit()).ReturnsAsync(true);
            _mockTaskRepository.Setup(m => m.UnitOfWork).Returns(_mockUow.Object);
        }

        [Fact]
        public async Task DeveAlterarUmaTarefaComSucesso()
        {
            // Arrange
            var userId = Guid.Parse("1cc3474b-d24c-472d-843a-1b114011c65c");
            var task = new TaskTL("Task Title", "Task Description", userId);

            var handler = new TaskCommandHandler(_mockTaskRepository.Object, _mockUserContext.Object);
            var command = new AlterTaskCommand(task.Id, "Task Title", "Task Description", StatusEnum.Concluido);

            _mockUserContext.Setup(u => u.ObterUserId()).Returns(userId);
            _mockTaskRepository.Setup(q => q.Get(task.Id)).ReturnsAsync(task);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public async Task DeveAlterarUmaTarefaComSucessoQueNaoSejaDoUserLogadoMasEleTenhaFuncaoDeAdmin()
        {
            // Arrange
            var userLoginAdminId = Guid.Parse("1cc3474b-d24c-472d-843a-1b114011c65c");
            var userTaskId = Guid.Parse("7b0758a4-37fe-48f6-95fa-a7d85625bc12");
            var task = new TaskTL("Task Title", "Task Description", userTaskId);

            var handler = new TaskCommandHandler(_mockTaskRepository.Object, _mockUserContext.Object);
            var command = new AlterTaskCommand(task.Id, "Task Title", "Task Description", StatusEnum.Concluido);

            _mockUserContext.Setup(u => u.ObterUserId()).Returns(userLoginAdminId);
            _mockTaskRepository.Setup(q => q.Get(task.Id)).ReturnsAsync(task);
            _mockUserContext.Setup(u => u.PossuiRole(nameof(RoleEnum.Admin))).Returns(true);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public async Task NaoDeveAlterarUmaTarefaQueNaoSejaDoUserLogadoEEleTenhaFuncaoDeUser()
        {
            // Arrange
            var userLoginAdminId = Guid.Parse("1cc3474b-d24c-472d-843a-1b114011c65c");
            var userTaskId = Guid.Parse("7b0758a4-37fe-48f6-95fa-a7d85625bc12");
            var task = new TaskTL("Task Title", "Task Description", userTaskId);

            var handler = new TaskCommandHandler(_mockTaskRepository.Object, _mockUserContext.Object);
            var command = new AlterTaskCommand(task.Id, "Task Title", "Task Description", StatusEnum.Concluido);

            _mockUserContext.Setup(u => u.ObterUserId()).Returns(userLoginAdminId);
            _mockTaskRepository.Setup(q => q.Get(task.Id)).ReturnsAsync(task);
            _mockUserContext.Setup(u => u.PossuiRole(nameof(RoleEnum.Admin))).Returns(false);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public async Task NaoDeveAlterarUmaTarefaComSucessoSeExistirAlgumCampoVazio()
        {
            // Arrange
            var userId = Guid.Parse("1cc3474b-d24c-472d-843a-1b114011c65c");
            var task = new TaskTL("Task Title", "Task Description", userId);

            var handler = new TaskCommandHandler(_mockTaskRepository.Object, _mockUserContext.Object);
            var command = new AlterTaskCommand(task.Id, "", "", StatusEnum.Concluido);

            _mockUserContext.Setup(u => u.ObterUserId()).Returns(userId);
            _mockTaskRepository.Setup(q => q.Get(task.Id)).ReturnsAsync(task);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
        }
    }
}
