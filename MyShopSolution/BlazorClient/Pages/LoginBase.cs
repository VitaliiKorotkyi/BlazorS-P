using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorClient.Provider;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json;
using Core.Models;

namespace BlazorClient.Pages
{
    public class LoginBase : ComponentBase
    {
        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Inject]
        protected NavigationManager Navigation { get; set; }

        [Inject]
        protected ILogger<LoginBase> Logger { get; set; }

        [Inject]
        protected CustomAuthStateProvider AuthStateProvider { get; set; }

        protected LoginModel loginModel = new LoginModel();

        protected async Task HandleLogin()
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync("https://localhost:7057/api/account/login", loginModel);


                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonSerializer.Deserialize<JsonElement>(jsonResponse);

                    if (responseObject.TryGetProperty("token", out var tokenElement))
                    {
                        var token = tokenElement.GetString();
                        Logger.LogInformation("Получен токен: {Token}", token);

                        try
                        {
                            await AuthStateProvider.SetTokenAsync(token);
                            Navigation.NavigateTo("/dashboard");
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(ex, "Ошибка установки токена");
                        }
                    }
                    else
                    {
                        Logger.LogWarning("Токен не найден в ответе.");
                    }
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Logger.LogWarning("Неверная попытка входа. Ответ сервера: {Response}", error);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Исключение во время попытки входа");
            }
        }
    }
}
