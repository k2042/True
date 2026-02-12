namespace True.Users.Features.Tests.Handlers
{
    [TestClass]
    public class RegisterUserHandlerTests
    {
        private IUserRepository _repository = null!;
        private RegisterUserHandler _handler = null!;

        [TestInitialize]
        public void Setup()
        {
            _repository = A.Fake<IUserRepository>();
            _handler = new RegisterUserHandler(_repository);
        }

        [TestMethod]
        public async Task Handle_NewUser_RegistersSuccessfully()
        {
            // Arrange
            var user = new User("testuser", "password123");
            var command = new RegisterUserCommand(user);
            A.CallTo(() => _repository.FindUser("testuser", A<CancellationToken>._))
                .Returns(Task.FromResult<User?>(null));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            A.CallTo(() => _repository.CreateUser(user, A<CancellationToken>._))
                .MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public async Task Handle_ExistingUser_ReturnsFailResult()
        {
            // Arrange
            var existingUser = new User("existinguser", "password123");
            var command = new RegisterUserCommand(existingUser);
            A.CallTo(() => _repository.FindUser("existinguser", A<CancellationToken>._))
                .Returns(Task.FromResult<User?>(existingUser));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Errors.ShouldNotBeEmpty();
            result.Errors.First().Message.ShouldContain("already exists");
            A.CallTo(() => _repository.CreateUser(A<User>._, A<CancellationToken>._))
                .MustNotHaveHappened();
        }

        [TestMethod]
        public async Task Handle_VerifyRepositoryCallsForFindUser()
        {
            // Arrange
            var user = new User("testuser", "pass");
            var command = new RegisterUserCommand(user);
            A.CallTo(() => _repository.FindUser("testuser", A<CancellationToken>._))
                .Returns(Task.FromResult<User?>(null));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            A.CallTo(() => _repository.FindUser("testuser", A<CancellationToken>._))
                .MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public async Task Handle_VerifyRepositoryCallsForCreateUser()
        {
            // Arrange
            var user = new User("newuser", "password");
            var command = new RegisterUserCommand(user);
            A.CallTo(() => _repository.FindUser("newuser", A<CancellationToken>._))
                .Returns(Task.FromResult<User?>(null));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            A.CallTo(() => _repository.CreateUser(user, A<CancellationToken>._))
                .MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public async Task Handle_CancellationToken_RespectsCancellation()
        {
            // Arrange
            var user = new User("testuser", "password");
            var command = new RegisterUserCommand(user);
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
