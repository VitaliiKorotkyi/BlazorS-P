using Core.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorClient.Pages
{
    public class TvProjectorsVideoBase:ProductsListBase
    {
        protected override async Task OnInitializedAsync()
        {
            Category = "TvProjectorsAndVideo";
            await base.OnInitializedAsync();
        }
    }
}
