using Core.Interface;
using Core.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorClient.Pages
{
    public class ViewProductInfoBase : ComponentBase
    {

        [Inject]
        public IProductService ProductService { get; set; } = null!;

        [Inject]
        public NavigationManager Navigation { get; set; } = null!;

        [Parameter]
        public Guid ProductId { get; set; }

        protected Product product;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                product = await ProductService.GetProductByIdAsync(ProductId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading product: {ex.Message}");
            }
        }

        protected void GoBack()
        {
            Navigation.NavigateTo("/products");
        }
    }

}

