using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace TewiClipool.Shared.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public CommentType Type { get; set; } = CommentType.Chat;

        [Required, MaxLength(233)]
        public string Content { get; set; }

        public DateTime PublishTime { get; set; }

        public bool Edited { get; set; } = false;

        [Required]
        public int ClipId { get; set; }
        [JsonIgnore]
        public Clip Clip { get; set; }
    }
}
