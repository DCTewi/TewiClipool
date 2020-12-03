using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(TewiClipool.Server.Areas.Identity.IdentityHostingStartup))]
namespace TewiClipool.Server.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}