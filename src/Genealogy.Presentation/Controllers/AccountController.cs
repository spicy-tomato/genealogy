using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Genealogy.Presentation.Controllers;

[Route("Account")]
public class AccountController : Controller
{
    [HttpPost("FirebaseLogin")]
    public async Task<IActionResult> FirebaseLogin([FromBody] TokenDto dto)
    {
        try
        {
            FirebaseToken decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(dto.IdToken);
            string uid = decoded.Uid;
            var email = decoded.Claims["email"]?.ToString();

            // You can now store the user info in session or create an auth cookie
            // For example:
            HttpContext.Session.SetString("uid", uid);
            HttpContext.Session.SetString("email", email ?? string.Empty);

            return Json(new { success = true, uid, email });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { success = false, message = ex.Message });
        }
    }

    public class TokenDto
    {
        public string IdToken { get; set; }
    }
}