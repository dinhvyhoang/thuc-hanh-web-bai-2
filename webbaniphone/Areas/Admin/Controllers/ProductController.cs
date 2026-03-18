using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using webbaniphone.Models;
using webbaniphone.Repositories;

namespace webbaniphone.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }

        public async Task<IActionResult> Add()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile imageUrl, List<IFormFile> images)
        {
            // Kiểm tra thủ công để ngăn chặn giá trị 0 xuống Database
            if (product.CategoryId == null || product.CategoryId == 0)
            {
                ModelState.AddModelError("CategoryId", "Vui lòng chọn danh mục hợp lệ.");
            }

            if (ModelState.IsValid)
            {
                if (imageUrl != null) product.ImageUrl = await SaveImage(imageUrl);

                if (images != null && images.Count > 0)
                {
                    foreach (var file in images)
                    {
                        product.ImageUrls.Add(await SaveImage(file));
                    }
                }

                await _productRepository.AddAsync(product);
                return RedirectToAction("Index");
            }

            await LoadCategories(); // Nạp lại danh mục khi có lỗi nhập liệu
            return View(product);
        }

        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();

            await LoadCategories();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Product product, IFormFile imageUrl, List<IFormFile> images)
        {
            if (id != product.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var existingProduct = await _productRepository.GetByIdAsync(id);
                if (existingProduct == null) return NotFound();

                product.ImageUrl = (imageUrl != null) ? await SaveImage(imageUrl) : existingProduct.ImageUrl;

                if (images != null && images.Count > 0)
                {
                    product.ImageUrls = new List<string>();
                    foreach (var file in images)
                    {
                        product.ImageUrls.Add(await SaveImage(file));
                    }
                }
                else
                {
                    product.ImageUrls = existingProduct.ImageUrls;
                }

                await _productRepository.UpdateAsync(product);
                return RedirectToAction("Index");
            }

            await LoadCategories();
            return View(product);
        }

        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var savePath = Path.Combine(folderPath, fileName);

            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + fileName;
        }
    }
}
