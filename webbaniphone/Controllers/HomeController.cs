using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using webbaniphone.Models;
using webbaniphone.Repositories; // Nhớ thêm dòng này để gọi Repository

namespace webbaniphone.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    // Khai báo thêm Repository để lấy dữ liệu sản phẩm
    private readonly IProductRepository _productRepository;

    // Inject ProductRepository vào Constructor
    public HomeController(ILogger<HomeController> logger, IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }

    public async Task<IActionResult> Index()
    {
        // Lấy toàn bộ sản phẩm từ database để hiện lên trang chủ
        var products = await _productRepository.GetAllAsync();

        // Truyền danh sách products vào View
        return View(products);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}