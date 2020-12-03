using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TewiClipool.Server.Areas.Identity.Pages.Account
{
    public class RedirectToManageIndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToPage("Manage/Index");
        }
    }
}
