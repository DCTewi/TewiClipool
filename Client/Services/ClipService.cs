using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TewiClipool.Shared;
using TewiClipool.Shared.Models;

namespace TewiClipool.Client.Services
{
    public class ClipService : HttpClientServiceBase<Clip>
    {
        public ClipService(HttpClient http) : base(http)
        {
        }

        protected override string GetApiUrl() => Literal.ApiUrls.Clip;

        public async Task<List<Clip>> GetByUserName(string username)
        {
            return await http.GetFromJsonAsync<List<Clip>>($"{GetApiUrl()}/username/{username}");
        }
    }
}
