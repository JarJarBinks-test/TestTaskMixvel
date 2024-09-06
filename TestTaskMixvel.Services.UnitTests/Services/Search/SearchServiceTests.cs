using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using TestTaskMixvel.Repositories.Interfaces.Cache;
using TestTaskMixvel.Repositories.Interfaces.Provider;
using TestTaskMixvel.Services.Interfaces.Search;
using TestTaskMixvel.Services.Models.Search;
using TestTaskMixvel.Services.Services.Search;

namespace TestTaskMixvel.Services.UnitTests.Services.Search
{
    public class SearchServiceTests
    {
        Fixture _fixture;
        Mock<IProviderAdapter<IProviderOneRepository>> _providerOneAdapter;
        Mock<IProviderAdapter<IProviderTwoRepository>> _providerTwoAdapter;
        Mock<IRedisCacheRepository> _redisCacheRepository;
        Mock<ILogger<SearchService>> _logger;
        ISearchService _searchService;

        public SearchServiceTests()
        {
            _fixture = new Fixture();
            _providerOneAdapter = new Mock<IProviderAdapter<IProviderOneRepository>>();
            _providerTwoAdapter = new Mock<IProviderAdapter<IProviderTwoRepository>>();
            _redisCacheRepository = new Mock<IRedisCacheRepository>();
            _logger = new Mock<ILogger<SearchService>>();
            _searchService = new SearchService(_providerOneAdapter.Object, _providerTwoAdapter.Object, _redisCacheRepository.Object, _logger.Object);
        }

        [Fact]
        public async void SearchService_SearchAsync_ThrowException()
        {
            var request = _fixture.Create<SearchRequest>();
            var token = _fixture.Create<CancellationToken>();
            var exception = _fixture.Create<Exception>();

            _redisCacheRepository.Setup(repo => repo.Get<SearchResponse>(request.UniqueKey, token)).ThrowsAsync(exception);
            var exceptionResult = await Assert.ThrowsAsync<Exception>(async () => await _searchService.SearchAsync(request, token));
            _redisCacheRepository.Verify(repo => repo.Get<SearchResponse>(request.UniqueKey, token), Times.Once);
            _providerOneAdapter.Verify(repo => repo.SearchAsync(request, token), Times.Never);
            _providerTwoAdapter.Verify(repo => repo.SearchAsync(request, token), Times.Never);

            Assert.Equal(exception, exceptionResult);
        }

        [Fact]
        public async void SearchService_SearchAsync_ReturnOnlyCached()
        {
            var request = _fixture.Build<SearchRequest>()
                                  .With(request=> request.Filters, new SearchFilters() { OnlyCached = true })
                                  .Create();
            var cacheResponse = _fixture.Create<SearchResponse>();
            var token = _fixture.Create<CancellationToken>();
            var exception = _fixture.Create<Exception>();

            _redisCacheRepository.Setup(repo => repo.Get<SearchResponse>(request.UniqueKey, token)).ReturnsAsync(cacheResponse);
            var results = await _searchService.SearchAsync(request, token);
            _redisCacheRepository.Verify(repo => repo.Get<SearchResponse>(request.UniqueKey, token), Times.Once);
            _providerOneAdapter.Verify(repo => repo.SearchAsync(request, token), Times.Never);
            _providerTwoAdapter.Verify(repo => repo.SearchAsync(request, token), Times.Never);

            Assert.Equal(cacheResponse, results);
        }

        [Fact]
        public async void SearchService_SearchAsync_ReturnOnlyCachedWhenExists()
        {
            var request = _fixture.Build<SearchRequest>()
                                  .With(request => request.Filters, new SearchFilters() { OnlyCached = false })
                                  .Create();
            var cacheResponse = _fixture.Create<SearchResponse>();
            var token = _fixture.Create<CancellationToken>();
            var exception = _fixture.Create<Exception>();

            _redisCacheRepository.Setup(repo => repo.Get<SearchResponse>(request.UniqueKey, token)).ReturnsAsync(cacheResponse);
            var results = await _searchService.SearchAsync(request, token);
            _redisCacheRepository.Verify(repo => repo.Get<SearchResponse>(request.UniqueKey, token), Times.Once);
            _providerOneAdapter.Verify(repo => repo.SearchAsync(request, token), Times.Never);
            _providerTwoAdapter.Verify(repo => repo.SearchAsync(request, token), Times.Never);

            Assert.Equal(cacheResponse, results);
        }

        [Fact]
        public async void SearchService_SearchAsync_ReturnProviderResults()
        {
            var request = _fixture.Build<SearchRequest>()
                                  .With(request => request.Filters, new SearchFilters() { OnlyCached = false })
                                  .Create();
            SearchResponse cacheResponse = null!;
            var oneResponse = _fixture.Create<SearchResponse>();
            var twoResponse = _fixture.Create<SearchResponse>();
            var token = _fixture.Create<CancellationToken>();
            var exception = _fixture.Create<Exception>();

            _redisCacheRepository.Setup(repo => repo.Get<SearchResponse>(request.UniqueKey, token)).ReturnsAsync(cacheResponse);
            _providerOneAdapter.Setup(repo => repo.SearchAsync(request, token)).ReturnsAsync(oneResponse);
            _providerTwoAdapter.Setup(repo => repo.SearchAsync(request, token)).ReturnsAsync(twoResponse);
            var results = await _searchService.SearchAsync(request, token);
            _redisCacheRepository.Verify(repo => repo.Get<SearchResponse>(request.UniqueKey, token), Times.Once);
            _providerOneAdapter.Verify(repo => repo.SearchAsync(request, token), Times.Once);
            _providerTwoAdapter.Verify(repo => repo.SearchAsync(request, token), Times.Once);

            var routes = oneResponse.Routes.Concat(twoResponse.Routes).ToArray();
            var expectedResult = new SearchResponse()
            {
                Routes = routes,
                MinPrice = routes.Min(route => route.Price),
                MaxPrice = routes.Max(route => route.Price),
                MinMinutesRoute = Convert.ToInt32(routes.Min(route => (route.DestinationDateTime - route.OriginDateTime).TotalMinutes)),
                MaxMinutesRoute = Convert.ToInt32(routes.Max(route => (route.DestinationDateTime - route.OriginDateTime).TotalMinutes)),
            };
            Assert.Equal(expectedResult.MaxPrice, results.MaxPrice);
            Assert.Equal(expectedResult.MinPrice, results.MinPrice);
            Assert.Equal(expectedResult.MaxMinutesRoute, results.MaxMinutesRoute);
            Assert.Equal(expectedResult.MinMinutesRoute, results.MinMinutesRoute);
            Assert.Equal(expectedResult.Routes.Select(route=> route.TimeLimit), results.Routes.Select(route => route.TimeLimit));
            Assert.Equal(expectedResult.Routes.Select(route => route.OriginDateTime), results.Routes.Select(route => route.OriginDateTime));
            Assert.Equal(expectedResult.Routes.Select(route => route.Price), results.Routes.Select(route => route.Price));
            Assert.Equal(expectedResult.Routes.Select(route => route.Id), results.Routes.Select(route => route.Id));
            Assert.Equal(expectedResult.Routes.Select(route => route.Destination), results.Routes.Select(route => route.Destination));
            Assert.Equal(expectedResult.Routes.Select(route => route.DestinationDateTime), results.Routes.Select(route => route.DestinationDateTime));
        }

        [Fact]
        public async void SearchService_IsAvailableAsync_ReturnTrueOnlyOneAvailable()
        {
            var oneResponse = false;
            var twoResponse = true;
            var token = _fixture.Create<CancellationToken>();
            var exception = _fixture.Create<Exception>();

            _providerOneAdapter.Setup(repo => repo.IsAvailableAsync(token)).ReturnsAsync(oneResponse);
            _providerTwoAdapter.Setup(repo => repo.IsAvailableAsync(token)).ReturnsAsync(twoResponse);
            var result = await _searchService.IsAvailableAsync(token);
            _providerOneAdapter.Verify(repo => repo.IsAvailableAsync(token), Times.Once);
            _providerTwoAdapter.Verify(repo => repo.IsAvailableAsync(token), Times.Once);

            Assert.True(result);
        }

        [Fact]
        public async void SearchService_IsAvailableAsync_ReturnTrueOnlyTwoAvailable()
        {
            var oneResponse = true;
            var twoResponse = false;
            var token = _fixture.Create<CancellationToken>();
            var exception = _fixture.Create<Exception>();

            _providerOneAdapter.Setup(repo => repo.IsAvailableAsync(token)).ReturnsAsync(oneResponse);
            _providerTwoAdapter.Setup(repo => repo.IsAvailableAsync(token)).ReturnsAsync(twoResponse);
            var result = await _searchService.IsAvailableAsync(token);
            _providerOneAdapter.Verify(repo => repo.IsAvailableAsync(token), Times.Once);
            _providerTwoAdapter.Verify(repo => repo.IsAvailableAsync(token), Times.Once);

            Assert.True(result);
        }

        [Fact]
        public async void SearchService_IsAvailableAsync_ReturnFalseNoAvailable()
        {
            var oneResponse = false;
            var twoResponse = false;
            var token = _fixture.Create<CancellationToken>();
            var exception = _fixture.Create<Exception>();

            _providerOneAdapter.Setup(repo => repo.IsAvailableAsync(token)).ReturnsAsync(oneResponse);
            _providerTwoAdapter.Setup(repo => repo.IsAvailableAsync(token)).ReturnsAsync(twoResponse);
            var result = await _searchService.IsAvailableAsync(token);
            _providerOneAdapter.Verify(repo => repo.IsAvailableAsync(token), Times.Once);
            _providerTwoAdapter.Verify(repo => repo.IsAvailableAsync(token), Times.Once);

            Assert.False(result);
        }
    }
}