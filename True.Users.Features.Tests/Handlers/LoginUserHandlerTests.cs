namespace True.Users.Features.Tests.Handlers
{
    [TestClass]
    public class LoginUserHandlerTests
    {
        private IUserRepository _repository = null!;
        private LoginUserHandler _handler = null!;

        [TestInitialize]
        public void Setup()
        {
            _repository = A.Fake<IUserRepository>();
            _handler = new LoginUserHandler(_repository);
        }

        [TestMethod]
        public async Task Handle_ValidCredentials_ReturnsSuccess()
        {
            // Arrange
            var user = new User("testuser", "correctpassword");
            var command = new LoginUserCommand("testuser", "correctpassword");
            A.CallTo(() => _repository.FindUser("testuser", A<CancellationToken>._))
                .Returns(Task.FromResult<User?>(user));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
        }

        [TestMethod]
        public async Task Handle_InvalidUsername_ReturnsFail()
        {
            // Arrange
            var command = new LoginUserCommand("nonexistent", "password");
            A.CallTo(() => _repository.FindUser("nonexistent", A<CancellationToken>._))
                .Returns(Task.FromResult<User?>(null));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Errors.ShouldNotBeEmpty();
            result.Errors.First().Message.ShouldBe("Wrong username or password");
        }

        [TestMethod]
        public async Task Handle_InvalidPassword_ReturnsFail()
        {
            // Arrange
            var user = new User("testuser", "correctpassword");
            var command = new LoginUserCommand("testuser", "wrongpassword");
            A.CallTo(() => _repository.FindUser("testuser", A<CancellationToken>._))
                .Returns(Task.FromResult<User?>(user));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Errors.First().Message.ShouldBe("Wrong username or password");
        }

        [TestMethod]
        public async Task Handle_NullPassword_ReturnsFail()
        {
            // Arrange
            var user = new User("testuser", "somepassword");
            var command = new LoginUserCommand("testuser", null!);
            A.CallTo(() => _repository.FindUser("testuser", A<CancellationToken>._))
                .Returns(Task.FromResult<User?>(user));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeFalse();
        }

        [TestMethod]
        public async Task Handle_CancellationToken_RespectsCancellation()
        {
            // Arrange
            var command = new LoginUserCommand("testuser", "password");
            var cts = new CancellationTokenSource();
            cts.Cancel();

            A.CallTo(() => _repository.FindUser("testuser", cts.Token))
                .Throws(new OperationCanceledException());

            // Act & Assert
            await Should.ThrowAsync<OperationCanceledException>(() =>
                _handler.Handle(command, cts.Token));
        }
    }
}
