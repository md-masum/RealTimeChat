using Api.Extensions;
using Core.Helpers;
using Core.Middleware;
using Microsoft.AspNetCore.ResponseCompression;
using Serilog;
using Serilog.Events;
using Service.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Logs");
builder.Services.AddSignalR();
if (!Directory.Exists(pathBuilt))
{
    Directory.CreateDirectory(pathBuilt);
}

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.File(pathBuilt + "\\log.txt", rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 5242880,
        rollOnFileSizeLimit: true)
    // .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning));

builder.Services.AddSingleton(builder.Configuration.GetSection("MailSettings").Get<MailSettings>());
builder.Services.AddSingleton(builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.RegisterDependency(builder.Configuration);

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});


var app = builder.Build();
app.UseResponseCompression();
app.UseSerilogRequestLogging();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseGlobalErrorHandlingMiddleware();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chathub");
});

app.Run();
