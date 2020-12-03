using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TewiClipool.Server.Models;

namespace TewiClipool.Server.Services
{
    public class UserNameValidator<TUser> : IUserValidator<TUser> where TUser : AppUser
    {
        private static readonly Regex allowedRegex = new Regex(@"\p{IsBasicLatin}|\p{IsCJKUnifiedIdeographs}");

        public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            if (user.UserName.Contains(' '))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "InvalidCharactersUsername",
                    Description = "Username can only contain BasicLatin and CJK"
                }));
            }

            if (allowedRegex.IsMatch(user.UserName))
            {
                return Task.FromResult(IdentityResult.Success);
            }
            else
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "InvalidCharactersUsername",
                    Description = "Username can only contain BasicLatin and CJK"
                }));
            }
        }
    }
}
