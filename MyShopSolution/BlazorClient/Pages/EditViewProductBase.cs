

using Core.Interface;
using Core.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace BlazorClient.Pages
{
    public class EditViewProductBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; } = null!;

        [Inject]
        public NavigationManager Navigation { get; set; } = null!;

        [Parameter]
        public Guid ProductId { get; set; }

        protected Product product = new Product();

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

        protected async Task HandleValidSubmit()
        {
            try
            {
                if (product.ProductId == Guid.Empty)
                {
                    await ProductService.AddProductAsync(product);
                }
                else
                {
                    await ProductService.UpdateProductAsync(product);
                }
                Navigation.NavigateTo("/products");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving product: {ex.Message}");
            }
        }

        protected void GoBack()
        {
            Navigation.NavigateTo("/products");
        }
    }
}
