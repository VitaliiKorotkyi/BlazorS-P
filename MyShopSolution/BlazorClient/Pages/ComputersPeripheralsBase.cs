using Microsoft.AspNetCore.Components;

namespace BlazorClient.Pages
{
    public class ComputersPeripheralsBase:ProductsList
    {
        protected override async Task OnInitializedAsync()
        {
            Category = "ComputersAndPeripherals";
            await base.OnInitializedAsync();
        }
    }
}
