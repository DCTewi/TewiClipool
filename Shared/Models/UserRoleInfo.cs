using System.ComponentModel.DataAnnotations;

namespace TewiClipool.Shared.Models
{
    public struct UserRoleInfo
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public StaffType StaffType { get; set; }
        [Required]
        public bool IsAdd { get; set; }
    }
}
