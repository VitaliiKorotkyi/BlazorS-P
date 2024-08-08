using Microsoft.AspNetCore.Components;

namespace BlazorClient.Pages
{
    public class BeautyHealthBase:ProductsList 
    {
        protected override async Task OnInitializedAsync()
        {
            Category = "BeautyAndHealth";
            await base.OnInitializedAsync();
        }
    }
}
