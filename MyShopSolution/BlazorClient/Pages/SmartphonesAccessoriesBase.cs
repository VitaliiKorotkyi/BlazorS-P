using Core.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorClient.Pages
{
    public class SmartphonesAccessoriesBase:ProductsListBase
    {
        protected override async Task OnInitializedAsync()
        {
            Category = "Smartphones";
            await base.OnInitializedAsync();
        }
    }
}
