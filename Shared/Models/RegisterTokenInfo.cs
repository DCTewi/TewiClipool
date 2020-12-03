using System.ComponentModel.DataAnnotations;

namespace TewiClipool.Shared.Models
{
    public class RegisterTokenInfo
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Token { get; set; }

        public bool Used { get; set; }

        public string UserName { get; set; }
    }
}
