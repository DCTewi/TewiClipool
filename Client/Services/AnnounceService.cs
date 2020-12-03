using System.Net.Http;
using TewiClipool.Shared;
using TewiClipool.Shared.Models;

namespace TewiClipool.Client.Services
{
    public class AnnounceService : HttpClientServiceBase<AnnounceInfo>
    {
        public AnnounceService(HttpClient http) : base(http)
        {
        }

        protected override string GetApiUrl() => Literal.ApiUrls.Announce;
    }
}
