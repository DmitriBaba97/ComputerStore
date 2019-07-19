using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerStore.Models
{
    public class ProductProperty
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public long ProductID { get; set; }
        public Product Product { get; set; }
    }
}
