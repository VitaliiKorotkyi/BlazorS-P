using Microsoft.AspNetCore.Components;

namespace BlazorClient.Pages
{

    public class ClimateTechnologyBase : ProductsListBase
    {
        protected override async Task OnInitializedAsync()
        {
            Category = "ClimateTechnology";
            await base.OnInitializedAsync();
        }
    }
}
