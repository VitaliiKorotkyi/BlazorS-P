using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Models
{
 public class Category
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
      
        public ICollection<Product> Products { get; set; }
        public CategoryEnum CategoryType { get; set; }
    }
}
