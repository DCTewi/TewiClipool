using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TewiClipool.Server.Data;
using TewiClipool.Shared;
using TewiClipool.Shared.Models;

namespace TewiClipool.Server.Controllers
{
    [ApiController]
    [Route(Literal.ApiUrls.Clip)]
    [Authorize]
    public class ClipController : DbControllerBase<Clip>
    {
        public ClipController(ApplicationDbContext db,
            ILogger<DbControllerBase<Clip, int>> logger,
            HtmlEncoder htmlEncoder) : base(db, logger, htmlEncoder)
        {
        }

        [HttpPost]
        [Authorize(Roles = Literal.Roles.User)]
        public override async Task<ActionResult<int>> Create(Clip model)
        {
            if (!(await GetUser()).IsUser) return Forbid();

            model.CreateTime = DateTime.Now;
            model.CreateUserName = await GetUserName();
            model.Title = model.Title;
            model.Description = model.Description;
            model.OriginInfo = model.OriginInfo;

            var clip = await db.Clips.AddAsync(model);
            await db.SaveChangesAsync();

            return Ok(clip.Entity.Id);
        }

        [HttpDelete("id/{id}")]
        [Authorize(Roles = Literal.Roles.User)]
        public override async Task<ActionResult> Delete(int id)
        {
            if (!(await GetUser()).IsUser) return Forbid();

            var clip = await db.Clips.FindAsync(id);

            var permission = clip.CreateUserName == await GetUserName();

            if (permission || HttpContext.User.IsInRole(Literal.Roles.Operator))
            {
                logger.LogInformation($"Clip {id} is deleted");

                db.Clips.Remove(clip);
                await db.SaveChangesAsync();
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet]
        public override async Task<ActionResult<List<Clip>>> Get()
        {
            if (!(await GetUser()).IsUser) return Forbid();

            return Ok(await db.Clips.ToListAsync());
        }

        [HttpGet("id/{id}")]
        public override async Task<ActionResult<Clip>> Get(int id)
        {
            if (!(await GetUser()).IsUser) return Forbid();

            var clip = await db.Clips.FindAsync(id);

            return clip == null ? NotFound() : Ok(clip);
        }

        [HttpGet("username/{username}")]
        public async Task<ActionResult<List<Clip>>> GetByUsername(string username)
        {
            if (!(await GetUser()).IsUser) return Forbid();

            var clips = await db.Clips
                .Where(c => c.CreateUserName == username)
                .ToListAsync();

            return Ok(clips);
        }

        [HttpPut]
        [Authorize(Roles = Literal.Roles.User)]
        public override async Task<ActionResult> Update(Clip model)
        {
            if (!(await GetUser()).IsUser) return Forbid();

            var clip = await db.Clips.FindAsync(model.Id);
            var permission = clip.CreateUserName == await GetUserName();

            if (permission || HttpContext.User.IsInRole(Literal.Roles.Operator))
            {
                clip.Type = model.Type;
                clip.Priority = model.Priority;
                clip.Title = model.Title;
                clip.OriginInfo = model.OriginInfo;
                clip.Description = model.Description;
                clip.Length = model.Length;
                clip.PublishAddress = model.PublishAddress;
                clip.StaffItems = model.StaffItems;
                clip.Comments = model.Comments;

                db.Clips.Update(clip);
                await db.SaveChangesAsync();
                return Ok();
            }

            return BadRequest();
        }
    }
}
