using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using TLA.Tasks.Api.Application.Commands;
using TLA.Tasks.Api.Domain.Interface;
using TLA.WebApi.Core.Data.Interface;
using TLA.WebApi.Core.Identity;
using Xunit;

namespace TLA.Tasks.Tests
{
    public class AddTaskCommandTest
    {
        private readonly Mock<ITaskRepository> _mockTaskRepository;
        private readonly Mock<IAspNetUser> _mockUserContext;
        private readonly Mock<IUnitOfWork> _mockUow;

        public AddTaskCommandTest()
        {
            _mockTaskRepository = new Mock<ITaskRepository>();
            _mockUserContext = new Mock<IAspNetUser>();
            _mockUow = new Mock<IUnitOfWork>();
            _mockUow.Setup(u => u.Commit()).ReturnsAsync(true);
            _mockTaskRepository.Setup(m => m.UnitOfWork).Returns(_mockUow.Object);
        }

        [Fact]
        public async Task DeveCriarUmaTarefaComSucesso()
        {
            // Arrange
            var handler = new TaskCommandHandler(_mockTaskRepository.Object, _mockUserContext.Object);
            var command = new AddTaskCommand(Guid.Parse("1cc3474b-d24c-472d-843a-1b114011c65c"), "Task Title", "Task Description");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Theory]
        [InlineData("", "")]
        public async Task DeveImpedirDeCriarUmaTarefaCasoOsCamposObrigatoriosNaoForemPreenchidos(string title, string description)
        {
            // Arrange
            var handler = new TaskCommandHandler(_mockTaskRepository.Object, _mockUserContext.Object);
            var command = new AddTaskCommand(Guid.Empty, title, description);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public async Task DeveImpedirDeCriarUmaTarefaCasoOUsuarioPertencenteATarefaNaoForDefinido()
        {
            // Arrange
            var handler = new TaskCommandHandler(_mockTaskRepository.Object, _mockUserContext.Object);
            var command = new AddTaskCommand(Guid.Empty, "Task Title", "Task Description");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
        }
    }
}
