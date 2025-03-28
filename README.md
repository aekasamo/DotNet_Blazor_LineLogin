dotnet new MyWebSite --output aspnetcoreapp


dotnet add package Microsoft.AspNetCore.Authentication.OpenIdConnect

config Program.cs AddOpenIdConnect

Add Login Page

Add Auth Folder
Add AuthEndpoint.cs

config Program.cs app.MapAuthEndpoints();

