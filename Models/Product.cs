using System.ComponentModel.DataAnnotations;

namespace SmartInventoryManagement.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = "";

        [Required]
        public string Category { get; set; } = "";

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string Quality { get; set; } = "";

        [Required]
        public decimal Price { get; set; }
    }
}