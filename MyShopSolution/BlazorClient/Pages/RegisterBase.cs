using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Core.Models;

namespace BlazorClient.Pages
{
    public class RegisterBase : ComponentBase
    {
        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        public RegisterModel RegisterModel { get; set; } = new RegisterModel();

        public async Task HandleRegister()
        {
            try
            {
                var response = await Http.PostAsJsonAsync("https://localhost:7057/api/account/register", RegisterModel);
                if (response.IsSuccessStatusCode)
                {
                    Navigation.NavigateTo("/dashboard");
                }
                else
                {
                    // Handle registration failure
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Registration failed: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }
}