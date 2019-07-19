using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using ComputerStore.Models;

namespace ComputerStore.Infrastructure.Validators
{
    public class CustomUserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            List<IdentityError> errors = new List<IdentityError>();

            /*------------------Required policies----------------*/
            Regex regex = new Regex(@"[a-z]+");
            if (!regex.IsMatch(user.UserName))
            {
                errors.Add(new IdentityError
                {
                    Code = "UsernameNoLowercaseLetter",
                    Description = "Username has no lowercase letter."
                });
            }

            regex = new Regex(@"[0-9]+");
            if (!regex.IsMatch(user.UserName))
            {
                errors.Add(new IdentityError
                {
                    Code = "UsernameNoDigit",
                    Description = "Username has no digit."
                });
            }

            /*------------------Forbidden policies----------------*/
            regex = new Regex(@"[\W_]+");
            //Should not have nonalphanumeric values.
            if (regex.IsMatch(user.UserName))
            {
                errors.Add(new IdentityError
                {
                    Code = "UsernameHasNonAlphaNumericValues",
                    Description = "Username cannot contain nonalphanumeric values."
                });
            }

            return Task.FromResult(errors.Count == 0 ?
                IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
