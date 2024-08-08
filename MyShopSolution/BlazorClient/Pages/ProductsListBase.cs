//using BlazorSpinner;
//using Core.Interface;
//using Core.Models;
//using Microsoft.AspNetCore.Components;

//namespace BlazorClient.Pages
//{
//    public class ProductsListBase : ComponentBase
//    {
//        [Inject]
//        protected IProductService ProductService { get; set; }

//        [Parameter]
//        public string? Category { get; set; }

//        protected IEnumerable<Product> products;
//        protected bool isLoading = true;
//        protected string errorMessage;
//        protected int currentPage = 1;
//        protected int pageSize = 6;
//        protected int TotalPages => (products.Count() + pageSize - 1) / pageSize;

//        protected bool IsPreviousDisabled => currentPage == 1;
//        protected bool IsNextDisabled => currentPage == TotalPages;

//        protected override async Task OnInitializedAsync()
//        {
//            Console.WriteLine("OnInitializedAsync called");
//            await LoadProducts();
//        }

//        protected override async Task OnParametersSetAsync()
//        {
//            Console.WriteLine("OnParametersSetAsync called");
//            await LoadProducts();
//        }

//        protected async Task LoadProducts()
//        {
//            Console.WriteLine("LoadProducts called.");
//            isLoading = true;
//            errorMessage = null;

//            try
//            {
//                var allProducts = await ProductService.GetProductsAsync();
//                if (!string.IsNullOrEmpty(Category))
//                {
//                    products = allProducts.Where(p => p.Category.CategoryName == Category);
//                }
//                else
//                {
//                    products = allProducts;
//                }

//                var categories = await ProductService.GetCategoriesAsync();
//                var categoryDict = categories.ToDictionary(c => c.CategoryId, c => c);

//                foreach (var product in products)
//                {
//                    if (categoryDict.TryGetValue(product.CategoryId, out var category))
//                    {
//                        product.Category = category;
//                    }
//                }

//                Console.WriteLine("Products and categories assigned successfully.");
//            }
//            catch (Exception ex)
//            {
//                errorMessage = $"Error loading products: {ex.Message}";
//                Console.WriteLine($"Error: {ex.Message}");
//            }
//            finally
//            {
//                isLoading = false;
//                Console.WriteLine("Loading finished.");
//                StateHasChanged();
//            }
//        }

//        protected string TruncateDescription(string description)
//        {
//            if (string.IsNullOrWhiteSpace(description))
//            {
//                return string.Empty;
//            }

//            var words = description.Split(' ');
//            if (words.Length <= 15)
//            {
//                return description;
//            }

//            return string.Join(' ', words.Take(15)) + "...";
//        }

//        protected void NextPage()
//        {
//            if (currentPage < TotalPages)
//            {
//                currentPage++;
//                Console.WriteLine("Next page: " + currentPage);
//                StateHasChanged();
//            }
//        }

//        protected void PreviousPage()
//        {
//            if (currentPage > 1)
//            {
//                currentPage--;
//                Console.WriteLine("Previous page: " + currentPage);
//                StateHasChanged();
//            }
//        }
//    }
//}
using BlazorSpinner;
using Core.Interface;
using Core.Models;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace BlazorClient.Pages
{
    public class ProductsListBase : ComponentBase
    {
        [Inject]
        protected IProductService ProductService { get; set; }

        [Parameter]
        public string? Category { get; set; }

        protected IEnumerable<Product> products;
        protected bool isLoading = true;
        protected string errorMessage;
        protected int currentPage = 1;
        protected int pageSize = 6;
        protected int TotalPages => (products.Count() + pageSize - 1) / pageSize;

        protected bool IsPreviousDisabled => currentPage == 1;
        protected bool IsNextDisabled => currentPage == TotalPages;

        protected override async Task OnParametersSetAsync()
        {
            Console.WriteLine("OnParametersSetAsync called");
            await LoadProducts();
        }

        protected async Task LoadProducts()
        {
            Console.WriteLine("LoadProducts called.");
            isLoading = true;
            errorMessage = null;

            try
            {
                var allProducts = await ProductService.GetProductsAsync();
                if (!string.IsNullOrEmpty(Category))
                {
                    products = allProducts.Where(p => p.Category.CategoryName == Category).ToList();
                }
                else
                {
                    products = allProducts.ToList();
                }

                Console.WriteLine("Products loaded successfully.");
            }
            catch (Exception ex)
            {
                errorMessage = $"Error loading products: {ex.Message}";
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                isLoading = false;
                Console.WriteLine("Loading finished.");
                StateHasChanged();
            }
        }

        protected string TruncateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return string.Empty;
            }

            var words = description.Split(' ');
            if (words.Length <= 15)
            {
                return description;
            }

            return string.Join(' ', words.Take(15)) + "...";
        }

        protected void NextPage()
        {
            if (currentPage < TotalPages)
            {
                currentPage++;
                Console.WriteLine("Next page: " + currentPage);
                StateHasChanged();
            }
        }

        protected void PreviousPage()
        {
            if (currentPage > 1)
            {
                currentPage--;
                Console.WriteLine("Previous page: " + currentPage);
                StateHasChanged();
            }
        }
    }
}
