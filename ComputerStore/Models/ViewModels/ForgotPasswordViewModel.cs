using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ComputerStore.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Please enter your username.")]
        [StringLength(maximumLength:40, ErrorMessage = "This field must be max 40 chars length")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please answer the secret question.")]
        [StringLength(maximumLength: 40, ErrorMessage = "This field must be max 40 chars length")]
        public string SecurityQuestionAnswer { get; set; }

        [Required(ErrorMessage = "Please enter your new password.")]
        [Remote("ValidatePassword", "Account", AdditionalFields = nameof(Username))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your new password.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
