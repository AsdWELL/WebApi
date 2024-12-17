using Microsoft.AspNetCore.Mvc.Testing;
using SportNewsWebApi.Models;
using SportNewsWebApi.Requests;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using UsersWebApi.Requests;
using Xunit.Abstractions;

namespace Tests
{
    public class SportNewsResponse
    {
        public List<SportNews> SportNews { get; set; }
    }
    
    public class GatewayTests(WebApplicationFactory<Gateway.Program> factory, ITestOutputHelper output) 
        : IClassFixture<WebApplicationFactory<Gateway.Program>>
    {
        private readonly string _baseUrl = "https://localhost:7293/gateway";

        private readonly HttpClient _httpClient = factory.CreateClient();

        private readonly ITestOutputHelper _output = output;

        private async Task<HttpResponseMessage> Authorize(LoginUserRequest loginUserRequest)
        {
            return await _httpClient.PostAsJsonAsync(_baseUrl + "/login", loginUserRequest);
        }

        [Theory]
        [ClassData(typeof(LoginData))]
        public async Task Login(LoginUserRequest loginUserRequest)
        {
            var response = await Authorize(loginUserRequest);

            Assert.True(response.StatusCode == HttpStatusCode.OK, await response.Content.ReadAsStringAsync());
        }

        [Theory]
        [ClassData(typeof(RegisterData))]
        public async Task Register(RegisterUserRequest request)
        {
            _output.WriteLine($"Логин: {request.Login}, Пароль: {request.Password}");

            var response = await _httpClient.PostAsJsonAsync(_baseUrl + "/register", request);

            Assert.True(response.StatusCode == HttpStatusCode.OK, await response.Content.ReadAsStringAsync());
        }

        [Theory]
        [ClassData(typeof(LoginData))]
        public async Task GetUser(LoginUserRequest loginUserRequest)
        {
            var loginResponse = await Authorize(loginUserRequest);

            if (loginResponse.StatusCode == HttpStatusCode.OK)
            {
                var response = await _httpClient.GetAsync(_baseUrl + "/user");

                _output.WriteLine(await response.Content.ReadAsStringAsync());

                Assert.True(true);
            }
            else
                Assert.Fail(await loginResponse.Content.ReadAsStringAsync());
        }

        [Theory]
        [ClassData(typeof(LoginData))]
        public async Task DeleteUser(LoginUserRequest loginUserRequest)
        {
            var loginResponse = await Authorize(loginUserRequest);

            if (loginResponse.StatusCode == HttpStatusCode.OK)
            {
                var response = await _httpClient.DeleteAsync(_baseUrl + "/user");

                Assert.True(response.StatusCode == HttpStatusCode.OK, await response.Content.ReadAsStringAsync());
            }
            else
                Assert.Fail(await loginResponse.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task GetSportNewsUnauthorized()
        {
            var response = await _httpClient.GetAsync(_baseUrl + "/sportnews");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [ClassData(typeof(LoginData))]
        public async Task GetSportNewsAuthorized(LoginUserRequest loginUserRequest)
        {
            var loginResponse = await Authorize(loginUserRequest);

            if (loginResponse.StatusCode == HttpStatusCode.OK)
            {
                var response = await _httpClient.GetAsync(_baseUrl + "/sportnews");

                Assert.True(response.StatusCode == HttpStatusCode.OK, await response.Content.ReadAsStringAsync());
            }
            else
                Assert.Fail(await loginResponse.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task AddSportNewsUnauthorized()
        {
            var sportNews = new CreateSportNewsRequest
            {
                Title = "Заголовок",
                Content = "Содержание",
                UserId = 0
            };

            var response = await _httpClient.PostAsJsonAsync(_baseUrl + "/sportnews", sportNews);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [ClassData(typeof(LoginData))]
        public async Task AddSportNewsAuthorized(LoginUserRequest loginUserRequest)
        {
            var loginResponse = await Authorize(loginUserRequest);

            if (loginResponse.StatusCode == HttpStatusCode.OK)
            {
                var sportNews = new CreateSportNewsRequest
                {
                    Title = "Тест",
                    Content = "Тест",
                    UserId = 0
                };

                var response = await _httpClient.PostAsJsonAsync(_baseUrl + "/sportnews", sportNews);

                Assert.True(response.StatusCode == HttpStatusCode.OK, await response.Content.ReadAsStringAsync());
            }
            else
                Assert.Fail(await loginResponse.Content.ReadAsStringAsync());
            
        }

        private async Task<int?> GetSportNewsId(LoginUserRequest loginUserRequest)
        {
            var loginResponse = await Authorize(loginUserRequest);

            if (loginResponse.StatusCode == HttpStatusCode.OK)
            {
                var response = await _httpClient.GetAsync(_baseUrl + "/sportnews");

                var content = await response.Content.ReadAsStringAsync();

                var sportNews = JsonSerializer.Deserialize<SportNewsResponse>(content,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }).SportNews;

                if (sportNews.Count > 0)
                    return sportNews[0].Id;
            }

            return null;
        }

        [Fact]
        public async Task DeleteSportNewsUnauthorized()
        {
            var response = await _httpClient.DeleteAsync(_baseUrl + $"/sportnews/{new Random().Next()}");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [ClassData(typeof(LoginData))]
        public async Task DeleteSportNewsAuthorized(LoginUserRequest loginUserRequest)
        {
            int? id = await GetSportNewsId(loginUserRequest);

            if (id == null)
                Assert.Fail("Отсутствуют новости в бд");

            var response = await _httpClient.DeleteAsync(_baseUrl + $"/sportnews/{id}");

            Assert.True(response.StatusCode == HttpStatusCode.OK, await response.Content.ReadAsStringAsync());
        }
    }
}
