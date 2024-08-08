using Microsoft.AspNetCore.Components;

namespace BlazorClient.Pages
{
    public class KitchenAppliancesBase : ProductsListBase
    {
        protected override async Task OnInitializedAsync()
        {
            Category = "KitchenAppliances";
            await base.OnInitializedAsync();
        }
    }
}
