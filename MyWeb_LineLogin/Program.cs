using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using MyWeb_LineLogin.Auth;
using MyWeb_LineLogin.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication("LineLogin")
    .AddCookie("LineLogin", options =>
    {
        options.Cookie.Name = "LineLogin";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.LoginPath = "/login";
    }).AddOpenIdConnect("Line", options =>
    {
        options.Authority = "https://access.line.me";
        options.ClientId = "your_line_clientid";
        options.ClientSecret = "your_line_secret";
        options.CallbackPath = "/signin-line";
        options.ResponseType = "code";
        options.SaveTokens = true;

        options.Events = new OpenIdConnectEvents()
        {
            OnAuthorizationCodeReceived = context =>
            {
                context.TokenEndpointRequest?.SetParameter("id_token_key_type", "JWK");
                return Task.CompletedTask;
            }
        };

    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthentication();

app.MapAuthEndpoints();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
