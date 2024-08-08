using Core.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorClient.Pages
{
    public class DiscountedProductsBase:ProductsList
    {
      
    protected override async Task OnInitializedAsync()
        {
            Category = "DiscountedProducts";
            await base.OnInitializedAsync();
        }
    }

}

