using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TewiClipool.Server.Data;
using TewiClipool.Server.Models;

namespace TewiClipool.Server.Controllers
{
    public abstract class DbControllerBase<TModel> : DbControllerBase<TModel, int>
    {
        protected DbControllerBase(ApplicationDbContext db,
            ILogger<DbControllerBase<TModel, int>> logger,
            HtmlEncoder htmlEncoder) : base(db, logger, htmlEncoder)
        {
        }
    }

    public abstract class DbControllerBase<TModel, TKey> : Controller
    {
        protected readonly ApplicationDbContext db;
        protected readonly ILogger<DbControllerBase<TModel, TKey>> logger;
        protected readonly HtmlEncoder htmlEncoder;

        public DbControllerBase(ApplicationDbContext db,
            ILogger<DbControllerBase<TModel, TKey>> logger,
            HtmlEncoder htmlEncoder)
        {
            this.db = db;
            this.logger = logger;
            this.htmlEncoder = htmlEncoder;
        }

        protected string GetUserId() =>
            HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        protected ValueTask<AppUser> GetUser() =>
            db.Users.FindAsync(GetUserId());

        protected async Task<string> GetUserName() =>
            (await GetUser())?.UserName;

        protected string SafeEncode(string raw) =>
            htmlEncoder.Encode(raw.Trim());

        public abstract Task<ActionResult<List<TModel>>> Get();

        public abstract Task<ActionResult<TModel>> Get(TKey id);

        public abstract Task<ActionResult<TKey>> Create(TModel model);

        public abstract Task<ActionResult> Update(TModel model);

        public abstract Task<ActionResult> Delete(TKey id);
    }
}
