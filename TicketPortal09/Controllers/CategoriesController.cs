using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TicketPortal09.Data;
using TicketPortal09.Models;
using TicketPortal09.Services;

namespace TicketPortal09.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _categoriesService;
        private readonly ILogger<CategoriesController> _logger;


        public CategoriesController(ICategoriesService categoriesService, ILogger<CategoriesController> logger)
        {
            _categoriesService = categoriesService;
            _logger = logger;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            try
            {

                var categories = await _categoriesService.GetCategoriesAsync();
                return View(categories);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {

                if (id == null)
                {
                    return NotFound();
                }

                var category = await _categoriesService.GetCategoryByIdAsync(id.Value);
                if (category == null)
                {
                    return NotFound();
                }

                return View(category);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            try
            {

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,Name")] Category category)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    await _categoriesService.CreateCategoryAsync(category);
                    return RedirectToAction(nameof(Index));
                }
                return View(category);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {

                if (id == null)
                {
                    return NotFound();
                }

                var category = await _categoriesService.GetCategoryByIdAsync(id.Value);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,Name")] Category category)
        {

            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _categoriesService.UpdateCategoryAsync(id, category);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_categoriesService.CategoryExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {

                if (id == null)
                {
                    return NotFound();
                }

                var category = await _categoriesService.GetCategoryByIdAsync(id.Value);
                if (category == null)
                {
                    return NotFound();
                }

                return View(category);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {

                await _categoriesService.DeleteCategoryAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }
    }
}
