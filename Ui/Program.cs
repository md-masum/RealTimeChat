using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Ui;
using Ui.AuthProviders;
using Ui.HttpRepository;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
var value = builder.Configuration.GetSection("Syncfusion").Value;
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(builder.Configuration.GetSection("Syncfusion").Value);
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7280") }.EnableIntercept(sp));
builder.Services.AddHttpClientInterceptor();
builder.Services.AddScoped<HttpInterceptorService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<IChatService, ChatService>();
builder.Services.AddSyncfusionBlazor(options => { options.IgnoreScriptIsolation = true; });

await builder.Build().RunAsync();
