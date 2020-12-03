using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TewiClipool.Server.Data;
using TewiClipool.Server.Models;
using TewiClipool.Shared;
using TewiClipool.Shared.Models;

namespace TewiClipool.Server.Controllers
{
    [ApiController]
    [Route(Literal.ApiUrls.User)]
    [Authorize(Roles = Literal.Roles.User)]
    public class UserController : DbControllerBase<UserInfo, string>
    {
        private readonly UserManager<AppUser> userManager;
        public UserController(ApplicationDbContext db,
            ILogger<DbControllerBase<UserInfo, string>> logger,
            HtmlEncoder htmlEncoder,
            UserManager<AppUser> userManager) : base(db, logger, htmlEncoder)
        {
            this.userManager = userManager;
        }

        /// <summary>
        /// You CANNOT create userinfo here
        /// </summary>
        /// <returns>BadRequest 400</returns>
        [HttpPost]
        public override async Task<ActionResult<string>> Create(UserInfo model)
        {
            return await Task.FromResult(Forbid());
        }

        /// <summary>
        /// Just BAN a person if you hate him, or a cascade delete'll happen.
        /// </summary>
        /// <returns>BadRequest 400</returns>
        [HttpDelete("id/{id}")]
        [Authorize(Roles = Literal.Roles.Operator)]
        public override async Task<ActionResult> Delete(string id)
        {
            if (!(await GetUser()).IsOperator) return Forbid();
            return BadRequest();

            //var user = await db.Users
            //    .Where(u => u.Id == id)
            //    .SingleOrDefaultAsync();

            //if (user == null)
            //{
            //    return await Task.FromResult(NotFound());
            //}

            //logger.LogInformation($"User {user.Id} deleted");

            //db.Users.Remove(user);
            //await db.SaveChangesAsync();

            //return Ok();
        }

        [HttpGet]
        public override async Task<ActionResult<List<UserInfo>>> Get()
        {
            if (!(await GetUser()).IsUser) return Forbid();

            List<UserInfo> result = new List<UserInfo>();
            (await db.Users.ToListAsync()).ForEach(user => result.Add(user.ToUserInfo()));

            return Ok(result);
        }

        [HttpGet("id/{id}")]
        public override async Task<ActionResult<UserInfo>> Get(string id)
        {
            if (!(await GetUser()).IsUser) return Forbid();

            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

            return user == null ? NotFound() : Ok(user.ToUserInfo());
        }

        [HttpGet("username/{username}")]
        public async Task<ActionResult<UserInfo>> GetByUserName(string username)
        {
            if (!(await GetUser()).IsUser) return Forbid();

            var user = await db.Users.FirstOrDefaultAsync(u => u.UserName == username);

            return user == null ? NotFound() : Ok(user.ToUserInfo());
        }

        [HttpPut("role")]
        [Authorize(Roles = Literal.Roles.Operator)]
        public async Task<ActionResult> ConfigRole(UserRoleInfo info)
        {
            if (!(await GetUser()).IsOperator) return Forbid();

            var user = await db.Users.FirstOrDefaultAsync(u => u.UserName == info.UserName);

            if (user == null) return NotFound();

            if (info.IsAdd)
            {
                await userManager.AddToRoleAsync(user, Enum.GetName(info.StaffType));
            }
            else
            {
                await userManager.RemoveFromRoleAsync(user, Enum.GetName(info.StaffType));
            }

            switch (info.StaffType)
            {
                case StaffType.Editor:
                    user.IsEditor = info.IsAdd;
                    break;
                case StaffType.Translator:
                    user.IsTranslator = info.IsAdd;
                    break;
                case StaffType.Timeline:
                    user.IsTimeline = info.IsAdd;
                    break;
                case StaffType.Reviewer:
                    user.IsReviewer = info.IsAdd;
                    break;
                case StaffType.PostProducer:
                    user.IsPostProducer = info.IsAdd;
                    break;
                case StaffType.Art:
                    user.IsArt = info.IsAdd;
                    break;
                default:
                    break;
            }

            db.Users.Update(user);
            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("op")]
        [Authorize(Roles = Literal.Roles.Admin)]
        public async Task<ActionResult> ConfigOp(UserOpInfo info)
        {
            if (!(await userManager.IsInRoleAsync(await GetUser(), Literal.Roles.Admin))) return Forbid();

            var user = await db.Users.FirstOrDefaultAsync(u => u.UserName == info.UserName);

            if (user == null) return NotFound();

            if (info.IsAdd)
            {
                if (!user.IsUser) return BadRequest();

                await userManager.AddToRoleAsync(user, Literal.Roles.Operator);
                user.IsOperator = true;
            }
            else
            {
                await userManager.RemoveFromRoleAsync(user, Literal.Roles.Operator);
                user.IsOperator = false;
            }
            db.Users.Update(user);
            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("ban/{username}")]
        [Authorize(Roles = Literal.Roles.Operator)]
        public async Task<ActionResult<bool>> ToggleBanByUsername(string username)
        {
            if (!(await GetUser()).IsOperator) return Forbid();

            var user = await db.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null) return NotFound();

            bool isop = await userManager.IsInRoleAsync(user, Literal.Roles.Operator);
            if (isop) return BadRequest();

            if (await userManager.IsInRoleAsync(user, Literal.Roles.User))
            {
                await userManager.RemoveFromRoleAsync(user, Literal.Roles.User);
                user.IsUser = false;
            }
            else
            {
                await userManager.AddToRoleAsync(user, Literal.Roles.User);
                user.IsUser = true;
            }
            db.Users.Update(user);
            await db.SaveChangesAsync();

            return Ok(user.IsUser);
        }

        [HttpGet("mail/{username}")]
        [Authorize(Roles = Literal.Roles.Operator)]
        public async Task<ActionResult<string>> GetEmail(string username)
        {
            if (!(await GetUser()).IsOperator) return Forbid();

            var user = await db.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.Email);
        }

        [HttpGet("reset/{username}")]
        [Authorize(Roles = Literal.Roles.Admin)]
        public async Task<ActionResult<bool>> ResetPassword(string username)
        {
            if (!(await userManager.IsInRoleAsync(await GetUser(), Literal.Roles.Admin))) return Forbid();

            var user = await db.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                return NotFound();
            }

            // Reset To 'Aa1234'
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, "Aa1234");

            logger.LogInformation($"Reset User [{user.UserName}]'s password to Aa1234");
            db.Users.Update(user);
            await db.SaveChangesAsync();

            return true;
        }


        /// <summary>
        /// You CANNOT update userinfo here
        /// </summary>
        /// <returns>BadRequest 400</returns>
        [HttpPut]
        public override async Task<ActionResult> Update(UserInfo model)
        {
            return await Task.FromResult(Forbid());
        }
    }
}
