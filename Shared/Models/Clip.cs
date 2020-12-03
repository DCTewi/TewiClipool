using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TewiClipool.Shared.Models
{
    public class Clip
    {
        public int Id { get; set; }

        [Required]
        public ClipType Type { get; set; } = ClipType.LiveClip;

        [Required]
        public PriorityType Priority { get; set; } = PriorityType.Normal;

        public DateTime CreateTime { get; set; } = DateTime.Now;

        public string CreateUserName { get; set; }

        [Required, MaxLength(30)]
        public string Title { get; set; }

        [Required, MaxLength(30), RegularExpression("^[0-9]{8}.*$")]
        public string OriginInfo { get; set; }

        [MaxLength(233)]
        public string Description { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int Length { get; set; }

        [Url]
        public string PublishAddress { get; set; }

        [JsonIgnore]
        public List<StaffItem> StaffItems { get; set; } = new List<StaffItem>();

        [JsonIgnore]
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
