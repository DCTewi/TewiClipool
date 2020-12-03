using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TewiClipool.Server.Data;
using TewiClipool.Shared;
using TewiClipool.Shared.Models;

namespace TewiClipool.Server.Controllers
{
    [ApiController]
    [Route(Literal.ApiUrls.Announce)]
    [Authorize(Roles = Literal.Roles.User)]
    public class AnnounceController : DbControllerBase<AnnounceInfo>
    {
        public AnnounceController(ApplicationDbContext db,
            ILogger<DbControllerBase<AnnounceInfo, int>> logger,
            HtmlEncoder htmlEncoder) : base(db, logger, htmlEncoder)
        {
        }

        [HttpPost]
        [Authorize(Roles = Literal.Roles.Operator)]
        public override async Task<ActionResult<int>> Create(AnnounceInfo model)
        {
            if (!(await GetUser()).IsOperator) return Forbid();

            model.PublishTime = DateTime.Now;
            model.UserName = await GetUserName();
            model.Title = model.Title;
            //model.RawHtml = htmlEncoder.Encode(model.RawHtml);

            var announce = await db.AnnounceInfos.AddAsync(model);
            await db.SaveChangesAsync();

            return Ok(announce.Entity.Id);
        }

        [HttpDelete("id/{id}")]
        [Authorize(Roles = Literal.Roles.Operator)]
        public override async Task<ActionResult> Delete(int id)
        {
            if (!(await GetUser()).IsOperator) return Forbid();

            var announce = await db.AnnounceInfos.FirstOrDefaultAsync(a => a.Id == id);

            logger.LogInformation($"Announce {announce.Id} deleted");

            db.AnnounceInfos.Remove(announce);
            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public override async Task<ActionResult<List<AnnounceInfo>>> Get()
        {
            if (!(await GetUser()).IsUser) return Forbid();

            return Ok(await db.AnnounceInfos.ToListAsync());
        }

        [HttpGet("id/{id}")]
        public override async Task<ActionResult<AnnounceInfo>> Get(int id)
        {
            if (!(await GetUser()).IsUser) return Forbid();

            var announce = await db.AnnounceInfos.FirstOrDefaultAsync(a => a.Id == id);

            return announce == null ? NotFound() : Ok(announce);
        }

        [HttpPut]
        [Authorize(Roles = Literal.Roles.Operator)]
        public override async Task<ActionResult> Update(AnnounceInfo model)
        {
            if (!(await GetUser()).IsOperator) return Forbid();

            model.PublishTime = DateTime.Now;
            model.UserName = await GetUserName();
            model.Title = model.Title;

            db.AnnounceInfos.Update(model);
            await db.SaveChangesAsync();

            return Ok();
        }
    }
}
