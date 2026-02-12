namespace True.Users.Infrastructure.Tests.Repositories
{
    [TestClass]
    public class UserRepositoryTests
    {
        private class TrueDbContextInMemory(DbContextOptions options) : TrueDbContext(options);


        private ITrueDbContext _dbContext = null!;
        private UserRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TrueDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new TrueDbContextInMemory(options);
            _repository = new UserRepository(_dbContext);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext?.Dispose();
        }

        [TestMethod]
        public async Task CreateUser_ValidUser_InsertsToDatabase()
        {
            // Arrange
            var user = new User("newuser", "password123");

            // Act
            await _repository.CreateUser(user, CancellationToken.None);

            // Assert
            var dbUser = await _dbContext.Users.SingleOrDefaultAsync(u => u.Name == "newuser");
            dbUser.ShouldNotBeNull();
            dbUser.Name.ShouldBe("newuser");
            dbUser.Password.ShouldBe("password123");
        }

        [TestMethod]
        public async Task FindUser_ExistingUser_ReturnsUserRecord()
        {
            // Arrange
            var testUser = new UserDbo { Name = "testuser", Password = "hashedpass" };
            _dbContext.Users.Add(testUser);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await _repository.FindUser("testuser", CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Name.ShouldBe("testuser");
            result.Password.ShouldBe("hashedpass");
        }

        [TestMethod]
        public async Task FindUser_NonExistingUser_ReturnsNull()
        {
            // Arrange
            var testUser = new UserDbo { Name = "existinguser", Password = "pass" };
            _dbContext.Users.Add(testUser);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await _repository.FindUser("nonexistent", CancellationToken.None);

            // Assert
            result.ShouldBeNull();
        }

        [TestMethod]
        public async Task FindUser_CaseInsensitiveMatch_WorksRegardlessOfCase()
        {
            // Arrange
            var testUser = new UserDbo { Name = "TestUser", Password = "password" };
            _dbContext.Users.Add(testUser);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Act - Try different cases
            var result1 = await _repository.FindUser("testuser", CancellationToken.None);
            var result2 = await _repository.FindUser("TESTUSER", CancellationToken.None);
            var result3 = await _repository.FindUser("TestUser", CancellationToken.None);

            // Assert
            result1.ShouldNotBeNull();
            result2.ShouldNotBeNull();
            result3.ShouldNotBeNull();
            result1.Name.ShouldBe("TestUser");
            result2.Name.ShouldBe("TestUser");
            result3.Name.ShouldBe("TestUser");
        }

        [TestMethod]
        public async Task FindUser_CancellationToken_RespectsCancellation()
        {
            // Arrange
            var testUser = new UserDbo { Name = "testuser", Password = "pass" };
            _dbContext.Users.Add(testUser);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
            var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act & Assert
            await Should.ThrowAsync<OperationCanceledException>(() =>
                _repository.FindUser("testuser", cts.Token));
        }

        [TestMethod]
        public async Task CreateUser_CallsSaveChangesAsync()
        {
            // Arrange
            var user = new User("anotheruser", "pass");
            var initialCount = _dbContext.Users.Count();

            // Act
            await _repository.CreateUser(user, CancellationToken.None);

            // Assert
            var finalCount = _dbContext.Users.Count();
            (finalCount > initialCount).ShouldBeTrue();
        }

        [TestMethod]
        public async Task FindUser_MultipleUsers_ReturnsCorrectOne()
        {
            // Arrange
            _dbContext.Users.AddRange(
                new UserDbo { Name = "user1", Password = "pass1" },
                new UserDbo { Name = "user2", Password = "pass2" },
                new UserDbo { Name = "user3", Password = "pass3" }
            );
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await _repository.FindUser("user2", CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Name.ShouldBe("user2");
            result.Password.ShouldBe("pass2");
        }
    }
}
