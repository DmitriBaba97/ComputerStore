using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ComputerStore.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter your first name.")]
        [StringLength(40)]
        public string FirstName { get; set; }

        [StringLength(40)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your username.")]
        [Remote("ValidateUsername", "Account")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your email.")]
        [UIHint("EmailAddress")]
        [Remote("ValidateEmail", "Account")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your password.")]
        [UIHint("Password")]
        [Remote("ValidatePassword", "Account", AdditionalFields = nameof(Username))]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [UIHint("Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please select secret question.")]
        public string SecurityQuestion { get; set; }

        [Required(ErrorMessage = "Please give your answer on secret question.")]
        [StringLength(40)]
        public string SecurityQuestionAnswer { get; set; }
    }
}
