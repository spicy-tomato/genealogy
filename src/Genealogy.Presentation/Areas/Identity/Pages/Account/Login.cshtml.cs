using Genealogy.Domain.Postgres.Models;
using Genealogy.Presentation.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Genealogy.Presentation.Areas.Identity.Pages.Account;

public class LoginModel : PageModel
{
    public LoginModel(SignInManager<User> signInManager, ILogger<LoginModel> logger)
    {
    }

    public IList<LoginMethod> LoginMethods { get; set; } = new List<LoginMethod> { LoginMethod.Google };

    public IActionResult OnGet() => Page();
}