namespace True.Finance.Infrastructure.Tests.Repositories
{
    [TestClass]
    public class CurrencyRepositoryTests
    {
        private class TrueDbContextInMemory(DbContextOptions options) : TrueDbContext(options);

        private ITrueDbContext _dbContext = null!;
        private CurrencyRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TrueDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new TrueDbContextInMemory(options);
            _repository = new CurrencyRepository(_dbContext);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext?.Dispose();
        }

        [TestMethod]
        public async Task GetCurrenciesList_ValidIds_ReturnsCurrencies()
        {
            // Arrange
            _dbContext.Currencies.AddRange(
                new CurrencyDbo { Id = "USD", Name = "US Dollar", Rate = 1.0m },
                new CurrencyDbo { Id = "EUR", Name = "Euro", Rate = 0.92m }
            );
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await _repository.GetCurrenciesList(["USD", "EUR"], CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBe(2);
            result.ShouldContain(c => c.Id == "USD");
            result.ShouldContain(c => c.Id == "EUR");
        }

        [TestMethod]
        public async Task GetCurrenciesList_EmptyIds_ReturnsEmptyList()
        {
            // Arrange
            _dbContext.Currencies.Add(
                new CurrencyDbo { Id = "USD", Name = "US Dollar", Rate = 1.0m }
            );
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await _repository.GetCurrenciesList(Array.Empty<string>(), CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBe(0);
        }

        [TestMethod]
        public async Task GetCurrenciesList_MixedCase_ConvertsToUppercase()
        {
            // Arrange
            _dbContext.Currencies.AddRange(
                new CurrencyDbo { Id = "USD", Name = "US Dollar", Rate = 1.0m },
                new CurrencyDbo { Id = "EUR", Name = "Euro", Rate = 0.92m }
            );
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Act - Query with mixed case
            var result = await _repository.GetCurrenciesList(["usd", "Eur"], CancellationToken.None);

            // Assert
            result.Count.ShouldBe(2);
            result.ShouldContain(c => c.Id == "USD");
            result.ShouldContain(c => c.Id == "EUR");
        }

        [TestMethod]
        public async Task GetCurrenciesList_NonExistentIds_ReturnsOnlyExisting()
        {
            // Arrange
            _dbContext.Currencies.AddRange(
                new CurrencyDbo { Id = "USD", Name = "US Dollar", Rate = 1.0m },
                new CurrencyDbo { Id = "EUR", Name = "Euro", Rate = 0.92m }
            );
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await _repository.GetCurrenciesList(
                new[] { "USD", "NONEXISTENT", "EUR", "INVALID" },
                CancellationToken.None
            );

            // Assert
            result.Count.ShouldBe(2);
            result.ShouldContain(c => c.Id == "USD");
            result.ShouldContain(c => c.Id == "EUR");
        }

        [TestMethod]
        public async Task GetCurrenciesList_DuplicateIds_ReturnsUnique()
        {
            // Arrange
            _dbContext.Currencies.Add(
                new CurrencyDbo { Id = "USD", Name = "US Dollar", Rate = 1.0m }
            );
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Act - Query with duplicate IDs
            var result = await _repository.GetCurrenciesList(
                ["USD", "USD", "USD"],
                CancellationToken.None
            );

            // Assert
            result.Count.ShouldBe(1);
            result.First().Id.ShouldBe("USD");
        }

        [TestMethod]
        public async Task GetCurrenciesList_CancellationToken_RespectsCancellation()
        {
            // Arrange
            _dbContext.Currencies.Add(
                new CurrencyDbo { Id = "USD", Name = "US Dollar", Rate = 1.0m }
            );
            await _dbContext.SaveChangesAsync(CancellationToken.None);
            var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act & Assert
            await Should.ThrowAsync<OperationCanceledException>(() =>
                _repository.GetCurrenciesList(["USD"], cts.Token));
        }

        [TestMethod]
        public async Task GetCurrenciesList_LargeDataSet_ReturnsCorrectResults()
        {
            // Arrange
            var currencies = Enumerable.Range(0, 100)
                .Select(i => new CurrencyDbo
                {
                    Id = $"CUR{i:D3}",
                    Name = $"Currency {i}",
                    Rate = (decimal)(1.0 + i * 0.01)
                })
                .ToList();
            _dbContext.Currencies.AddRange(currencies);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Act
            var ids = Enumerable.Range(0, 100).Select(i => $"CUR{i:D3}").ToList();
            var result = await _repository.GetCurrenciesList(ids, CancellationToken.None);

            // Assert
            result.Count.ShouldBe(100);
            result.ShouldContain(c => c.Id == "CUR000");
            result.ShouldContain(c => c.Id == "CUR099");
        }

        [TestMethod]
        public async Task GetCurrenciesList_PartialMatch_ReturnsOnlyMatching()
        {
            // Arrange
            _dbContext.Currencies.AddRange(
                new CurrencyDbo { Id = "USD", Name = "US Dollar", Rate = 1.0m },
                new CurrencyDbo { Id = "EUR", Name = "Euro", Rate = 0.92m },
                new CurrencyDbo { Id = "GBP", Name = "British Pound", Rate = 1.18m }
            );
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await _repository.GetCurrenciesList(["USD", "GBP"], CancellationToken.None);

            // Assert
            result.Count.ShouldBe(2);
            result.ShouldContain(c => c.Id == "USD");
            result.ShouldContain(c => c.Id == "GBP");
            result.ShouldNotContain(c => c.Id == "EUR");
        }
    }
}
