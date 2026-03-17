using Microsoft.AspNetCore.Mvc;
using webbaniphone.Models;
using webbaniphone.Repositories;

namespace webbaniphone.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index() => View(await _categoryRepository.GetAllAsync());

        // CHỨC NĂNG THÊM
        [HttpGet] public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.AddAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // CHỨC NĂNG SỬA
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.UpdateAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // CHỨC NĂNG XÓA
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}