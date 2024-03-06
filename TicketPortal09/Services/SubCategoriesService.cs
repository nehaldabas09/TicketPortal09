using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketPortal09.Data;
using TicketPortal09.Models;

namespace TicketPortal09.Services
{
    
    public class SubcategoriesService : ISubCategoriesService
    {
        private readonly TicketDbContext _context;

        public SubcategoriesService(TicketDbContext context)
        {
            _context = context;
        }

        public async Task<List<SubCategory>> GetAllSubCategoriesAsync()
        {
            return await _context.Subcategories.Include(s => s.Category).ToListAsync();
        }

        public async Task<SubCategory> GetSubCategoryByIdAsync(int id)
        {
            return await _context.Subcategories.FindAsync(id);
        }

        public async Task CreateSubCategoryAsync(SubCategory subCategory)
        {
            _context.Add(subCategory);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSubCategoryAsync(SubCategory subCategory)
        {
            _context.Update(subCategory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubCategoryAsync(int id)
        {
            var subCategory = await _context.Subcategories.FindAsync(id);
            if (subCategory != null)
            {
                _context.Subcategories.Remove(subCategory);
                await _context.SaveChangesAsync();
            }
        }
    }
}
