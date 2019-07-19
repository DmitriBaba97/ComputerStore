using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ComputerStore.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ComputerStore.Models
{
    public class Order
    {
        [BindNever]
        public long Id { get; set; }

        [Required(ErrorMessage = "Please enter your full name.")]
        [StringLength(50)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter your email.")]
        [StringLength(70)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your phone number.")]
        [StringLength(30)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please select your country.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Please enter your city.")]
        [StringLength(40)]
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter your address.")]
        [StringLength(70)]
        public string Address { get; set; }

        [BindNever]
        public ICollection<CartLine> Lines { get; set; }

        [BindNever]
        public DateTime DateCreated { get; set; }

        [BindNever]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
    }
}
