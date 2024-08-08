using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Product
    {
        public Guid ProductId { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        [StringLength(2500)]
        public string Description { get; set; }
        [Range(0.01, 1000000.00)]
        public decimal Price { get; set; }
        [Required]
        [Url]
        public string ImageUrl { get; set; }
        [Range(1, 1000)]
        public int Quantity { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
      
        public Category? Category { get; set; }

        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
