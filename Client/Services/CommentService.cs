using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TewiClipool.Shared;
using TewiClipool.Shared.Models;

namespace TewiClipool.Client.Services
{
    public class CommentService : HttpClientServiceBase<Comment>
    {
        public CommentService(HttpClient http) : base(http)
        {
        }

        protected override string GetApiUrl() => Literal.ApiUrls.Comment;

        public async Task<List<Comment>> GetByClipId(int clipid)
        {
            return await http.GetFromJsonAsync<List<Comment>>($"{GetApiUrl()}/clipid/{clipid}");
        }

        public async Task<List<Comment>> GetByUserName(string username)
        {
            return await http.GetFromJsonAsync<List<Comment>>($"{GetApiUrl()}/username/{username}");
        }
    }
}
