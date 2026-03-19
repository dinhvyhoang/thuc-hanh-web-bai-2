using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace webbaniphone.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        public string Name { get; set; } = string.Empty;

        [Range(1, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Price { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public List<string> ImageUrls { get; set; } = new List<string>();

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        public int? CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}