using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketPortal09.Data;
using TicketPortal09.Models;

namespace TicketPortal09.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly TicketDbContext _context;

        public CategoriesService(TicketDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<bool> CreateCategoryAsync(Category category)
        {
            if (category == null)
                return false;

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCategoryAsync(int id, Category category)
        {
            if (id != category.CategoryId)
                return false;

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                    return false;
                else
                    throw;
            }

            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}
