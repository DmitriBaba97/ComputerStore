using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace ComputerStore.Models
{
    public class AppUser : IdentityUser
    {
        [StringLength(40)]
        public string FirstName { get; set; }

        [StringLength(40)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string SecurityQuestion { get; set; }

        [StringLength(50)]
        public string SecurityQuestionAnswer { get; set; }

        public DateTime DateCreated { get; set; }

        public string Photo { get; set; }

        [StringLength(40)]
        public string Country { get; set; }

        [StringLength(40)]
        public string City { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(450)]
        public string AboutMe { get; set; }
        
    }
}
