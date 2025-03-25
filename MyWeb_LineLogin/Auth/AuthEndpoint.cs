using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MyWeb_LineLogin.Auth
{
    public static class AuthEndpoint
    {
        public static string ReturnUrl = string.Empty;
        public static IEndpointConventionBuilder MapAuthEndpoints(this IEndpointRouteBuilder builder)
        {
            var accountGroup = builder.MapGroup("/authentication");

            accountGroup.MapPost("externallogin",
                async (HttpContext context, [FromForm] string provider, [FromForm] string returnUrl) =>
                {
                    ReturnUrl = returnUrl;
                    var authProp = new AuthenticationProperties
                    {
                        RedirectUri = string.Format("authentication/{0}", provider)
                    };
                    var result = TypedResults.Challenge(authProp, [provider]);
                    await result.ExecuteAsync(context);
                });

            accountGroup.MapGet("/line",
                async (HttpContext context) =>
                {
                    if (!context.User.Identity!.IsAuthenticated)
                        return Results.Unauthorized();

                    var lineID = context.User.Claims.FirstOrDefault(x=> x.Type == ClaimTypes.NameIdentifier)!.Value;

                    if (lineID is null)
                        return Results.Unauthorized();

                    Claim[] claims = 
                        [
                            new (ClaimTypes.NameIdentifier, lineID)
                        ]; 
                    var identity = new ClaimsIdentity(claims, "LineLogin");
                    var principal = new ClaimsPrincipal(identity);
                    await context.SignInAsync(principal);

                    var returnLink = (ReturnUrl == "") ? "/" : ReturnUrl;
                    return Results.LocalRedirect($"~{returnLink}");
                });

            accountGroup.MapGet("/logout"
                , (HttpContext context) =>
                {
                    context.SignOutAsync("LineLogin");
                    return Results.LocalRedirect("~/login");
                });
            return accountGroup;
        }
    }
}
