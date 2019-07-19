using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ComputerStore.Models
{
    public class FilterOption
    {
        public FilterOption() {
            Values = new HashSet<FilterOptionValue>();
        }

        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public long SubcategoryID { get; set; }
        public Subcategory Subcategory { get; set; }

        [JsonIgnore]
        public ICollection<FilterOptionValue> Values { get; set; }
    }
}
