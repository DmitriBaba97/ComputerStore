using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ComputerStore.Models
{
    public class Product
    { 
        public Product()
        {
            Properties = new List<ProductProperty>();
            Images = new List<Image>();
        }

        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "Please specify product name.")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please specify product price.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal StockPrice { get; set; }

        public int AmountInStock { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        public long CategoryID { get; set; }
        public Category Category { get; set; }

        public long SubcategoryID { get; set; }
        public Subcategory Subcategory { get; set; }

        [JsonIgnore]
        public IList<ProductProperty> Properties { get; set; }
        [JsonIgnore]
        public IList<Image> Images { get; set; }
    }
}
