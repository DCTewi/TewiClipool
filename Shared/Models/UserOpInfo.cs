using System.ComponentModel.DataAnnotations;

namespace TewiClipool.Shared.Models
{
    public struct UserOpInfo
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public bool IsAdd { get; set; }
    }
}
