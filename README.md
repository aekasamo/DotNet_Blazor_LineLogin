dotnet new MyWebSite --output blazor

dotnet add package Microsoft.AspNetCore.Authentication.OpenIdConnect

config Program.cs AddOpenIdConnect

Add Login Page

Add Auth Folder
Add AuthEndpoint.cs

config Program.cs app.MapAuthEndpoints();

