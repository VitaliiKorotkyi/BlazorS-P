using Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
  // Убедитесь, что это пространство имен правильно
namespace Data // Обновленное пространство имен на Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = Enum.GetValues(typeof(CategoryEnum))
                    .Cast<CategoryEnum>()
                    .Select(e => new Category
                    {
                        CategoryId = Guid.NewGuid(),
                        CategoryName = e.ToString(),
                        CategoryType = e
                    }).ToArray();  // Это правильно

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }
        }
    }
}
