using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ComputerStore.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter username.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter password.")]
        [UIHint("Password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
