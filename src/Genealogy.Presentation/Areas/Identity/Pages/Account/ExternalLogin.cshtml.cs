using FirebaseAdmin.Auth;
using Genealogy.Domain.Postgres.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Genealogy.Presentation.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ExternalLoginModel : PageModel
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<ExternalLoginModel> _logger;

    public ExternalLoginModel(SignInManager<User> signInManager, UserManager<User> userManager,
        ILogger<ExternalLoginModel> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    public IActionResult OnGet() => Page();

    public async Task<IActionResult> OnPost([FromBody] FirebaseTokenDto firebaseTokenDto)
    {
        try
        {
            FirebaseToken? decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(firebaseTokenDto.IdToken);
            string email = decodedToken.Claims["email"]?.ToString() ?? string.Empty;
            var name = decodedToken.Claims["name"]?.ToString();

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            // Try to find existing user
            User? user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new User(name, email);
                IdentityResult result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(new { error = "User creation failed." });
                }
            }

            // Sign in user
            await _signInManager.SignInAsync(user, isPersistent: false);

            return new JsonResult(new { success = true });
        }
        catch (Exception ex)
        {
            return Unauthorized();
        }
    }

    public class FirebaseTokenDto
    {
        public required string IdToken { get; set; }
    }
}