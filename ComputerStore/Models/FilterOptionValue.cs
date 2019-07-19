using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ComputerStore.Models
{
    public class FilterOptionValue
    {
        [Key]
        public long Id { get; set; }
 
        public string Value { get; set; }

        public long FilterOptionID { get; set; }
        public FilterOption FilterOption { get; set; }
    }
}
