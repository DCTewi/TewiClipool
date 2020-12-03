using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace TewiClipool.Shared.Models
{
    public class StaffItem
    {
        public int Id { get; set; }

        [Required]
        public StaffType Type { get; set; }

        [Required, MaxLength(30)]
        public string Description { get; set; }

        [Required, MaxLength(30)]
        public string StaffNickName { get; set; }

        public string UserName { get; set; }

        public ProcessType Process { get; set; } = ProcessType.Accepted;

        public DateTime AcceptTime { get; set; }

        public DateTime CompletedTime { get; set; }

        public string ResAddress { get; set; }

        public bool Saged { get; set; } = false;

        public string SagedUser { get; set; }

        [Required]
        public int ClipId { get; set; }
        [JsonIgnore]
        public Clip Clip { get; set; }

        public void SetCompleted()
        {
            Process = ProcessType.Completed;
            CompletedTime = DateTime.Now;
        }

        public void SetCompleted(string resource)
        {
            Process = ProcessType.Completed;
            CompletedTime = DateTime.Now;
            ResAddress = resource;
        }

        public void SetGaveUp()
        {
            Process = ProcessType.GaveUp;
            ResAddress = "";
        }

        public void SetAccepted()
        {
            Process = ProcessType.Accepted;
            ResAddress = "";
        }

        public void ToggleSage()
        {
            Saged = !Saged;
        }
    }
}
