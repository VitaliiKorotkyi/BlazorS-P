using Core.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorClient.Pages
{
    public class GamingLeisureBase : ProductsListBase
    {
        protected override async Task OnInitializedAsync()
        {
            Category = "GamingAndLeisure";
            await base.OnInitializedAsync();
        }
    }
}
