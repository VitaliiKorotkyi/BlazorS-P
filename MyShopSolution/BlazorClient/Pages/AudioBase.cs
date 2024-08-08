using Microsoft.AspNetCore.Components;

namespace BlazorClient.Pages
{
    public class AudioBase : ProductsListBase
    {
        protected override async Task OnInitializedAsync()
        {
            Category = "Audio";
            await base.OnInitializedAsync();
        }
    }
}
