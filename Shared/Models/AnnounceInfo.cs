using System;
using System.ComponentModel.DataAnnotations;

namespace TewiClipool.Shared.Models
{
    public class AnnounceInfo
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        [Required, MaxLength(30)]
        public string Title { get; set; }

        [Required]
        public string RawHtml { get; set; }

        public DateTime PublishTime { get; set; }

        public bool IsTopped { get; set; }
    }
}
