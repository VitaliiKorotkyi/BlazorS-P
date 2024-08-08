using Core.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorClient.Pages
{
    public class HomeAppliancesBase:ProductsListBase
    {
        protected override async Task OnInitializedAsync()
        {
            Category = "HomeAppliances";
            await base.OnInitializedAsync();
        }
    }
}
