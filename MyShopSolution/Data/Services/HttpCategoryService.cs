using Core.Interface;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Data.Services
{
    public class HttpCategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public HttpCategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Category> AddCategoryAsync(Category category)
        {
            var response = await _httpClient.PostAsJsonAsync("api/categories", category);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Category>();
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/categories/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            try
            {
                Console.WriteLine("Sending request to get categories");
                var response = await _httpClient.GetAsync("api/categories");
                Console.WriteLine("Request completed");
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.Error.WriteLine($"Error fetching categories: {response.StatusCode} - {error}");
                    throw new HttpRequestException($"Error fetching categories: {response.StatusCode}");
                }
                return await response.Content.ReadFromJsonAsync<IEnumerable<Category>>();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching categories: {ex.Message}");
                throw;
            }
        }
        public async Task<Category> GetCategoryByIdAsync(Guid id)
      => await _httpClient.GetFromJsonAsync<Category>($"api/categories/{id}");

        public async Task<Category> GetCategoryByProductIdAsync(Guid productId)
       => await _httpClient.GetFromJsonAsync<Category>($"api/categories/product/{productId}");

        public async Task UpdateCategoryAsync(Category category)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/categories/{category.CategoryId}", category);
            response.EnsureSuccessStatusCode();
        }
    }
}
