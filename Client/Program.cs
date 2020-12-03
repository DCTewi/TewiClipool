using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TewiClipool.Client.Services;

namespace TewiClipool.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Logging.AddConfiguration(
                builder.Configuration.GetSection("Logging"));

            builder.RootComponents.Add<App>("#app");

            void option(HttpClient client) => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
            builder.Services.AddHttpClient<AnnounceService>(option)
                            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
            builder.Services.AddHttpClient<ClipService>(option)
                            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
            builder.Services.AddHttpClient<CommentService>(option)
                            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
            builder.Services.AddHttpClient<RegisterTokenService>(option)
                            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
            builder.Services.AddHttpClient<StaffService>(option)
                            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
            builder.Services.AddHttpClient<UserService>(option)
                            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddScoped(typeof(AccountClaimsPrincipalFactory<RemoteUserAccount>),
                typeof(RolesAccountClaimsPrincipalFactory));
            builder.Services.AddApiAuthorization();

            await builder.Build().RunAsync();
        }
    }
}
