using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using TestTaskMixvel.Controllers.Search;
using TestTaskMixvel.Services.Interfaces.Search;
using TestTaskMixvel.Services.Models.Search;

namespace TestTaskMixvel.UnitTests.Controllers.Search
{
    public class SearchRouteControllerTest
    {
        Fixture _fixture;
        Mock<ISearchService> _searchService;
        Mock<ILogger<SearchRouteController>> _logger;
        SearchRouteController _searchRouteController;

        public SearchRouteControllerTest()
        {
            _fixture = new Fixture();
            _searchService = new Mock<ISearchService>();
            _logger = new Mock<ILogger<SearchRouteController>>();
            _searchRouteController = new SearchRouteController(_searchService.Object, _logger.Object);
        }

        [Fact]
        public async void SearchRouteController_Search_ReturnValidResult()
        {
            var token = _fixture.Create<CancellationToken>();
            var response = _fixture.Create<SearchResponse>();
            var request = _fixture.Create<SearchRequest>();

            _searchService.Setup(repo => repo.SearchAsync(request, token)).ReturnsAsync(response);
            var result = await _searchRouteController.Search(request, token);
            _searchService.Verify(repo => repo.SearchAsync(request, token), Times.Once);

            Assert.Equal(response, result);
        }

        [Fact]
        public async void SearchRouteController_IsAvailable_ReturnValidResult()
        {
            var token = _fixture.Create<CancellationToken>();
            var response = _fixture.Create<bool>();

            _searchService.Setup(repo => repo.IsAvailableAsync(token)).ReturnsAsync(response);
            var result = await _searchRouteController.IsAvailable(token);
            _searchService.Verify(repo => repo.IsAvailableAsync(token), Times.Once);

            Assert.Equal(response, result);
        }
    }
}