using Microsoft.AspNetCore.Components;

namespace BlazorClient.Pages
{
    public class LaptopsTabletsBase:ProductsListBase
    {
        protected override async Task OnInitializedAsync()
        {
            Category = "LaptopsAndTablets";
            await base.OnInitializedAsync();
        }
    }
}
