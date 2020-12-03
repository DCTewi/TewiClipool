using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TewiClipool.Shared;
using TewiClipool.Shared.Models;

namespace TewiClipool.Client.Services
{
    public class RegisterTokenService : HttpClientServiceBase<RegisterTokenInfo>
    {
        public RegisterTokenService(HttpClient http) : base(http)
        {
        }

        protected override string GetApiUrl() => Literal.ApiUrls.RegisterToken;

        public async Task CreateRandom(int count)
        {
            _ = await http.GetFromJsonAsync<bool>($"{GetApiUrl()}/random/{count}");
        }
    }
}
