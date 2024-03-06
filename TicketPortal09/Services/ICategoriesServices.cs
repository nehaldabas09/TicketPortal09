using System.Collections.Generic;
using System.Threading.Tasks;
using TicketPortal09.Models;

namespace TicketPortal09.Services
{
    public interface ICategoriesService
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task<bool> CreateCategoryAsync(Category category);
        Task<bool> UpdateCategoryAsync(int id, Category category);
        Task<bool> DeleteCategoryAsync(int id);
        bool CategoryExists(int id);
    }
}
