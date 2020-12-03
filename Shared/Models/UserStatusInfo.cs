using System;
using System.Collections.Generic;
using System.Linq;

namespace TewiClipool.Shared.Models
{
    public class UserStatusInfo
    {
        public string UserName { get; set; }

        public string NickName { get; set; }

        public List<StaffItem> CompletedItems { get; private set; }

        public DateTime LastCompleteTime { get; private set; }

        public StaffType? LastCompleteType { get; private set; }

        public List<StaffItem> LastMonth { get; private set; }

        public UserStatusInfo(string username, string nickname, List<StaffItem> allStaffs)
        {
            UserName = username;
            NickName = nickname;
            CompletedItems = allStaffs
                .Where(s => s.UserName == UserName)
                .OrderByDescending(s => s.CompletedTime)
                .ToList();

            LastMonth = CompletedItems
                    .Where(s => DateTime.Compare(s.CompletedTime.AddMonths(1), DateTime.Now) > 0)
                    .ToList();

            if (CompletedItems.Count == 0)
            {
                LastCompleteTime = DateTime.MinValue;
                LastCompleteType = null;
            }
            else
            {
                var item = CompletedItems.FirstOrDefault();
                LastCompleteTime = item.CompletedTime;
                LastCompleteType = item.Type;
            }
        }
    }
}
