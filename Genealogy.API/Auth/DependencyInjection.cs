using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

namespace Genealogy.API.Auth;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddAuth(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddGoogle(builder.Configuration);
        
        builder.Services.AddAuthorization();

        return builder;
    }

    private static void AddGoogle(this AuthenticationBuilder builder, ConfigurationManager configuration)
    {
        string? clientId = configuration["Authentication:Google:ClientId"];
        string? clientSecret = configuration["Authentication:Google:ClientSecret"];

        if (clientId == null || clientSecret == null)
        {
            throw new InvalidOperationException("ClientId or ClientSecret not configured");
        }

        builder.AddGoogle(options =>
        {
            options.ClientId = clientId;
            options.ClientSecret = clientSecret;
            options.CallbackPath = "/sign-in/google";
        });
    }
}