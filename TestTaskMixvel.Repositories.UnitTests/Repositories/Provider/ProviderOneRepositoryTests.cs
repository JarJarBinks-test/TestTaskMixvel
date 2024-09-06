using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;
using TestTaskMixvel.Repositories.Interfaces.Provider;
using TestTaskMixvel.Repositories.Models.Provider.ProviderOne;
using TestTaskMixvel.Repositories.Repositories.Search;

namespace TestTaskMixvel.Repositories.UnitTests.Repositories.Provider
{
    public class ProviderOneRepositoryTests
    {
        Fixture _fixture;
        Mock<ILogger<ProviderOneRepository>> _logger;
        Mock<IHttpClientFactory> _httpFactory;
        IProviderOneRepository _providerOneRepository;

        public ProviderOneRepositoryTests()
        {
            _fixture = new Fixture();
            _logger = new Mock<ILogger<ProviderOneRepository>>();
            _httpFactory = new Mock<IHttpClientFactory>();
            _providerOneRepository = new ProviderOneRepository(_httpFactory.Object, _logger.Object);
        }

        [Fact]
        public async void ProviderOneRepository_SearchAsync_ThrowBadStatusException()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.InternalServerError,
                   Content = new StringContent(string.Empty, null, "application/json"),
               });
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://a.a/"),
            };

            var request = _fixture.Create<ProviderOneSearchRequest>();
            var token = new CancellationToken(false);

            _httpFactory.Setup(factory => factory.CreateClient("ProviderOneClient")).Returns(httpClient);
            var exceptionResult = await Assert.ThrowsAsync<HttpRequestException>(async () => await _providerOneRepository.SearchAsync(request, token));
            _httpFactory.Verify(factory => factory.CreateClient("ProviderOneClient"), Times.Once);

            Assert.NotNull(exceptionResult.Message);
        }

        [Fact]
        public async void ProviderOneRepository_SearchAsync_ThrowResultIsEmptyException()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent("", null, "application/json"),
               });
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://a.a/"),
            };

            var request = _fixture.Create<ProviderOneSearchRequest>();
            var token = new CancellationToken(false);

            _httpFactory.Setup(factory => factory.CreateClient("ProviderOneClient")).Returns(httpClient);
            var exceptionResult = await Assert.ThrowsAsync<System.Text.Json.JsonException>(async () => await _providerOneRepository.SearchAsync(request, token));
            _httpFactory.Verify(factory => factory.CreateClient("ProviderOneClient"), Times.Once);

            Assert.NotNull(exceptionResult.Message);
        }

        [Fact]
        public async void ProviderOneRepository_SearchAsync_ReturnValidResult()
        {
            var resultEntity = _fixture.Create<ProviderOneSearchResponse>();
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,                   
                   Content = JsonContent.Create(resultEntity),
               });
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://a.a/"),
            };

            var request = _fixture.Create<ProviderOneSearchRequest>();
            var token = new CancellationToken(false);

            _httpFactory.Setup(factory => factory.CreateClient("ProviderOneClient")).Returns(httpClient);
            var response = await _providerOneRepository.SearchAsync(request, token);
            _httpFactory.Verify(factory => factory.CreateClient("ProviderOneClient"), Times.Once);

            Assert.Equal(resultEntity.Routes.Select(route=>route.To), response.Routes.Select(route => route.To));
            Assert.Equal(resultEntity.Routes.Select(route => route.From), response.Routes.Select(route => route.From));
            Assert.Equal(resultEntity.Routes.Select(route => route.Price), response.Routes.Select(route => route.Price));
            Assert.Equal(resultEntity.Routes.Select(route => route.TimeLimit), response.Routes.Select(route => route.TimeLimit));
            Assert.Equal(resultEntity.Routes.Select(route => route.DateFrom), response.Routes.Select(route => route.DateFrom));
            Assert.Equal(resultEntity.Routes.Select(route => route.DateTo), response.Routes.Select(route => route.DateTo));
        }
    }
}