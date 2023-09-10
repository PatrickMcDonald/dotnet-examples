using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

var app = builder.Build();

app.UseAuthentication();

app.MapGet("/", (HttpContext context) =>
{
    return context.User.FindFirstValue("sub") is { } userName
        ? $"you are signed in as {userName}"
        : "you are not signed in";
});

app.MapGet("signIn", async (HttpContext context, string userName) =>
{
    var user = CreateUser(userName);
    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);

    ////return Results.Json(user, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });
    return $"you are signed in as {userName}";
});

app.MapGet("/signOut", async (HttpContext context) =>
{
    await context.SignOutAsync();

    return "you are signed out";
});

////app.Run((HttpContext context) => Task.FromResult("not found"));

app.Run();

return;

static ClaimsPrincipal CreateUser(string userName)
{
    var claims = new Claim[] { new("sub", userName) };
    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    return new ClaimsPrincipal(identity);
}
