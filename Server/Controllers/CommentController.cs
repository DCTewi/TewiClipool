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
    [Route(Literal.ApiUrls.Comment)]
    [Authorize]
    public class CommentController : DbControllerBase<Comment>
    {
        public CommentController(ApplicationDbContext db,
            ILogger<DbControllerBase<Comment, int>> logger,
            HtmlEncoder htmlEncoder) : base(db, logger, htmlEncoder)
        {
        }

        [HttpPost]
        [Authorize(Roles = Literal.Roles.User)]
        public override async Task<ActionResult<int>> Create(Comment model)
        {
            if (!(await GetUser()).IsUser) return Forbid();

            var clip = await db.Clips.FirstOrDefaultAsync(c => c.Id == model.ClipId);

            if (clip == null) return NotFound();

            model.PublishTime = DateTime.Now;
            model.UserName = await GetUserName();
            model.Content = model.Content;
            model.Edited = false;

            var comment = await db.Comments.AddAsync(model);
            await db.SaveChangesAsync();

            return Ok(comment.Entity.Id);
        }

        [HttpDelete("id/{id}")]
        [Authorize(Roles = Literal.Roles.User)]
        public override async Task<ActionResult> Delete(int id)
        {
            if (!(await GetUser()).IsUser) return Forbid();

            var comment = await db.Comments.FindAsync(id);

            var permission = comment.UserName == await GetUserName();

            if (permission || HttpContext.User.IsInRole(Literal.Roles.Operator))
            {
                logger.LogInformation($"Comment {id} is deleted");

                db.Comments.Remove(comment);
                await db.SaveChangesAsync();
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet]
        public override async Task<ActionResult<List<Comment>>> Get()
        {
            return Ok(await db.Comments.ToListAsync());
        }

        [HttpGet("id/{id}")]
        public override async Task<ActionResult<Comment>> Get(int id)
        {
            var comment = await db.Comments.FindAsync(id);

            return comment == null ? NotFound() : Ok(comment);
        }

        [HttpGet("clipid/{clipid}")]
        public async Task<ActionResult<List<Comment>>> GetByClipId(int clipid)
        {
            var result = await db.Comments
                .Where(c => c.ClipId == clipid)
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("username/{username}")]
        public async Task<ActionResult<List<Comment>>> GetByUserName(string username)
        {
            var result = await db.Comments
                .Where(c => c.UserName == username)
                .ToListAsync();

            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = Literal.Roles.User)]
        public override async Task<ActionResult> Update(Comment model)
        {
            if (!(await GetUser()).IsUser) return Forbid();

            var comment = await db.Comments.FindAsync(model.Id);
            var permission = comment.UserName == await GetUserName();

            if (permission || HttpContext.User.IsInRole(Literal.Roles.Operator))
            {
                comment.Type = model.Type;
                comment.Content = model.Content;
                comment.Edited = true;

                db.Comments.Update(comment);
                await db.SaveChangesAsync();
                return Ok();
            }

            return BadRequest();
        }
    }
}
