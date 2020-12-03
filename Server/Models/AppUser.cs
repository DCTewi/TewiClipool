using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using TewiClipool.Shared.Models;

namespace TewiClipool.Server.Models
{
    public class AppUser : IdentityUser
    {
        [Required, MaxLength(30)]
        public string NickName { get; set; }

        public bool IsOperator { get; set; }
        public bool IsUser { get; set; }
        public bool IsEditor { get; set; }
        public bool IsTranslator { get; set; }
        public bool IsTimeline { get; set; }
        public bool IsReviewer { get; set; }
        public bool IsPostProducer { get; set; }
        public bool IsArt { get; set; }

        public int RegisterTokenId { get; set; }
        public RegisterToken RegisterToken { get; set; }

        public UserInfo ToUserInfo() =>
            new UserInfo
            {
                Id = Id,
                UserName = UserName,
                NickName = NickName,
                IsOperator = IsOperator,
                IsUser = IsUser,
                IsEditor = IsEditor,
                IsTranslator = IsTranslator,
                IsTimeline = IsTimeline,
                IsReviewer = IsReviewer,
                IsPostProducer = IsPostProducer,
                IsArt = IsArt
            };
    }
}
