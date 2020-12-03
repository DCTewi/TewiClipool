using System;
using System.ComponentModel.DataAnnotations;
using TewiClipool.Shared.Models;

namespace TewiClipool.Server.Models
{
    public class RegisterToken
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Token { get; set; }

        public bool Used { get; set; } = false;

        public AppUser AppUser { get; set; }

        public RegisterTokenInfo ToInfo() =>
            new RegisterTokenInfo
            {
                Id = Id,
                Token = Token,
                Used = Used,
                UserName = AppUser?.UserName
            };

        public static RegisterToken New()
        {
            return new RegisterToken
            {
                Token = Guid.NewGuid().ToString(),
                Used = false,
            };
        }

        public static RegisterToken New(string token)
        {
            return new RegisterToken
            {
                Token = token,
                Used = false,
            };
        }
    }
}
