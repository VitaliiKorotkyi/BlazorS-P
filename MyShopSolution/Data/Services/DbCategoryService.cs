using Core.Interface;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class DbCategoryService :ICategoryService
    {
        private readonly ApplicationDbContext _context;
        public DbCategoryService(ApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Category> AddCategoryAsync(Category category)
        {
           
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category is null)
            {
                return false;
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(Guid id)
        => await _context.Categories.FindAsync(id);


        public async Task<Category> GetCategoryByProductIdAsync(Guid productId)
        {

            var product = await _context.Products.Include(ca => ca.Category).FirstOrDefaultAsync(i => i.ProductId == productId);
            return product?.Category;
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            // Найти существующую категорию по её идентификатору
            var existingCategory = await _context.Categories.FindAsync(category.CategoryId);

            // Если категория найдена, обновите её свойства
            if (existingCategory != null)
            {
                existingCategory.CategoryName = category.CategoryName;
                existingCategory.CategoryType = category.CategoryType;

                // Обновить состояние категории в контексте до Modified
                _context.Entry(existingCategory).State = EntityState.Modified;

                // Сохранить изменения в базе данных
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Category not found.");
            }
        }
    }
}