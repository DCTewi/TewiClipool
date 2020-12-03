using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TewiClipool.Shared;
using TewiClipool.Shared.Models;

namespace TewiClipool.Client.Services
{
    public class StaffService : HttpClientServiceBase<StaffItem>
    {
        public StaffService(HttpClient http) : base(http)
        {
        }

        protected override string GetApiUrl() => Literal.ApiUrls.Staff;

        public async Task<List<StaffItem>> GetByClipId(int clipid)
        {
            return await http.GetFromJsonAsync<List<StaffItem>>($"{GetApiUrl()}/clip/{clipid}");
        }

        public async Task<bool> ToggleSageAsync(int id)
        {
            return await http.GetFromJsonAsync<bool>($"{GetApiUrl()}/sage/{id}");
        }
    }
}
