using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TewiClipool.Server.Data;
using TewiClipool.Server.Models;
using TewiClipool.Shared;
using TewiClipool.Shared.Models;

namespace TewiClipool.Server.Controllers
{
    [ApiController]
    [Route(Literal.ApiUrls.RegisterToken)]
    [Authorize(Roles = Literal.Roles.Operator)]
    public class RegisterTokenController : DbControllerBase<RegisterTokenInfo>
    {
        public RegisterTokenController(ApplicationDbContext db,
            ILogger<DbControllerBase<RegisterTokenInfo, int>> logger,
            HtmlEncoder htmlEncoder) : base(db, logger, htmlEncoder)
        {
        }

        [HttpPost]
        public override async Task<ActionResult<int>> Create(RegisterTokenInfo model)
        {
            if (!(await GetUser()).IsOperator) return Forbid();

            var used = db.RegisterTokens
                .FirstOrDefault(t => t.Token == model.Token) != null;

            if (used) return BadRequest();

            var token = await db.RegisterTokens.AddAsync(RegisterToken.New(model.Token));
            await db.SaveChangesAsync();

            return Ok(token.Entity.Id);
        }

        [HttpGet("random/{count}")]
        public async Task<ActionResult<bool>> CreateRandom(int count)
        {
            if (!(await GetUser()).IsOperator) return Forbid();

            for (int i = 0; i < count; i++)
            {
                await db.RegisterTokens.AddAsync(RegisterToken.New());
            }
            await db.SaveChangesAsync();

            return Ok(true);
        }

        [HttpDelete("id/{id}")]
        public override async Task<ActionResult> Delete(int id)
        {
            if (!(await GetUser()).IsOperator) return Forbid();

            var token = await db.RegisterTokens
                .Where(t => t.Id == id)
                .SingleOrDefaultAsync();

            if (token == null)
            {
                return NotFound();
            }

            db.Remove(token);
            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public override async Task<ActionResult<List<RegisterTokenInfo>>> Get()
        {
            if (!(await GetUser()).IsOperator) return Forbid();

            var tokens = await db.RegisterTokens.ToListAsync();
            List<RegisterTokenInfo> result = new List<RegisterTokenInfo>();

            tokens.ForEach(t =>
            {
                t.AppUser = db.Users.Where(u => u.RegisterTokenId == t.Id).SingleOrDefault();
                result.Add(t.ToInfo());
            });

            return Ok(result);
        }

        [HttpGet("id/{id}")]
        public override async Task<ActionResult<RegisterTokenInfo>> Get(int id)
        {
            if (!(await GetUser()).IsOperator) return Forbid();

            var token = await db.RegisterTokens
                .Where(t => t.Id == id)
                .SingleOrDefaultAsync();

            token.AppUser = db.Users.Where(u => u.RegisterTokenId == token.Id).SingleOrDefault();
            return token == null ? NotFound() : Ok(token.ToInfo());
        }

        [HttpPut]
        public override async Task<ActionResult> Update(RegisterTokenInfo model)
        {
            if (!(await GetUser()).IsOperator) return Forbid();

            return await Task.FromResult(BadRequest());
        }
    }
}
