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
    [Route(Literal.ApiUrls.Staff)]
    [Authorize]
    public class StaffController : DbControllerBase<StaffItem>
    {
        public StaffController(ApplicationDbContext db,
            ILogger<DbControllerBase<StaffItem, int>> logger,
            HtmlEncoder htmlEncoder) : base(db, logger, htmlEncoder)
        {
        }

        [HttpPost]
        [Authorize(Roles = Literal.Roles.User)]
        public override async Task<ActionResult<int>> Create(StaffItem model)
        {
            if (!(await GetUser()).IsUser) return Forbid();

            if (!HttpContext.User.IsInRole(Enum.GetName(model.Type)))
            {
                return BadRequest();
            }

            var clip = await db.Clips.FirstOrDefaultAsync(c => c.Id == model.ClipId);

            if (clip == null ||
                clip.Priority == PriorityType.Completed ||
                clip.Priority == PriorityType.Desperated)
            {
                return BadRequest();
            }

            model.Process = ProcessType.Accepted;
            model.AcceptTime = DateTime.Now;
            model.Description = model.Description;
            model.StaffNickName = model.StaffNickName;
            model.UserName = await GetUserName();

            var staff = await db.StaffItems.AddAsync(model);
            await db.SaveChangesAsync();

            return Ok(staff.Entity.Id);
        }


        [HttpDelete("id/{id}")]
        [Authorize(Roles = Literal.Roles.User)]
        public override async Task<ActionResult> Delete(int id)
        {
            if (!(await GetUser()).IsUser) return Forbid();

            var staff = await db.StaffItems.FindAsync(id);

            var permission = staff.UserName == await GetUserName();

            if (permission || HttpContext.User.IsInRole(Literal.Roles.Operator))
            {
                logger.LogInformation($"Staff {id} is deleted");

                db.StaffItems.Remove(staff);
                await db.SaveChangesAsync();
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet]
        public override async Task<ActionResult<List<StaffItem>>> Get()
        {
            if (!(await GetUser()).IsUser) return Forbid();

            return Ok(await db.StaffItems.ToListAsync());
        }

        [HttpGet("id/{id}")]
        public override async Task<ActionResult<StaffItem>> Get(int id)
        {
            if (!(await GetUser()).IsUser) return Forbid();

            var staff = await db.StaffItems.FindAsync(id);

            return staff == null ? NotFound() : Ok(staff);
        }

        [HttpGet("clip/{clipid}")]
        public async Task<ActionResult<StaffItem>> GetByClipId(int clipid)
        {
            if (!(await GetUser()).IsUser) return Forbid();

            var clip = await db.Clips.FirstOrDefaultAsync(c => c.Id == clipid);

            if (clip == null)
            {
                return NotFound();
            }

            var staff = await db.StaffItems
                .Where(s => s.ClipId == clipid)
                .ToListAsync();

            return staff == null ? NotFound() : Ok(staff);
        }

        [HttpGet("sage/{id}")]
        public async Task<ActionResult<bool>> ToggleSage(int id)
        {
            if (!(await GetUser()).IsUser) return Forbid();

            var userid = GetUserId();
            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userid);
            var staff = await db.StaffItems.FirstOrDefaultAsync(s => s.Id == id);

            if (user == null || staff == null)
            {
                return BadRequest();
            }

            bool permission = user.IsReviewer || user.IsOperator;

            if (permission)
            {
                staff.ToggleSage();
                staff.SagedUser = await GetUserName();
                db.StaffItems.Update(staff);
                await db.SaveChangesAsync();
            }

            return Ok(staff.Saged);
        }

        [HttpPut]
        [Authorize(Roles = Literal.Roles.User)]
        public override async Task<ActionResult> Update(StaffItem model)
        {
            if (!(await GetUser()).IsUser) return Forbid();

            var staff = await db.StaffItems.FindAsync(model.Id);
            var clip = await db.Clips.FirstOrDefaultAsync(c => c.Id == staff.ClipId);
            var permission = staff.UserName == await GetUserName();

            if (permission || HttpContext.User.IsInRole(Literal.Roles.Operator))
            {
                staff.Description = model.Description;
                staff.StaffNickName = model.StaffNickName;
                staff.ResAddress = model.ResAddress;

                if (clip.Priority != PriorityType.Desperated &&
                    clip.Priority != PriorityType.Completed)
                {
                    if (staff.Process != ProcessType.Completed && model.Process == ProcessType.Completed)
                    {
                        staff.CompletedTime = DateTime.Now;
                    }
                    staff.Process = model.Process;
                }

                db.StaffItems.Update(staff);
                await db.SaveChangesAsync();
                return Ok();
            }

            return BadRequest();
        }
    }
}
