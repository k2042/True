namespace True.Finance.Features.Tests.Handlers
{
    [TestClass]
    public class GetFavoriteCurrenciesHandlerTests
    {
        private ICurrencyRepository _repository = null!;
        private GetFavoriteCurrenciesHandler _handler = null!;

        [TestInitialize]
        public void Setup()
        {
            _repository = A.Fake<ICurrencyRepository>();
            _handler = new GetFavoriteCurrenciesHandler(_repository);
        }

        [TestMethod]
        public async Task Handle_ValidFavorites_ReturnsCurrencies()
        {
            // Arrange
            var currencies = new List<Currency>
            {
                new Currency("USD", "US Dollar", 1.0m),
                new Currency("EUR", "Euro", 0.92m)
            };
            var query = new GetFavoriteCurrenciesQuery(new[] { "USD", "EUR" });
            A.CallTo(() => _repository.GetCurrenciesList(A<IEnumerable<string>>._, A<CancellationToken>._))
                .Returns(Task.FromResult(currencies));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(2);
            result.Value.First().Id.ShouldBe("USD");
        }

        [TestMethod]
        public async Task Handle_EmptyFavorites_ReturnsEmptyList()
        {
            // Arrange
            var currencies = new List<Currency>();
            var query = new GetFavoriteCurrenciesQuery(Array.Empty<string>());
            A.CallTo(() => _repository.GetCurrenciesList(A<IEnumerable<string>>._, A<CancellationToken>._))
                .Returns(Task.FromResult(currencies));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(0);
        }

        [TestMethod]
        public async Task Handle_NonExistentCurrencies_ReturnsOnlyExisting()
        {
            // Arrange
            var currencies = new List<Currency>
            {
                new Currency("USD", "US Dollar", 1.0m)
            };
            var query = new GetFavoriteCurrenciesQuery(new[] { "USD", "INVALID", "NONEXIST" });
            A.CallTo(() => _repository.GetCurrenciesList(A<IEnumerable<string>>._, A<CancellationToken>._))
                .Returns(Task.FromResult(currencies));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Value.Count.ShouldBe(1);
            result.Value.First().Id.ShouldBe("USD");
        }

        [TestMethod]
        public async Task Handle_VerifyRepositoryCall()
        {
            // Arrange
            var currencies = new List<Currency>();
            var favorites = new[] { "USD", "EUR", "GBP" };
            var query = new GetFavoriteCurrenciesQuery(favorites);
            A.CallTo(() => _repository.GetCurrenciesList(A<IEnumerable<string>>._, A<CancellationToken>._))
                .Returns(Task.FromResult(currencies));

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            A.CallTo(() => _repository.GetCurrenciesList(A<IEnumerable<string>>._, A<CancellationToken>._))
                .MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public async Task Handle_CancellationToken_RespectsCancellation()
        {
            // Arrange
            var query = new GetFavoriteCurrenciesQuery(new[] { "USD" });
            var cts = new CancellationTokenSource();
            cts.Cancel();

            A.CallTo(() => _repository.GetCurrenciesList(A<IEnumerable<string>>._, cts.Token))
                .Throws(new OperationCanceledException());

            // Act & Assert
            await Should.ThrowAsync<OperationCanceledException>(() =>
                _handler.Handle(query, cts.Token));
        }

        [TestMethod]
        public async Task Handle_LargeCurrencyList_ReturnsAll()
        {
            // Arrange
            var currencies = Enumerable.Range(0, 100)
                .Select(i => new Currency($"CUR{i:D3}", $"Currency {i}", (decimal)(1.0 + i * 0.01)))
                .ToList();
            var query = new GetFavoriteCurrenciesQuery(currencies.Select(c => c.Id));
            A.CallTo(() => _repository.GetCurrenciesList(A<IEnumerable<string>>._, A<CancellationToken>._))
                .Returns(Task.FromResult(currencies));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Value.Count.ShouldBe(100);
        }
    }
}
