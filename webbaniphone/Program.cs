using Microsoft.EntityFrameworkCore;
using webbaniphone.Data;
using webbaniphone.Models;
using webbaniphone.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. CẤU HÌNH KẾT NỐI DATABASE
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. ĐĂNG KÝ SERVICES (Sửa đoạn này để dùng EF thay vì Mock)
builder.Services.AddControllersWithViews();

// Thay đổi từ Mock... sang EF...
// Dùng AddScoped cho Repository khi làm việc với Database
builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();

var app = builder.Build();

// 3. CẤU HÌNH PIPELINE
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

app.Run();