using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ComputerStore.Models
{
    public class Category
    {
        public Category()
        {
            Subcategories = new HashSet<Subcategory>();
            Products = new HashSet<Product>();
        }

        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "Please enter category name.")]
        [StringLength(maximumLength:50)]
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Subcategory> Subcategories { get; set; }

        [JsonIgnore]
        public ICollection<Product> Products { get; set; }
    }
}
