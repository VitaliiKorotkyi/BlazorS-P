using Core.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorClient.Pages
{
    public class SmartWatchesTrackersBase:ProductsListBase
    {
        protected override async Task OnInitializedAsync()
        {
            Category = "SmartWatchesAndTrackers";
            await base.OnInitializedAsync();
        }
    }
}
