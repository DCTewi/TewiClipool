using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TewiClipool.Shared;
using TewiClipool.Shared.Models;

namespace TewiClipool.Client.Services
{
    public class UserService : HttpClientServiceBase<UserInfo, string>
    {
        public UserService(HttpClient http) : base(http)
        {
        }

        protected override string GetApiUrl() => Literal.ApiUrls.User;

        private struct UserNameInfo
        {
            public UserInfo UserInfo { get; set; }
            public DateTime FetchTime { get; set; }

            public UserNameInfo(UserInfo info)
            {
                UserInfo = info;
                FetchTime = DateTime.Now;
            }

            public bool IsExpired() => DateTime.Compare(FetchTime.AddMinutes(5), DateTime.Now) < 0;
        }

        private static readonly Dictionary<string, UserNameInfo> UserPool = new Dictionary<string, UserNameInfo>();

        public async Task<UserInfo> GetByUserName(string username)
        {
            if (UserPool.ContainsKey(username) && !UserPool[username].IsExpired())
            {
                return UserPool[username].UserInfo;
            }

            UserPool[username] = new UserNameInfo(
                await http.GetFromJsonAsync<UserInfo>($"{GetApiUrl()}/username/{username}"));

            return UserPool[username].UserInfo;
        }

        [Authorize(Roles = Literal.Roles.Operator)]
        public async Task<bool> ConfigRole(string username, StaffType type, bool isAdd)
        {
            var response = await http.PutAsJsonAsync($"{GetApiUrl()}/role", new UserRoleInfo
            {
                UserName = username,
                StaffType = type,
                IsAdd = isAdd
            });
            return response.IsSuccessStatusCode;
        }

        [Authorize(Roles = Literal.Roles.Admin)]
        public async Task<bool> ConfigOp(string username, bool isAdd)
        {
            var response = await http.PutAsJsonAsync($"{GetApiUrl()}/op", new UserOpInfo
            {
                UserName = username,
                IsAdd = isAdd
            });

            return response.IsSuccessStatusCode;
        }

        [Authorize(Roles = Literal.Roles.Operator)]
        public async Task ToggleBanByUsername(string username)
        {
            _ = await http.GetFromJsonAsync<bool>($"{GetApiUrl()}/ban/{username}");
        }

        [Authorize(Roles = Literal.Roles.Admin)]
        public async Task<bool> ResetPassword(string username)
        {
            var response = await http.GetFromJsonAsync<bool>($"{GetApiUrl()}/reset/{username}");

            return response;
        }

        [Authorize(Roles = Literal.Roles.Operator)]
        public async Task<string> GetEmail(string username)
        {
            var response = await http.GetStringAsync($"{GetApiUrl()}/mail/{username}");

            return response;
        }
    }
}
