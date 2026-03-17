using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace webbaniphone.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(50, ErrorMessage = "Tên danh mục không quá 50 ký tự")]
        public string Name { get; set; }

        // Thuộc tính quan hệ: Một danh mục có thể có nhiều sản phẩm
        public List<Product>? Products { get; set; }

        // Constructor để khởi tạo danh sách tránh lỗi NullReferenceException
        public Category()
        {
            Products = new List<Product>();
        }
    }
}