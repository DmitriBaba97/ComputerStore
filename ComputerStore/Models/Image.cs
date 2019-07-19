using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ComputerStore.Models
{
    public class Image
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool ForPreview { get; set; }

        public long ProductID { get; set; }
        public Product Product { get; set; }
    }
}
