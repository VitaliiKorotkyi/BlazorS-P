
using Core.Interface;
using Core.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorClient.Pages
{
    public class AddProductBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; } = null!;

        [Inject]
        public ICategoryService CategoryService { get; set; } = null!;

        [Inject]
        public NavigationManager Navigation { get; set; } = null!;

        protected Product Product { get; set; } = new Product();
        protected List<Category> Categories { get; set; } = new List<Category>();

        private bool isSubmitting = false;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Console.WriteLine("Initializing AddProduct component");
                Categories = (await CategoryService.GetCategoriesAsync()).ToList();
                if (Categories != null && Categories.Any())
                {
                    Console.WriteLine("Categories loaded successfully.");
                }
                else
                {
                    Console.WriteLine("No categories found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading categories: {ex.Message}");
            }
        }

        protected async Task HandleValidSubmit()
        {
            if (isSubmitting)
            {
                Console.WriteLine("Already submitting, skipping...");
                return;
            }

            isSubmitting = true;

            try
            {
                Console.WriteLine("Submitting product...");
                await ProductService.AddProductAsync(Product);
                Console.WriteLine("Product submitted successfully");
                Navigation.NavigateTo("/admin/products");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error adding product: {ex.Message}");
            }
            finally
            {
                isSubmitting = false;
            }



        }

        private void NavigateToProduct(Guid productId)
        {
            Navigation.NavigateTo($"/product/{productId}");
        }


    }
}

