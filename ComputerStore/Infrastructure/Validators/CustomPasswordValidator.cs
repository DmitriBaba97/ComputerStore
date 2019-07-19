using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ComputerStore.Models;

namespace ComputerStore.Infrastructure.Validators
{
    public class CustomPasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            List<IdentityError> errors = new List<IdentityError>();

            /*------------------Required policies----------------*/
            Regex regex = new Regex(@"[a-z]+");
            //Must have at least one lowercase letter.
            if (!regex.IsMatch(password))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordNoLowercaseLetter",
                    Description = "Password doesn't contain lowercase letters."
                });
            }

            regex = new Regex(@"[0-9]+");
            //Must have at least one digit.
            if (!regex.IsMatch(password))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordNoDigit",
                    Description = "Password doesn't contain digits."
                });
            }

            /*------------------Forbidden policies----------------*/
            regex = new Regex(@"[A-Z]+");
            //Should not have uppercase letters.
            if (regex.IsMatch(password))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordHasUppercaseLetter",
                    Description = "Password cannot contain uppercase letters."
                });
            }

            regex = new Regex(@"[\W_]+");
            //Should not have nonalphanumeric values.
            if (regex.IsMatch(password))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordHasNonAlphaNumericValues",
                    Description = "Password cannot contain nonalphanumeric values."
                });
            }

            return Task.FromResult(errors.Count == 0 ?
                IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
