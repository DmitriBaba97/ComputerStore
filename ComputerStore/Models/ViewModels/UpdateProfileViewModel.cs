using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ComputerStore.Models.ViewModels
{
    public class UpdateProfileViewModel
    {
        public string Id { get; set; }

        [StringLength(40)]
        public string FirstName { get; set; }

        [StringLength(40)]
        public string LastName { get; set; }

        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(30)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Photo { get; set; }

        [StringLength(40)]
        public string Country { get; set; }

        [StringLength(40)]
        public string City { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(450)]
        [DataType(DataType.MultilineText)]
        public string AboutMe { get; set; }

        public string SecurityStamp { get; set; }
    }
}
