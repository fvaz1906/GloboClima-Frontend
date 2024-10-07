using GloboClima_Frontend.Models;
using System.Net.Http.Headers;

namespace GloboClima_Frontend.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(
            HttpClient httpClient,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Weather?> GetWeatherAsync(string city)
        {
            try
            {
                var url = $"{_configuration["Api:Url"]}/api/Weather/{city}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<Weather>();
                return result;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Erro na requisição: {e.Message}");
                return null;
            }
        }

        public async Task<Country?> GetCountryAsync(string country)
        {
            try
            {
                var url = $"{_configuration["Api:Url"]}/api/Country/{country}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<Country>();
                return result;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Erro na requisição: {e.Message}");
                return null;
            }
        }

        public async Task<List<Favorite>?> GetFavoritesAsync()
        {
            try
            {
                var url = $"{_configuration["Api:Url"]}/api/v1/Favorite/list";
                var token = _httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];

                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<List<Favorite>>();
                return result;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Erro na requisição: {e.Message}");
                return null;
            }
        }

        public async Task<bool> SaveFavoritesAsync(Favorite favorite)
        {
            try
            {
                var url = $"{_configuration["Api:Url"]}/api/v1/Favorite/save";
                var token = _httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];

                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpClient.PostAsJsonAsync(url, new
                {
                    country = favorite.Country,
                    city = favorite.City
                });

                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<Favorite>();

                if (result != null)
                {
                    return true;
                }

                return false;

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Erro na requisição: {e.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteFavoritesAsync(string hashId)
        {
            try
            {
                var url = $"{_configuration["Api:Url"]}/api/v1/Favorite/delete/{hashId}";
                var token = _httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];

                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpClient.DeleteAsync(url);
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Erro na requisição: {e.Message}");
                return false;
            }
        }

        public async Task<bool> Login(User user)
        {
            try
            {
                var url = $"{_configuration["Api:Url"]}/api/Auth/login";
                var response = await _httpClient.PostAsJsonAsync(url, user);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<Token>();

                _httpContextAccessor.HttpContext?.Response.Cookies.Append("AuthToken", result.Key, new CookieOptions
                {
                    HttpOnly = true,  // Apenas acessível via HTTP (não acessível por JavaScript)
                    Expires = DateTime.UtcNow.AddHours(1)  // Expiração do token
                });

                _httpContextAccessor.HttpContext?.Response.Cookies.Append("AuthMail", result.Email, new CookieOptions
                {
                    HttpOnly = true,  // Apenas acessível via HTTP (não acessível por JavaScript)
                    Expires = DateTime.UtcNow.AddHours(1)  // Expiração do token
                });

                return true;

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Erro na requisição: {e.Message}");
                return false;
            }
        }


        public async Task<bool> Register(User user)
        {
            try
            {
                var url = $"{_configuration["Api:Url"]}/api/Auth/register";
                var response = await _httpClient.PostAsJsonAsync(url, user);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<User>();
                
                if (result != null)
                {
                    return true;
                }

                return false;

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Erro na requisição: {e.Message}");
                return false;
            }
        }


    }
}
