using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ComputerStore.Models
{
    public class Subcategory
    {
        public Subcategory ()
        {
            Products = new HashSet<Product>();
            FilterOptions = new HashSet<FilterOption>();
        }

        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "Please specify subcategory name.")]
        [StringLength(maximumLength:50)]
        public string Name { get; set; }

        public long CategoryID { get; set; }
        public Category Category { get; set; }

        [JsonIgnore]
        public ICollection<Product> Products { get; set; }
        [JsonIgnore]
        public ICollection<FilterOption> FilterOptions { get; set; }
    }
}
