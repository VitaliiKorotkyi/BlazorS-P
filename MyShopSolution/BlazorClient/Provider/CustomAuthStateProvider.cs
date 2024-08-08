using BlazorClient.Provider.JWT;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace BlazorClient.Provider
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private const string TokenKey = "authToken";

        public CustomAuthStateProvider(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            var token = await GetTokenAsync();

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var claims = JwtParser.ParseClaimsFromJwt(token);
                    identity = new ClaimsIdentity(claims, "jwt");
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                catch (Exception ex)
                {
                    // Log or handle the exception as needed
                    Console.WriteLine($"Error parsing JWT token: {ex.Message}");
                    identity = new ClaimsIdentity();
                }
            }

            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }

        public async Task SetTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
                _httpClient.DefaultRequestHeaders.Authorization = null;

                var identity = new ClaimsIdentity();
                var user = new ClaimsPrincipal(identity);
                var state = new AuthenticationState(user);

                NotifyAuthenticationStateChanged(Task.FromResult(state));
            }
            else
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);

                var claims = JwtParser.ParseClaimsFromJwt(token);
                var identity = new ClaimsIdentity(claims, "jwt");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var user = new ClaimsPrincipal(identity);
                var state = new AuthenticationState(user);

                NotifyAuthenticationStateChanged(Task.FromResult(state));
            }
        }

        private async Task<string> GetTokenAsync()
        {
            // Получение токена из локального хранилища
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);
            return token;
        }
      
    }
}
