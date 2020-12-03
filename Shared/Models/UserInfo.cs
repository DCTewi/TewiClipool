using System.ComponentModel.DataAnnotations;

namespace TewiClipool.Shared.Models
{
    public class UserInfo
    {
        public string Id { get; set; }

        public string UserName { get; set; }

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
        public bool CheckRole(StaffType type) => type switch
        {
            StaffType.Editor => IsEditor,
            StaffType.Translator => IsTranslator,
            StaffType.Timeline => IsTimeline,
            StaffType.Reviewer => IsReviewer,
            StaffType.PostProducer => IsPostProducer,
            StaffType.Art => IsArt,
            _ => false,
        };
    }
}
